using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Utilities
{
    public static class AppSettingsFacade
    {
        /// <summary>
        /// App settings collection
        /// </summary>
        private static NameValueCollection appSettings;

        /// <summary>
        /// Reloads all app settings
        /// </summary>
        public static void Refresh()
        {
            appSettings = ConfigurationManager.AppSettings;
            _isDebugModeOn = bool.Parse(appSettings["IsDebugModeOn"]);
        }

        /// <summary>
        /// Window width
        /// </summary>
        public static int WindowWidth
        {
            get
            {
                return int.Parse(appSettings["WindowWidth"]);
            }
        }

        /// <summary>
        /// Window height
        /// </summary>
        public static int WindowHeight
        {
            get
            {
                return int.Parse(appSettings["WindowHeight"]);
            }
        }

        /// <summary>
        /// Is full screen?
        /// </summary>
        public static bool IsFullScreen
        {
            get
            {
                return bool.Parse(appSettings["IsFullScreen"]);
            }
        }

        /// <summary>
        /// Turn v-sync on?
        /// </summary>
        public static bool SynchronizeWithVerticalRetrace
        {
            get
            {
                return bool.Parse(appSettings["SynchronizeWithVerticalRetrace"]);
            }
        }

        /// <summary>
        /// Is debug mode on?
        /// </summary>
        private static bool _isDebugModeOn;
        public static bool IsDebugModeOn
        {
            get
            {
                return _isDebugModeOn;
            }
            set
            {
                _isDebugModeOn = value;
            }
        }

        /// <summary>
        /// Is pathfinding debug mode on?
        /// </summary>
        private static bool _isPathfindingDebugModeOn;
        public static bool IsPathfindingDebugModeOn
        {
            get
            {
                return _isPathfindingDebugModeOn;
            }
            set
            {
                _isPathfindingDebugModeOn = value;
            }
        }

        /// <summary>
        /// Player radius
        /// </summary>
        public static float PlayerRadius
        {
            get
            {
                return float.Parse(appSettings["PlayerRadius"]);
            }
        }

        /// <summary>
        /// Player mass
        /// </summary>
        public static float PlayerMass
        {
            get
            {
                return float.Parse(appSettings["PlayerMass"]);
            }
        }

        /// <summary>
        /// Player max speed
        /// </summary>
        public static float PlayerMaxSpeed
        {
            get
            {
                return float.Parse(appSettings["PlayerMaxSpeed"]);
            }
        }

        /// <summary>
        /// Player sprint speed
        /// </summary>
        public static float PlayerSprintSpeed
        {
            get
            {
                return float.Parse(appSettings["PlayerSprintSpeed"]);
            }
        }

        /// <summary>
        /// Player friction coefficient
        /// </summary>
        public static float PlayerFrictionCoefficient
        {
            get
            {
                return float.Parse(appSettings["PlayerFrictionCoefficient"]);
            }
        }

        /// <summary>
        /// Player health
        /// </summary>
        public static int PlayerHealth
        {
            get
            {
                return int.Parse(appSettings["PlayerHealth"]);
            }
        }
    }
}