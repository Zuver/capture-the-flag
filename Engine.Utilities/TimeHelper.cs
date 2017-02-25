using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Utilities
{
    public static class TimeHelper
    {
        private static double _currentGameTime;

        public static double GetCurrentGameTime()
        {
            return _currentGameTime;
        }

        public static void SetCurrentGameTime(double currentGameTime)
        {
            _currentGameTime = currentGameTime;
        }
    }
}