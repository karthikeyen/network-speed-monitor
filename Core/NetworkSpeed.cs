using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace NetworkMon.Core
{
    public class NetworkSpeed
    {
        private long prevValue;
        private int count = 8;
        private double old;
        private List<double> buffer = new List<double>();

        public double Speed(long received)
        {
            long recievedBytes = received - prevValue;
            Debug.WriteLine($"{DateTime.Now.ToString("hh:mm:ss:fff")} :R: {recievedBytes} Bytes");
            if (recievedBytes > 0)
            {
                // Debug.WriteLine($"{recievedBytes}={(recievedBytes / 1024f).ToString("#.##")}");
            }

            prevValue = received;
            return recievedBytes / 125000f;
        }

        public string GetSpeed(long received, int timeDelayInSeconds)
        {
            double _speed = Speed(received);

            /* if (buffer.Count < count)
            {
                buffer.Add(_speed);
            }

            if (buffer.Count == count)
            {
                _speed = buffer.Average(item => item);

                buffer.Clear();

                old = _speed;
            } 
            else
            {
                _speed = old;
            } */

            // _speed = _speed / (timeDelayInSeconds);

            string quicktext = String.Empty;

            Debug.WriteLine(_speed);

            if (_speed == 0)
            {
                quicktext = $"";
            }
            else if(_speed > 1)
            {
                quicktext = $"{_speed.ToString("#")} Mb/s ({(_speed * 125).ToString("#.#")} KB/s)";
            }

            if (_speed < 1)
            {
                if ((_speed * 1000) > 1)
                {
                    quicktext = $"{(_speed * 1000).ToString("#.#")} Kbps";
                }
            }

            //else if (_speed > 1 && _speed < 1024)
            //{
            //    quicktext = $"{_speed.ToString("#")} KB/s";
            //}
            //else if (_speed > 1024)
            //{
            //    quicktext = $"{(_speed / 100f).ToString("#.#")} MB/s";
            //}

            return quicktext;
        }
    }
}
