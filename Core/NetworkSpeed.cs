using System.Diagnostics;

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

        private double ConvertToUnit(double traffic, DisplayUnit displayUnit)
        {
            double convertedText = 0;

            switch (displayUnit)
            {
                case DisplayUnit.KiloBit:
                    convertedText = traffic / 125;

                    break;
                case DisplayUnit.KiloByte:
                    convertedText = traffic / 1000;

                    break;
                case DisplayUnit.MegeBit:
                    convertedText = traffic / 125000;

                    break;
                case DisplayUnit.MegaByte:
                    convertedText = traffic / 1000000;

                    break;
                default:
                    break;
            }

            return convertedText;
        }

        public string GetSpeed(long received, int timeDelayInSeconds)
        {
            double realtimeTraffic = CurrentTraffic(received);

            if (prevValue == 0)
            {
                return null;
            }

            Debug.WriteLine($"R:{realtimeTraffic} Bytes");

            double kbit = ConvertToUnit(realtimeTraffic, DisplayUnit.KiloBit);
            double kbyte = ConvertToUnit(realtimeTraffic, DisplayUnit.KiloByte);
            double mbit = ConvertToUnit(realtimeTraffic, DisplayUnit.MegeBit);
            double mbyte = ConvertToUnit(realtimeTraffic, DisplayUnit.MegaByte);

            Debug.WriteLine(kbit);
            Debug.WriteLine(kbyte);
            Debug.WriteLine(mbit);
            Debug.WriteLine(mbyte);

            if (mbyte > 1)
            {
                Debug.WriteLine($"F:{mbyte:#.#} MB/s");
                return $"{mbyte:#.#} MB/s";
            }
            else
            {
                Debug.WriteLine($"F:{kbyte:#.#} KB/s");
                return $"{kbyte:#.#} KB/s";
            }

            // Debug.WriteLine("-----------------------------------");

            // string quicktext = String.Empty;

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

            // return new List<string> { kbit.ToString("#.#"), kbyte.ToString("#.#"), mbit.ToString("#.#"), mbyte.ToString("#.#") };

            // return quicktext;
        }
    }
}
