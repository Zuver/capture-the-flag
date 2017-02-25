using Engine.Camera;
using Engine.Drawing;
using Engine.Entities;
using Engine.Physics.Bodies;
using Engine.Utilities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace CaptureTheFlag.Entities.Players
{
    public class UserPlayer : AbstractPlayer
    {
        protected KeyboardState PreviousKeyboardState;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="body"></param>
        /// <param name="model"></param>
        /// <param name="team"></param>
        /// <param name="enemyTeam"></param>
        public UserPlayer(AbstractBody body, PrimitiveBuilder model, AbstractTeam team, AbstractTeam enemyTeam)
            : base(body, model, team, enemyTeam)
        {
            this.SetCollisionBehaviors(new CollisionBehaviors(this));
        }

        /// <summary>
        /// Creates the "default" instance of this class
        /// </summary>
        /// <param name="color"></param>
        /// <param name="team"></param>
        /// <param name="enemyTeam"></param>
        /// <returns></returns>
        public static UserPlayer Default(Color color, AbstractTeam team, AbstractTeam enemyTeam)
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

            UserPlayer result = new UserPlayer(body, model, team, enemyTeam);
            team.AddPlayer(result);

            return result;
        }

        /// <summary>
        /// Update
        /// </summary>
        public override void Update()
        {
            base.Update();

            Vector2 force = Vector2.Zero;
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();

            // Check if player is sprinting
            if (keyboardState.IsKeyDown(Keys.Space))
            {
                this.Body.SetMaxSpeed(AppSettingsFacade.PlayerSprintSpeed);
            }
            else
            {
                this.Body.SetMaxSpeed(AppSettingsFacade.PlayerMaxSpeed);
            }

            // Check for player movement
            if (keyboardState.IsKeyDown(Keys.W))
            {
                force.Y += -1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.A))
            {
                force.X += -1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.S))
            {
                force.Y += 1.0f;
            }
            if (keyboardState.IsKeyDown(Keys.D))
            {
                force.X += 1.0f;
            }

            force.Normalize();

            if (float.IsNaN(force.X))
            {
                force.X = 0.0f;
            }
            if (float.IsNaN(force.Y))
            {
                force.Y = 0.0f;
            }

            this.Body.ApplyForce(force);

            Camera2D.Instance.SetTargetPosition(this.Body.GetPosition() - new Vector2(AppSettingsFacade.WindowWidth / 2, AppSettingsFacade.WindowHeight / 2));

            if (this.Gun != null && keyboardState.IsKeyDown(Keys.F) && !this.PreviousKeyboardState.IsKeyDown(Keys.F))
            {
                this.DropGun();
            }

            this.ProcessMouseState(mouseState);

            this.PreviousKeyboardState = keyboardState;
        }

        /// <summary>
        /// Pick up gun
        /// </summary>
        /// <param name="gun"></param>
        public override void PickUpGun(AbstractGun gun)
        {
            if (this.Gun == null)
            {
                this.EnqueueMessage("You picked up a " + gun.GetType().Name);
            }

            base.PickUpGun(gun);
        }

        /// <summary>
        /// Return flag
        /// </summary>
        /// <param name="flag"></param>
        public override void ReturnFlag(AbstractFlag flag)
        {
            base.ReturnFlag(flag);
            this.EnqueueMessage("You returned the flag");
        }

        /// <summary>
        /// Pick up flag
        /// </summary>
        /// <param name="flag"></param>
        public override void PickUpFlag(AbstractFlag flag)
        {
            base.PickUpFlag(flag);
            this.EnqueueMessage("You picked up the flag");
        }

        /// <summary>
        /// Capture the flag
        /// </summary>
        public override void CaptureFlag()
        {
            base.CaptureFlag();
            this.EnqueueMessage("You captured the flag");
        }

        /// <summary>
        /// Spawn
        /// </summary>
        public override void Spawn()
        {
            base.Spawn();
            Camera2D.Instance.Freeze();
        }

        /// <summary>
        /// Process mouse input state
        /// </summary>
        /// <param name="mouseState"></param>
        protected virtual void ProcessMouseState(MouseState mouseState)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 bodyPosition = this.Body.GetPosition() - Camera2D.Instance.GetPosition();
                Vector2 toMousePosition = new Vector2(mouseState.X - bodyPosition.X, mouseState.Y - bodyPosition.Y);
                this.FireGun(toMousePosition);
                this.Look(new Vector2(mouseState.X, mouseState.Y) - this.Body.GetPosition() + Camera2D.Instance.GetPosition());
            }
        }
    }
}