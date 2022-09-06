using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleWeatherStackAPI
{
    class WeatherForecast
    {
       public float precip
        {
            get; set;
        }
        public int uv_index
        {
            get;set;
        }
        public int wind_speed
        {
            get;set;
        }

    }
}
