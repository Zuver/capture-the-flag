using CaptureTheFlag.Entities.Screens;
using CaptureTheFlag.Entities.Walls;
using Engine.AI.Behaviors.Attack;
using Engine.AI.Behaviors.Movement;
using Engine.Camera;
using Engine.Content;
using Engine.Drawing;
using Engine.Entities;
using Engine.Entities.Graphs;
using Engine.Physics.Bodies;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaptureTheFlag.Entities.Players
{
    public class BotPlayer : AbstractPlayer
    {
        /// <summary>
        /// Movement behavior
        /// </summary>
        private MovementBehavior MovementBehavior;

        /// <summary>
        /// Attack behavior
        /// </summary>
        private AttackBehavior AttackBehavior;

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
            this.SetCollisionBehaviors(new CollisionBehaviors(this));
            this.MovementBehavior = new MovementBehavior();
            this.AttackBehavior = new AttackBehavior();
        }

        /// <summary>
        /// Creates the "default" instance of this class
        /// </summary>
        /// <param name="color"></param>
        /// <param name="team"></param>
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

            this.UpdateMovementBehavior();

            AbstractPlayer closestEnemyPlayer = GameScreen.Instance.Map.GetClosestPlayer(this.EnemyTeam, this.Body.GetPosition());
            this.AttackBehavior.Target = closestEnemyPlayer;

            bool canSeeTarget = !GameScreen.Instance.Map.CheckLineCollision(this.Body.GetPosition(), this.AttackBehavior.Target.GetBody().GetPosition());
            canSeeTarget &= Math.Abs(this.Body.GetPosition().X - this.AttackBehavior.Target.GetBody().GetPosition().X) < AppSettingsFacade.WindowWidth / 2;
            canSeeTarget &= Math.Abs(this.Body.GetPosition().Y - this.AttackBehavior.Target.GetBody().GetPosition().Y) < AppSettingsFacade.WindowHeight / 2;

            this.AttackBehavior.Update(this.Body.GetPosition(), this.GetGun(), canSeeTarget);
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

            AbstractGun closestGun = GameScreen.Instance.Map.GetClosestGun(this.Body.GetPosition());

            if (!this.MovementBehavior.HasGoal())
            {
                // If the bot does not have a gun he should seek one right away
                if (this.Gun == null && closestGun != null)
                {
                    Vector2 closestGunPosition = closestGun.GetBody().GetPosition();
                    this.MovementBehavior.SetGoal(closestGunPosition, this.PlanRoute(closestGunPosition));
                }
                else if (this.Gun != null && !this.Team.GetFlag().IsReset())
                {
                    Vector2 flagPosition = this.Team.GetFlag().GetBody().GetPosition();
                    this.MovementBehavior.SetGoal(flagPosition, this.PlanRoute(flagPosition));
                }
                else if (!this.HasFlag())
                {
                    Vector2 enemyFlagPosition = this.EnemyTeam.GetFlag().GetBody().GetPosition();
                    this.MovementBehavior.SetGoal(enemyFlagPosition, this.PlanRoute(enemyFlagPosition));
                }
                else if (this.HasFlag())
                {
                    Vector2 teamBasePosition = this.Team.GetBasePosition();
                    this.MovementBehavior.SetGoal(teamBasePosition, this.PlanRoute(teamBasePosition));
                }
            }
            else if (this.CanSee(this.MovementBehavior.GetGoal()))
            {
                this.MovementBehavior.SetTarget(this.MovementBehavior.GetGoal());
            }

            this.MovementBehavior.Update(this.Body.GetPosition(), this.Body.GetVelocity());

            Vector2 force = this.MovementBehavior.GetForce(this.Body.GetPosition(), this.Body.GetVelocity());
            this.Body.ApplyForce(force);
        }

        /// <summary>
        /// Plan route from current position to goal position
        /// </summary>
        /// <param name="goalPosition"></param>
        /// <returns></returns>
        protected List<Node> PlanRoute(Vector2 goalPosition)
        {
            Vector2 currentPosition = this.GetBody().GetPosition();

            return GameScreen.Instance.Map.GetShortestRoute(currentPosition, goalPosition);
        }
        
        /// <summary>
        /// Can the bot "see" the given target position?
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected bool CanSee(Vector2 target)
        {
            return !GameScreen.Instance.Map.CheckLineCollision(this.Body.GetPosition(), target);
        }

        /// <summary>
        /// Die
        /// </summary>
        /// <param name="where"></param>
        public override void Die(Vector2 where)
        {
            base.Die(where);
            this.MovementBehavior.Reset();
            this.AttackBehavior.Reset();
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

            if (AppSettingsFacade.IsDebugModeOn)
            {
                Vector2 target = this.MovementBehavior.GetTarget();
                spriteBatch.DrawString(SpriteFontRepository.Instance.Get("debug"), "Movement target: <" + target.X + ", " + target.Y + ">", this.Body.GetPosition() - Camera2D.Instance.GetPosition() + new Vector2(30f, 40f), Color.Yellow);
            }

            if (AppSettingsFacade.IsPathfindingDebugModeOn)
            {
                this.MovementBehavior.Draw(graphicsDevice, basicEffect, spriteBatch);
            }
        }
    }
}