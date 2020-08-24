using System;
using Engine.Camera;
using Engine.Content.Fonts;
using Engine.Drawing;
using Engine.GameStateManagement;
using Engine.Physics.Bodies;
using Engine.Physics.Bodies.Collisions;
using Engine.UI.Labels;
using Engine.UI.ProgressBars;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Entities
{
    public abstract class AbstractPlayer : AbstractEntity
    {
        /// <summary>
        /// Team
        /// </summary>
        protected AbstractTeam Team;

        /// <summary>
        /// Get team
        /// </summary>
        /// <returns></returns>
        public AbstractTeam GetTeam()
        {
            return Team;
        }

        /// <summary>
        /// Enemy team
        /// </summary>
        protected AbstractTeam EnemyTeam;

        /// <summary>
        /// Get enemy team
        /// </summary>
        /// <returns></returns>
        public AbstractTeam GetEnemyTeam()
        {
            return EnemyTeam;
        }

        /// <summary>
        /// Gun
        /// </summary>
        protected AbstractGun Gun;

        /// <summary>
        /// Get gun
        /// </summary>
        /// <returns></returns>
        public AbstractGun GetGun()
        {
            return Gun;
        }

        /// <summary>
        /// Flag
        /// </summary>
        protected AbstractFlag Flag;

        /// <summary>
        /// Has flag
        /// </summary>
        /// <returns></returns>
        public bool HasFlag()
        {
            return Flag != null;
        }

        /// <summary>
        /// Message queue
        /// </summary>
        private readonly TextLabelQueue _messageQueue;

        /// <summary>
        /// Enqueue message
        /// </summary>
        /// <param name="message"></param>
        protected void EnqueueMessage(string message)
        {
            _messageQueue.Enqueue(message);
        }

        /// <summary>
        /// Health bar
        /// </summary>
        private readonly CircleProgressBar _healthBar;

        /// <summary>
        /// Is alive?
        /// </summary>
        public bool IsAlive;

        /// <summary>
        /// Time of death
        /// </summary>
        private DateTime _timeOfDeath;

        /// <summary>
        /// Death location
        /// </summary>
        private Vector2 _deathLocation;

        /// <summary>
        /// Capture flag
        /// </summary>
        public virtual void CaptureFlag()
        {
            Team.CaptureFlag();
            Flag.ResetPosition();
            CollisionPool.Instance.AddBody(Flag.GetBody());
            ScreenController.Instance.ActiveScreen.EnqueueMessage(Team.GetName() + " captured " +
                                                                  Flag.GetTeam().GetName() + " flag");
            Flag = null;
        }

        /// <summary>
        /// Health
        /// </summary>
        protected int Health;

        /// <summary>
        /// Decrease health
        /// </summary>
        /// <param name="damage"></param>
        public void DecreaseHealth(int damage)
        {
            Health -= damage;

            if (Health <= 0)
            {
                Die(Body.GetPosition());
            }

            // Update health bar
            _healthBar.SetProgress(Health / (float)AppSettingsFacade.PlayerHealth);
        }

        /// <summary>
        /// Die
        /// </summary>
        /// <param name="where"></param>
        public virtual void Die(Vector2 where)
        {
            if (Gun != null)
            {
                DropGun();
            }

            if (HasFlag())
            {
                DropFlag();
            }

            Health = AppSettingsFacade.PlayerHealth;
            IsAlive = false;
            _timeOfDeath = DateTime.Now;
            _deathLocation = Body.GetPosition();

            // Remove body from collision pool so nothing else can interact with it
            CollisionPool.Instance.RemoveBody(Body);

            Body.Freeze();
            Body.SetPosition(Team.GetNextSpawnPoint());
        }

        /// <summary>
        /// Health label
        /// </summary>
        private readonly TextLabel _healthLabel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        /// <param name="team"></param>
        /// <param name="enemyTeam"></param>
        protected AbstractPlayer(AbstractBody body, PrimitiveBuilder model, AbstractTeam team, AbstractTeam enemyTeam)
            : base(body, model)
        {
            Team = team;
            EnemyTeam = enemyTeam;
            Health = AppSettingsFacade.PlayerHealth;

            _healthLabel = new TextLabel(string.Empty,
                SpriteFontRepository.Instance.Get("test"),
                Vector2.Zero,
                LabelAlignment.Center,
                Color.White);

            _messageQueue = new TextLabelQueue(
                SpriteFontRepository.Instance.Get("test"),
                Vector2.Zero,
                LabelAlignment.Center,
                Color.White,
                3000);

            _healthBar = new CircleProgressBar(new Color(255, 255, 255));

            EntityTable.Instance.Add(Body, this);

            IsAlive = true;
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Update gun position to match this player's position
            Gun?.GetBody().SetPosition(Body.GetPosition());

            // Update flag
            Flag?.GetBody().SetPosition(Body.GetPosition());

            // Update health label
            _healthLabel.SetText("Health: " + Health);
            _healthLabel.TopLeftPosition = Body.GetPosition() - _healthLabel.Origin -
                                           Camera2D.Instance.GetPosition() + new Vector2(0f, 30f);

            _healthBar.SetPosition(Body.GetPosition());

            // If the player is moving, then face the direction of movement
            if (Body.GetVelocity().LengthSquared() >= Body.GetMaxSpeed() * Body.GetMaxSpeed())
            {
                Look(Body.GetVelocity() * int.MaxValue);
            }

            _messageQueue.Update();
            _messageQueue.SetPosition(Body.GetPosition() - Camera2D.Instance.GetPosition() +
                                      new Vector2(0f, 50f));

            if (!IsAlive &&
                (DateTime.Now - _timeOfDeath).TotalMilliseconds > (AppSettingsFacade.PlayerRespawnTimeInSeconds * 1000))
            {
                // Respawn if it has been long enough
                Respawn();
            }
        }

        /// <summary>
        /// Pick up gun
        /// </summary>
        /// <param name="gun"></param>
        public virtual void PickUpGun(AbstractGun gun)
        {
            if (Gun == null)
            {
                Gun = gun;
                Gun.SetPlayer(this);
                CollisionPool.Instance.RemoveBody(gun.GetBody());
            }
        }

        /// <summary>
        /// Return flag
        /// </summary>
        /// <param name="flag"></param>
        public virtual void ReturnFlag(AbstractFlag flag)
        {
            flag.ResetPosition();
            ScreenController.Instance.ActiveScreen.EnqueueMessage(flag.GetTeam().GetName() + " flag was returned");
        }

        /// <summary>
        /// Pick up flag
        /// </summary>
        /// <param name="flag"></param>
        public virtual void PickUpFlag(AbstractFlag flag)
        {
            if (Flag == null)
            {
                Flag = flag;
                CollisionPool.Instance.RemoveBody(flag.GetBody());
                ScreenController.Instance.ActiveScreen.EnqueueMessage(flag.GetTeam().GetName() + " flag was taken");
            }
        }

        /// <summary>
        /// Initial spawn
        /// </summary>
        public virtual void InitialSpawn()
        {
            IsAlive = true;
            CollisionPool.Instance.AddBody(Body);
            Body.SetPosition(Team.GetNextSpawnPoint());
        }

        /// <summary>
        /// Respawn
        /// </summary>
        public virtual void Respawn()
        {
            IsAlive = true;
            CollisionPool.Instance.AddBody(Body);
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, SpriteBatch spriteBatch)
        {
            if (IsAlive)
            {
                base.Draw(graphicsDevice, basicEffect, spriteBatch);

                _healthBar.Draw(graphicsDevice, basicEffect);
                _messageQueue.Draw(spriteBatch);

                if (AppSettingsFacade.IsDebugModeOn)
                {
                    Vector2 bodyPosition = Body.GetPosition();
                    spriteBatch.DrawString(SpriteFontRepository.Instance.Get("debug"),
                        "Body position: (" + bodyPosition.X + ", " + bodyPosition.Y + ")",
                        bodyPosition - Camera2D.Instance.GetPosition() + new Vector2(30f, 0f), Color.Yellow);

                    Vector2 bodyVelocity = Body.GetVelocity();
                    spriteBatch.DrawString(SpriteFontRepository.Instance.Get("debug"),
                        "Body velocity: (" + bodyVelocity.X + ", " + bodyVelocity.Y + ")",
                        bodyPosition - Camera2D.Instance.GetPosition() + new Vector2(30f, 20f), Color.Yellow);
                }
            }
            else
            {
                spriteBatch.DrawString(SpriteFontRepository.Instance.Get("test"),
                    (AppSettingsFacade.PlayerRespawnTimeInSeconds - (int)((DateTime.Now - _timeOfDeath).TotalSeconds))
                    .ToString(),
                    _deathLocation - Camera2D.Instance.GetPosition(),
                    Team.Color);
            }
        }

        /// <summary>
        /// Fire gun
        /// </summary>
        /// <param name="direction"></param>
        protected void FireGun(Vector2 direction)
        {
            Gun?.Fire(direction);
        }

        /// <summary>
        /// Drop gun
        /// </summary>
        protected void DropGun()
        {
            Vector2 reverseDirection = -Body.GetVelocity();
            reverseDirection.Normalize();

            if (float.IsNaN(reverseDirection.X))
                reverseDirection.X = 0f;
            if (float.IsNaN(reverseDirection.Y))
                reverseDirection.Y = 0f;

            Vector2 direction = reverseDirection;

            Gun.GetBody()
                .SetPosition(Body.GetPosition() +
                             (((CircleBody)Body).GetRadius() + ((CircleBody)Gun.GetBody()).GetRadius() + 1f) *
                             direction);

            // Add gun body back to collision pool
            CollisionPool.Instance.AddBody(Gun.GetBody());

            Gun.SetPlayer(null);
            Gun = null;
        }

        /// <summary>
        /// Drop flag
        /// </summary>
        private void DropFlag()
        {
            CollisionPool.Instance.AddBody(Flag.GetBody());
            Flag = null;
        }
    }
}
