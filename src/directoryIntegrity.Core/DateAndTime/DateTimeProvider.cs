using System;

namespace directoryIntegrity.Core.DateAndTime
{
    public static class DateTimeProvider
    {
        private static Func<DateTime> _now = () => DateTime.Now;
        private static DateTime _frozenTime;

        public static DateTime Now => _frozen ? _frozenTime : _now();

        private static bool _frozen = false;
        public static DateTime Freeze()
        {
            _frozenTime = _now();
            _frozen = true;
            return _frozenTime;
        }

        public static void Thaw()
        {
            _frozen = false;
        }

        public static void Set(DateTime dt)
        {
            _now = () => dt;
        }

        public static void MoveBackward(TimeSpan ts)
        {
            var dt = _now().Subtract(ts);
            Set(dt);
        }

        public static void MoveForward(TimeSpan ts)
        {
            var dt = _now().Add(ts);
            Set(dt);
        }

        public static void Reset()
        {
            _now = () => DateTime.Now;
        }
    }
}