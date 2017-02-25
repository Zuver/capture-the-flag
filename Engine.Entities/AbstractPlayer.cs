using Engine.Camera;
using Engine.Content;
using Engine.Drawing;
using Engine.GameStateManagement;
using Engine.Physics.Bodies;
using Engine.Physics.Bodies.Collisions;
using Engine.UI.Labels;
using Engine.UI.ProgressBars;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Engine.Entities
{
    public abstract class AbstractPlayer : AbstractEntity
    {
        #region Configurable attributes

        /// <summary>
        /// Respawn time in milliseconds
        /// </summary>
        private static int RespawnTimeInSeconds = 3;

        #endregion Configurable attributes

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
            return this.Team;
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
            return this.EnemyTeam;
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
            return this.Gun;
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
            return this.Flag != null;
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
            this._messageQueue.Enqueue(message);
        }

        /// <summary>
        /// Health bar
        /// </summary>
        private CircleProgressBar _healthBar;

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
            this.Team.CaptureFlag();
            this.Flag.ResetPosition();
            CollisionPool.Instance.AddBody(this.Flag.GetBody());
            ScreenController.Instance.ActiveScreen.EnqueueMessage(this.Team.GetName() + " captured " + this.Flag.GetTeam().GetName() + " flag");
            this.Flag = null;
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
            this.Health -= damage;

            if (this.Health <= 0)
            {
                this.Die(this.Body.GetPosition());
            }
        }

        /// <summary>
        /// Die
        /// </summary>
        /// <param name="where"></param>
        public virtual void Die(Vector2 where)
        {
            if (this.Gun != null)
            {
                this.DropGun();
            }

            if (this.HasFlag())
            {
                this.DropFlag();
            }

            this.Health = 100;
            this.IsAlive = false;
            this._timeOfDeath = DateTime.Now;
            this._deathLocation = this.Body.GetPosition();

            // Remove body from collision pool
            CollisionPool.Instance.RemoveBody(this.Body);

            this.Body.Reset();
            this.Body.SetPosition(this.Team.GetNextSpawnPoint());
        }

        /// <summary>
        /// Health label
        /// </summary>
        private TextLabel HealthLabel;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        /// <param name="team"></param>
        protected AbstractPlayer(AbstractBody body, PrimitiveBuilder model, AbstractTeam team, AbstractTeam enemyTeam)
            : base(body, model)
        {
            this.Team = team;
            this.EnemyTeam = enemyTeam;
            this.Health = AppSettingsFacade.PlayerHealth;

            this.HealthLabel = new TextLabel(String.Empty,
                SpriteFontRepository.Instance.Get("test"),
                Vector2.Zero,
                LabelAlignment.Center,
                Color.White);

            this._messageQueue = new TextLabelQueue(
                SpriteFontRepository.Instance.Get("test"),
                Vector2.Zero,
                LabelAlignment.Center,
                Color.White,
                3000);

            this._healthBar = new CircleProgressBar(new Color(20, 20, 30));

            EntityTable.Instance.Add(this.Body, this);

            this.IsAlive = true;
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            base.Update();

            // Update gun
            if (this.Gun != null)
            {
                // Update gun position to match this player's position
                this.Gun.GetBody().SetPosition(this.Body.GetPosition());
            }

            // Update flag
            if (this.Flag != null)
            {
                this.Flag.GetBody().SetPosition(this.Body.GetPosition());
            }

            // Update health label
            this.HealthLabel.SetText("Health: " + this.Health);
            this.HealthLabel.TopLeftPosition = this.Body.GetPosition() - this.HealthLabel.Origin -
                                               Camera2D.Instance.GetPosition() + new Vector2(0f, 30f);

            // Update health bar
            this._healthBar.SetProgress((float) this.Health/(float) AppSettingsFacade.PlayerHealth);
            this._healthBar.SetPosition(this.Body.GetPosition());

            // If the player is moving, then face the direction of movement
            if (this.Body.GetVelocity().LengthSquared() >= this.Body.GetMaxSpeed()*this.Body.GetMaxSpeed())
            {
                this.Look(this.Body.GetVelocity()*int.MaxValue);
            }

            this._messageQueue.Update();
            this._messageQueue.SetPosition(this.Body.GetPosition() - Camera2D.Instance.GetPosition() +
                                           new Vector2(0f, 50f));

            // Spawn logic
            if (!this.IsAlive && (DateTime.Now - this._timeOfDeath).TotalMilliseconds > (RespawnTimeInSeconds * 1000))
            {
                this.Spawn();
            }
        }

        /// <summary>
        /// Pick up gun
        /// </summary>
        /// <param name="gun"></param>
        public virtual void PickUpGun(AbstractGun gun)
        {
            if (this.Gun == null)
            {
                this.Gun = gun;
                this.Gun.SetPlayer(this);
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
            if (this.Flag == null)
            {
                this.Flag = flag;
                CollisionPool.Instance.RemoveBody(flag.GetBody());
                ScreenController.Instance.ActiveScreen.EnqueueMessage(flag.GetTeam().GetName() + " flag was taken");
            }
        }

        /// <summary>
        /// Spawn
        /// </summary>
        public virtual void Spawn()
        {
            this.IsAlive = true;
            CollisionPool.Instance.AddBody(this.Body);
            this.Body.SetPosition(this.Team.GetNextSpawnPoint());
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="graphicsDevice"></param>
        /// <param name="basicEffect"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GraphicsDevice graphicsDevice, BasicEffect basicEffect, SpriteBatch spriteBatch)
        {
            if (this.IsAlive)
            {
                base.Draw(graphicsDevice, basicEffect, spriteBatch);

                this.HealthLabel.Draw(spriteBatch);
                this._healthBar.Draw(graphicsDevice, basicEffect);
                this._messageQueue.Draw(spriteBatch);

                if (AppSettingsFacade.IsDebugModeOn)
                {
                    Vector2 bodyPosition = this.Body.GetPosition();
                    spriteBatch.DrawString(SpriteFontRepository.Instance.Get("debug"), "Body position: (" + bodyPosition.X + ", " + bodyPosition.Y + ")", bodyPosition - Camera2D.Instance.GetPosition() + new Vector2(30f, 0f), Color.Yellow);

                    Vector2 bodyVelocity = this.Body.GetVelocity();
                    spriteBatch.DrawString(SpriteFontRepository.Instance.Get("debug"), "Body velocity: (" + bodyVelocity.X + ", " + bodyVelocity.Y + ")", bodyPosition - Camera2D.Instance.GetPosition() + new Vector2(30f, 20f), Color.Yellow);
                }
            }
            else
            {
                spriteBatch.DrawString(SpriteFontRepository.Instance.Get("test"),
                    (RespawnTimeInSeconds - (int)((DateTime.Now - this._timeOfDeath).TotalSeconds)).ToString(),
                    this._deathLocation - Camera2D.Instance.GetPosition(),
                    this.Team.Color);
            }
        }

        /// <summary>
        /// Fire gun
        /// </summary>
        /// <param name="direction"></param>
        protected void FireGun(Vector2 direction)
        {
            if (this.Gun != null)
            {
                this.Gun.Fire(direction);
            }
        }

        /// <summary>
        /// Drop gun
        /// </summary>
        protected void DropGun()
        {
            Vector2 reverseDirection = -this.Body.GetVelocity();
            reverseDirection.Normalize();

            if (float.IsNaN(reverseDirection.X))
                reverseDirection.X = 0f;
            if (float.IsNaN(reverseDirection.Y))
                reverseDirection.Y = 0f;

            Vector2 direction = reverseDirection;

            this.Gun.GetBody().SetPosition(this.Body.GetPosition() + (((CircleBody)this.Body).GetRadius() + ((CircleBody)this.Gun.GetBody()).GetRadius() + 1f) * direction);

            // Add gun body back to collision pool
            CollisionPool.Instance.AddBody(this.Gun.GetBody());

            this.Gun.SetPlayer(null);
            this.Gun = null;
        }

        /// <summary>
        /// Drop flag
        /// </summary>
        private void DropFlag()
        {
            CollisionPool.Instance.AddBody(this.Flag.GetBody());
            this.Flag = null;
        }
    }
}