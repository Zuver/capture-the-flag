using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Entities.EventHandlers
{
    /// <summary>
    /// Event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void FiredWeaponEventHandler(AbstractPlayer sender, FiredWeaponEventArgs e);

    /// <summary>
    /// Fired weapon event arguments
    /// </summary>
    public class FiredWeaponEventArgs : EventArgs
    {
        public Vector2 Direction { get; private set; }

        public FiredWeaponEventArgs(Vector2 direction)
        {
            this.Direction = direction;
        }
    }
}