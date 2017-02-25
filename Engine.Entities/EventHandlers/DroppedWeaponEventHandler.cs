using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Entities.EventHandlers
{
    /// <summary>
    /// Event handler
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void DroppedWeaponEventHandler(AbstractPlayer sender, DroppedWeaponEventArgs e);

    /// <summary>
    /// Dropped weapon event arguments
    /// </summary>
    public class DroppedWeaponEventArgs : EventArgs
    {
        public Vector2 DropPosition { get; private set; }

        public DroppedWeaponEventArgs(Vector2 dropPosition)
        {
            this.DropPosition = dropPosition;
        }
    }
}