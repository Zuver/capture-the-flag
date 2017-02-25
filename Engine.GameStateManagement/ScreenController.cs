using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.GameStateManagement
{
    public class ScreenController
    {
        /// <summary>
        /// Public data
        /// </summary>
        public AbstractScreen ActiveScreen { get; private set; }
        public event EventHandler Closed;

        /// <summary>
        /// Private data
        /// </summary>
        private Stack<AbstractScreen> ScreenStack;

        /// <summary>
        /// This is a singleton class
        /// </summary>
        private static ScreenController instance;
        public static ScreenController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ScreenController();
                }

                return instance;
            }
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private ScreenController()
        {
            this.ScreenStack = new Stack<AbstractScreen>();
            this.ActiveScreen = null;
        }

        /// <summary>
        /// Initialize using a top-level screen
        /// </summary>
        /// <param name="screen"></param>
        public void Initialize(AbstractScreen screen)
        {
            this.ScreenStack = new Stack<AbstractScreen>();
            this.ScreenStack.Push(screen);
            this.ActiveScreen = screen;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <returns>True if an application exit has been requested</returns>
        public bool Update(KeyboardState keyboardState, MouseState mouseState)
        {
            bool exitRequested = false;

            if (this.ActiveScreen != null)
            {
                this.ActiveScreen.Update();
            }
            else
            {
                exitRequested = true;
            }

            return exitRequested;
        }

        /// <summary>
        /// Navigate to a different screen
        /// </summary>
        /// <param name="screen">Screen</param>
        public void NavigateTo(AbstractScreen screen)
        {
            AbstractScreen activeScreen = this.ScreenStack.Peek();

            if (activeScreen != screen)
            {
                this.ScreenStack.Push(screen);
                this.ActiveScreen = screen;
            }
        }

        /// <summary>
        /// Navigate back
        /// </summary>
        public void NavigateBack()
        {
            if (this.ScreenStack.Count > 1)
            {
                this.ScreenStack.Pop();
                this.ActiveScreen = this.ScreenStack.Peek();
            }
        }

        /// <summary>
        /// Closes all screens and exits the application
        /// </summary>
        public void Close()
        {
            OnClose(EventArgs.Empty);
        }

        public void OnClose(EventArgs e)
        {
            this.Closed(this, e);
        }
    }
}