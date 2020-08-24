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
            SetCollisionBehaviors(new CollisionBehaviors(this));
        }

        /// <summary>
        /// Creates the "default" instance of this class
        /// </summary>
        /// <param name="team"></param>
        /// <param name="enemyTeam"></param>
        /// <returns></returns>
        public static UserPlayer Default(AbstractTeam team, AbstractTeam enemyTeam)
        {
            CircleBody body = BodyFactory.Circle(false,
                AppSettingsFacade.PlayerMass,
                AppSettingsFacade.PlayerMaxSpeed,
                AppSettingsFacade.PlayerFrictionCoefficient,
                true,
                AppSettingsFacade.PlayerRadius);

            PrimitiveBuilder model = new PrimitiveBuilder(team.Color);
            model.Add(PrimitiveFactory.Circle(team.Color, AppSettingsFacade.PlayerRadius, 3));
            model.Add(PrimitiveFactory.Line(team.Color, Vector2.Zero, Vector2.UnitX * body.GetRadius()));

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

            if (IsAlive)
            {
                Vector2 force = Vector2.Zero;
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
                KeyboardState keyboardState = Keyboard.GetState();
                MouseState mouseState = Mouse.GetState();

                // Check if player is sprinting
                if (keyboardState.IsKeyDown(Keys.Space) || gamePadState.IsButtonDown(Buttons.LeftStick))
                {
                    Body.SetMaxSpeed(AppSettingsFacade.PlayerSprintSpeed);
                }
                else
                {
                    Body.SetMaxSpeed(AppSettingsFacade.PlayerMaxSpeed);
                }

                force.X += gamePadState.ThumbSticks.Left.X;
                force.Y -= gamePadState.ThumbSticks.Left.Y;

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

                Body.ApplyForce(force);

                Camera2D.Instance.SetTargetPosition(Body.GetPosition() -
                                                    new Vector2((float)AppSettingsFacade.WindowWidth / 2,
                                                        (float)AppSettingsFacade.WindowHeight / 2));

                if (Gun != null && keyboardState.IsKeyDown(Keys.F) && !PreviousKeyboardState.IsKeyDown(Keys.F))
                {
                    DropGun();
                }

                if (gamePadState.ThumbSticks.Right.Length() > 0.25f)
                {
                    Vector2 faceDirection = new Vector2(gamePadState.ThumbSticks.Right.X,
                        -gamePadState.ThumbSticks.Right.Y);

                    Look(faceDirection);
                    FireGun(faceDirection);
                }

                ProcessMouseState(mouseState);

                PreviousKeyboardState = keyboardState;
            }
        }

        /// <summary>
        /// Pick up gun
        /// </summary>
        /// <param name="gun"></param>
        public override void PickUpGun(AbstractGun gun)
        {
            if (Gun == null)
            {
                EnqueueMessage("You picked up a " + gun.GetType().Name);
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
            EnqueueMessage("You returned the flag");
        }

        /// <summary>
        /// Pick up flag
        /// </summary>
        /// <param name="flag"></param>
        public override void PickUpFlag(AbstractFlag flag)
        {
            base.PickUpFlag(flag);
            EnqueueMessage("You picked up the flag");
        }

        /// <summary>
        /// Capture the flag
        /// </summary>
        public override void CaptureFlag()
        {
            base.CaptureFlag();
            EnqueueMessage("You captured the flag");
        }

        /// <summary>
        /// Respawn
        /// </summary>
        public override void Respawn()
        {
            base.Respawn();
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
                Vector2 bodyPosition = Body.GetPosition() - Camera2D.Instance.GetPosition();
                Vector2 toMousePosition = new Vector2(mouseState.X - bodyPosition.X, mouseState.Y - bodyPosition.Y);
                FireGun(toMousePosition);
                Look(new Vector2(mouseState.X, mouseState.Y) - Body.GetPosition() + Camera2D.Instance.GetPosition());
            }
        }
    }
}
