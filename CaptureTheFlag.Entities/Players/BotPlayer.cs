using System;
using System.Collections.Generic;
using CaptureTheFlag.Entities.Screens;
using Engine.AI.Behaviors.Attack;
using Engine.AI.Behaviors.Movement;
using Engine.Camera;
using Engine.Content.Fonts;
using Engine.Drawing;
using Engine.Entities;
using Engine.Entities.Graphs;
using Engine.Physics.Bodies;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace CaptureTheFlag.Entities.Players
{
    public class BotPlayer : AbstractPlayer
    {
        /// <summary>
        /// Movement behavior
        /// </summary>
        private readonly MovementBehavior _movementBehavior;

        /// <summary>
        /// Attack behavior
        /// </summary>
        private readonly AttackBehavior _attackBehavior;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        /// <param name="team"></param>
        /// <param name="enemyTeam"></param>
        public BotPlayer(AbstractBody body, PrimitiveBuilder model, AbstractTeam team, AbstractTeam enemyTeam)
            : base(body, model, team, enemyTeam)
        {
            SetCollisionBehaviors(new CollisionBehaviors(this));
            _movementBehavior = new MovementBehavior();
            _attackBehavior = new AttackBehavior(AppSettingsFacade.BotReactionTimeInMilliseconds);
        }

        /// <summary>
        /// Creates the "default" instance of this class
        /// </summary>
        /// <param name="color"></param>
        /// <param name="team"></param>
        /// <param name="otherTeam"></param>
        /// <returns></returns>
        public static BotPlayer Default(Color color, AbstractTeam team, AbstractTeam otherTeam)
        {
            CircleBody body = BodyFactory.Circle(false,
                AppSettingsFacade.PlayerMass,
                AppSettingsFacade.PlayerMaxSpeed,
                AppSettingsFacade.PlayerFrictionCoefficient,
                true,
                AppSettingsFacade.PlayerRadius);

            PrimitiveBuilder model = new PrimitiveBuilder(color);
            model.Add(PrimitiveFactory.Circle(color, AppSettingsFacade.PlayerRadius, 3));
            model.Add(PrimitiveFactory.Line(color, Vector2.Zero, Vector2.UnitX * body.GetRadius()));

            BotPlayer result = new BotPlayer(body, model, team, otherTeam);
            team.AddPlayer(result);

            return result;
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (IsAlive)
            {
                UpdateMovementBehavior();

                _attackBehavior.Target = GameScreen.Instance.Map.GetClosestPlayer(EnemyTeam, Body.GetPosition());

                bool canSeeTarget = false;

                if (_attackBehavior.Target != null)
                {
                    // Check for a clear line of sight from the bot to its target
                    canSeeTarget =
                        !GameScreen.Instance.Map.CheckLineCollision(Body.GetPosition(),
                            _attackBehavior.Target.GetBody().GetPosition());

                    // Check that the bot's viewport, which should match the dimensions of the player's viewport (no bot cheating), contains the target
                    canSeeTarget &= Math.Abs(Body.GetPosition().X - _attackBehavior.Target.GetBody().GetPosition().X) <
                                    (float) AppSettingsFacade.WindowWidth / 2;
                    canSeeTarget &= Math.Abs(Body.GetPosition().Y - _attackBehavior.Target.GetBody().GetPosition().Y) <
                                    (float) AppSettingsFacade.WindowHeight / 2;
                }

                _attackBehavior.Update(Body.GetPosition(), GetGun(), canSeeTarget);
            }
        }

        /// <summary>
        /// Update movement behavior
        /// </summary>
        protected void UpdateMovementBehavior()
        {
            // Priorities
            // 1. Acquire weapon
            // 2. Retrieve flag if the enemy team stole it
            // 3. Pursue other team's flag
            //     a. Go back to base if you have flag

            AbstractGun closestGun = GameScreen.Instance.Map.GetClosestGun(Body.GetPosition());

            if (!_movementBehavior.HasGoal())
            {
                // If the bot does not have a gun he should seek one right away
                if (Gun == null && closestGun != null)
                {
                    Vector2 closestGunPosition = closestGun.GetBody().GetPosition();
                    _movementBehavior.SetGoal(closestGunPosition, PlanRoute(closestGunPosition));
                }
                else if (Gun != null && !Team.GetFlag().IsReset())
                {
                    Vector2 flagPosition = Team.GetFlag().GetBody().GetPosition();
                    _movementBehavior.SetGoal(flagPosition, PlanRoute(flagPosition));
                }
                else if (!HasFlag())
                {
                    Vector2 enemyFlagPosition = EnemyTeam.GetFlag().GetBody().GetPosition();
                    _movementBehavior.SetGoal(enemyFlagPosition, PlanRoute(enemyFlagPosition));
                }
                else if (HasFlag())
                {
                    Vector2 teamBasePosition = Team.GetBasePosition();
                    _movementBehavior.SetGoal(teamBasePosition, PlanRoute(teamBasePosition));
                }
            }
            else if (CanSee(_movementBehavior.GetGoal()))
            {
                _movementBehavior.SetTarget(_movementBehavior.GetGoal());
            }

            _movementBehavior.Update(Body.GetPosition(), Body.GetVelocity());

            Vector2 force = _movementBehavior.GetForce(Body.GetPosition(), Body.GetVelocity());
            Body.ApplyForce(force);
        }

        /// <summary>
        /// Plan route from current position to goal position
        /// </summary>
        /// <param name="goalPosition"></param>
        /// <returns></returns>
        protected List<Node> PlanRoute(Vector2 goalPosition)
        {
            Vector2 currentPosition = GetBody().GetPosition();

            return GameScreen.Instance.Map.GetShortestRoute(currentPosition, goalPosition);
        }

        /// <summary>
        /// Can the bot "see" the given target position?
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected bool CanSee(Vector2 target)
        {
            return !GameScreen.Instance.Map.CheckLineCollision(Body.GetPosition(), target);
        }

        /// <summary>
        /// Die
        /// </summary>
        /// <param name="where"></param>
        public override void Die(Vector2 where)
        {
            base.Die(where);
            _movementBehavior.Reset();
            _attackBehavior.Reset();
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, SpriteBatch spriteBatch)
        {
            base.Draw(graphicsDevice, basicEffect, spriteBatch);

            if (AppSettingsFacade.IsPathfindingDebugModeOn)
            {
                Vector2 target = _movementBehavior.GetTarget();
                spriteBatch.DrawString(SpriteFontRepository.Instance.Get("debug"),
                    "Movement target: <" + target.X + ", " + target.Y + ">",
                    Body.GetPosition() - Camera2D.Instance.GetPosition() + new Vector2(30f, 0f), Color.Yellow);
            }

            if (AppSettingsFacade.IsPathfindingDebugModeOn)
            {
                _movementBehavior.Draw(graphicsDevice, basicEffect, spriteBatch);
            }
        }
    }
}
