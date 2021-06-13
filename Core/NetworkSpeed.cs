using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetworkMon.Core
{
    public enum DisplayUnit
    {
        KiloBit, KiloByte, MegeBit, MegaByte
    }

    public class NetworkSpeed
    {
        private long prevValue;

        public double CurrentTraffic(long received)
        {
            long recievedBytes = received - prevValue;
            // Debug.WriteLine($"{DateTime.Now.ToString("hh:mm:ss:fff")} :R: {recievedBytes} Bytes");
            if (recievedBytes > 0)
            {
                // Debug.WriteLine($"{recievedBytes}={(recievedBytes / 1024f).ToString("#.##")}");
            }

            prevValue = received;
            // return recievedBytes / 125000f;
            return recievedBytes;
        }

        string ConvertToUnit(double traffic, DisplayUnit displayUnit)
        {
            string convertedText = "";

            switch (displayUnit)
            {
                case DisplayUnit.KiloBit:
                    convertedText = (traffic / 125).ToString("#.#");

                    break;
                case DisplayUnit.KiloByte:
                    convertedText = (traffic / 1000).ToString("#.#");

                    break;
                case DisplayUnit.MegeBit:
                    convertedText = (traffic / 125000).ToString("#.#");

                    break;
                case DisplayUnit.MegaByte:
                    convertedText = (traffic / 1000000).ToString("#.#");

                    break;
            }

            return convertedText;
        }

        public List<string> GetSpeed(long received, int timeDelayInSeconds)
        {
            double realtimeTraffic = CurrentTraffic(received);
            Debug.WriteLine($"R:{realtimeTraffic} Bytes");

            var kbit = $"{ConvertToUnit(realtimeTraffic, DisplayUnit.KiloBit)} Kb/s";
            var kbyte = $"{ConvertToUnit(realtimeTraffic, DisplayUnit.KiloByte)} KB/s";
            var mbit = $"{ConvertToUnit(realtimeTraffic, DisplayUnit.MegeBit)} Mb/s";
            var mbyte = $"{ConvertToUnit(realtimeTraffic, DisplayUnit.MegaByte)} MB/s";

            Debug.WriteLine(kbit);
            Debug.WriteLine(kbyte);
            Debug.WriteLine(mbit);
            Debug.WriteLine(mbyte);

            Debug.WriteLine("-----------------------------------");

            string quicktext = String.Empty;

            //Debug.WriteLine(_speed);

            //if (_speed == 0)
            //{
            //    quicktext = $"";
            //}
            //else if (_speed > 1)
            //{
            //    quicktext = $"{_speed.ToString("#")} Mb/s ({(_speed * 125).ToString("#.#")} KB/s)";
            //}

            //if (_speed < 1)
            //{
            //    if ((_speed * 1000) > 1)
            //    {
            //        quicktext = $"{(_speed * 1000).ToString("#.#")} Kbps";
            //    }
            //}

            return new List<string> { kbit, kbyte, mbit, mbyte };

            // return quicktext;
        }
    }
}
