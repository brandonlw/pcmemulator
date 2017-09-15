using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PCMEmulator
{
    internal struct AttoTime
    {
        public const int TIME_MAX_SECONDS = 1000000000;
        public const long SECONDS_PER_SECOND = (long)(1e18);
        public static readonly AttoTime TIME_ZERO = new AttoTime(0, 0);
        public static readonly AttoTime TIME_NEVER = new AttoTime(1000000000, 0);

        public int Seconds;

        public long AttoSeconds;

        public AttoTime(int i, long l)
        {
            Seconds = i;
            AttoSeconds = l;
        }

        public static AttoTime operator +(AttoTime first, AttoTime second)
        {
            var result = new AttoTime();

            /* if one of the items is Attotime_never, return Attotime_never */
            if (first.Seconds >= TIME_MAX_SECONDS || second.Seconds >= TIME_MAX_SECONDS)
            {
                return AttoTime.TIME_NEVER;
            }

            /* add the seconds and Attoseconds */
            result.AttoSeconds = first.AttoSeconds + second.AttoSeconds;
            result.Seconds = first.Seconds + second.Seconds;

            /* normalize and return */
            if (result.AttoSeconds >= AttoTime.SECONDS_PER_SECOND)
            {
                result.AttoSeconds -= AttoTime.SECONDS_PER_SECOND;
                result.Seconds++;
            }

            /* overflow */
            if (result.Seconds >= AttoTime.TIME_MAX_SECONDS)
            {
                return AttoTime.TIME_NEVER;
            }

            return result;
        }

        public static AttoTime operator -(AttoTime first, AttoTime second)
        {
            AttoTime result;

            /* if time1 is Attotime_never, return Attotime_never */
            if (first.Seconds >= AttoTime.TIME_MAX_SECONDS)
            {
                return AttoTime.TIME_NEVER;
            }

            /* add the seconds and Attoseconds */
            result.AttoSeconds = first.AttoSeconds - second.AttoSeconds;
            result.Seconds = first.Seconds - second.Seconds;

            /* normalize and return */
            if (result.AttoSeconds < 0)
            {
                result.AttoSeconds += AttoTime.SECONDS_PER_SECOND;
                result.Seconds--;
            }

            return result;
        }

        public static bool operator >(AttoTime first, AttoTime second)
        {
            var ret = false;

            if (first.Seconds > second.Seconds)
            {
                ret = true;
            }
            else if (first.AttoSeconds > second.AttoSeconds)
            {
                ret = true;
            }

            return ret;
        }

        public static bool operator >=(AttoTime first, AttoTime second)
        {
            return (first > second) || (first == second);
        }

        public static bool operator <(AttoTime first, AttoTime second)
        {
            var ret = false;

            if (first.Seconds < second.Seconds)
            {
                ret = true;
            }
            else if (first.AttoSeconds < second.AttoSeconds)
            {
                ret = true;
            }

            return ret;
        }

        public static bool operator <=(AttoTime first, AttoTime second)
        {
            return (first < second) || (first == second);
        }

        public static bool operator ==(AttoTime first, AttoTime second)
        {
            var ret = false;

            if (first.Seconds == second.Seconds)
            {
                ret = true;
            }
            else if (first.AttoSeconds == second.AttoSeconds)
            {
                ret = true;
            }

            return ret;
        }

        public static bool operator !=(AttoTime first, AttoTime second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
