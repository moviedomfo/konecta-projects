using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ScrollUpDemo_w32
{
    internal class Helper
    {
        /// <summary>
        /// Mueve el scroll vertical hasta el principio.
        /// </summary>
        /// <param name="pTable"></param>
        public static void TableLayoutPanel_ScrollToTop(TableLayoutPanel pTable)
        {
            int wSize = pTable.Controls.Count;
            Control wCtrl = null;

            if (wSize == 0)
                return;

            wCtrl = pTable.Controls[wSize - 1];
            pTable.ScrollControlIntoView(wCtrl);
        }
        public static void TableLayoutPanel_ScrollToBotton(TableLayoutPanel pTable)
        {
            int wSize = pTable.Controls.Count;
            Control wCtrl = null;

            if (wSize == 0)
                return;

            wCtrl = pTable.Controls[0];
            pTable.ScrollControlIntoView(wCtrl);

      
        }

        public static string TimeElapsedToString(Int64 pTimeInSeconds)
        {
            TimeSpan wTs = SecondToTimeSpan((int)pTimeInSeconds);
            string wResult = string.Empty;

            wResult = string.Concat(wTs.Seconds.ToString(), " segundo");
            if (wTs.Seconds > 1)
                wResult += "s";
            if (wTs.Minutes == 0 && wTs.Hours == 0 && wTs.Days == 0)
                return wResult;

            wResult = string.Empty;
            if (wTs.Minutes > 0)
                wResult = (wTs.Minutes == 1) ? "1 minuto " : string.Concat(wTs.Minutes.ToString(), " minutos ");
            if (wTs.Hours > 0)
                wResult = (wTs.Hours == 1) ? string.Concat("1 hora ", wResult) : string.Concat(wTs.Hours, " horas ", wResult);
            if (wTs.Days > 0)
                wResult = (wTs.Days == 1) ? string.Concat("1 día ", wResult) : string.Concat(wTs.Days, " días ", wResult);

            return wResult;
        }
        public static TimeSpan SecondToTimeSpan(int pTimeInSeconds)
        {
            int wHour = 0, wMin = 0, wSec = 0;

            wMin = Math.DivRem(pTimeInSeconds, 60, out wSec);
            if (wMin >= 60)
            {
                wHour = Math.DivRem(wMin, 60, out wMin);

                if (wMin > 0)
                    wMin = (wMin == 1) ? 1 : wMin;
                wHour = (wHour == 1) ? 1 : wHour;
            }
            else
                wMin = (wMin == 1) ? 1 : wMin;

            return new TimeSpan(wHour, wMin, wSec);
        }
    }
}
