using System.Collections.Specialized;
using System.Configuration;

namespace Engine.Utilities
{
    public static class AppSettingsFacade
    {
        /// <summary>
        /// App settings collection
        /// </summary>
        private static NameValueCollection _appSettings;

        /// <summary>
        /// Reloads all app settings
        /// </summary>
        public static void Refresh()
        {
            _appSettings = ConfigurationManager.AppSettings;
            IsDebugModeOn = bool.Parse(_appSettings["IsDebugModeOn"]);
        }

        /// <summary>
        /// Bot reaction time in milliseconds
        /// </summary>
        public static int BotReactionTimeInMilliseconds => int.Parse(_appSettings["BotReactionTimeInMilliseconds"]);

        /// <summary>
        /// Window width
        /// </summary>
        public static int WindowWidth => int.Parse(_appSettings["WindowWidth"]);

        /// <summary>
        /// Window height
        /// </summary>
        public static int WindowHeight => int.Parse(_appSettings["WindowHeight"]);

        /// <summary>
        /// Is full screen?
        /// </summary>
        public static bool IsFullScreen => bool.Parse(_appSettings["IsFullScreen"]);

        /// <summary>
        /// Turn v-sync on?
        /// </summary>
        public static bool SynchronizeWithVerticalRetrace => bool.Parse(_appSettings["SynchronizeWithVerticalRetrace"]);

        public static bool IsDebugModeOn { get; set; }

        public static bool IsPathfindingDebugModeOn { get; set; }

        public static bool IsPaused { get; set; } = false;

        /// <summary>
        /// Player radius
        /// </summary>
        public static float PlayerRadius => float.Parse(_appSettings["PlayerRadius"]);

        /// <summary>
        /// Player mass
        /// </summary>
        public static float PlayerMass => float.Parse(_appSettings["PlayerMass"]);

        /// <summary>
        /// Player max speed
        /// </summary>
        public static float PlayerMaxSpeed => float.Parse(_appSettings["PlayerMaxSpeed"]);

        /// <summary>
        /// Player sprint speed
        /// </summary>
        public static float PlayerSprintSpeed => float.Parse(_appSettings["PlayerSprintSpeed"]);

        /// <summary>
        /// Player friction coefficient
        /// </summary>
        public static float PlayerFrictionCoefficient => float.Parse(_appSettings["PlayerFrictionCoefficient"]);

        /// <summary>
        /// Player health
        /// </summary>
        public static int PlayerHealth => int.Parse(_appSettings["PlayerHealth"]);

        /// <summary>
        /// Player respawn time in seconds
        /// </summary>
        public static int PlayerRespawnTimeInSeconds => int.Parse(_appSettings["PlayerRespawnTimeInSeconds"]);

        /// <summary>
        /// Minimum distance between a point on a wall and a node in the navigation mesh
        /// </summary>
        public static int WallBufferInPixels => int.Parse(_appSettings["WallBufferInPixels"]);
    }
}
