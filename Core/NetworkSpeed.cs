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
            if (recievedBytes > 0)
            {
                Debug.WriteLine($"{recievedBytes}={(recievedBytes / 1024f).ToString("#.##")}");
            }

            prevValue = received;
            return recievedBytes / 1024f;
        }

        public string GetSpeed(long received)
        {
            double _speed = Speed(received);

            if (buffer.Count < count)
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
            }

            string quicktext = String.Empty;
            if (_speed == 0)
            {
                quicktext = $"";
            }
            else if (_speed > 1 && _speed < 1024)
            {
                quicktext = $"{_speed.ToString("#")} KB/s";
            }
            else if (_speed > 1024)
            {
                quicktext = $"{(_speed / 100f).ToString("#.#")} MB/s";
            }

            return quicktext;
        }
    }
}
