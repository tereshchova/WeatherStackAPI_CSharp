using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Configuration;

namespace ConsoleWeatherStackAPI
{
    class Program
    {
        static void Main(string[] args)
        {

            string resource = "http://api.weatherstack.com/";


            string clientId = ConfigurationManager.AppSettings["accessKey"];
            int zipCode;
            string inputValue ="";

            while(inputValue.ToUpper() != "EXIT")
            {

                Console.Write("\n==> Enter the zipCode: ");
                inputValue = Console.ReadLine();
                if (int.TryParse(inputValue, out zipCode))
                {

                    using (HttpClient client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(resource + "current?access_key=" + clientId + "&query=" + zipCode);

                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                        GetWeather(client).Wait();
                    }
                }
                else
                    Console.WriteLine("Invalid Input!");

            }
                

        }

        static async Task GetWeather(HttpClient cons)
        {
            using (cons)
            {
                HttpResponseMessage res = await cons.GetAsync("");

                res.EnsureSuccessStatusCode();
                if (res.IsSuccessStatusCode)
                {
                    string weather = await res.Content.ReadAsStringAsync(); //json

                    JObject jobj = JObject.Parse(weather);
                    JToken jToken = jobj.Root;
                    try
                    {
                        string currentLocation = jToken["location"]["country"].ToString();
                        string currentRegion = jToken["location"]["region"].ToString();
                        string currentWeather = jToken["current"].ToString();


                        WeatherForecast report = Newtonsoft.Json.JsonConvert.DeserializeObject<WeatherForecast>(currentWeather);
                        Console.WriteLine("\n");

                        Console.WriteLine("Current Location is: " + currentLocation + ", " + currentRegion);
                        Console.WriteLine("-----------------------------------------------------------");
                        Console.WriteLine("\nShould I go outside? ");

                        float rainPrecip = report.precip;
                        if (rainPrecip == 0)
                            Console.WriteLine("\tYes, it is not raining. Precipitaion is " + rainPrecip);
                        else
                            Console.WriteLine("\tNo, it is not raining. Precipitaion is " + rainPrecip);

                        Console.WriteLine("\nShould I wear sunscreen? ");
                        int uv_index = report.uv_index;
                        if (uv_index > 3)
                            Console.WriteLine("\tYes, UV Index is " + uv_index);
                        else
                            Console.WriteLine("\tNo, UV Index is " + uv_index);


                        Console.WriteLine("\nCan I fly my kite? ");
                        int wind_speed = report.wind_speed;
                        if (wind_speed > 15)
                            Console.WriteLine("\tYes, wind speed is " + wind_speed);
                        else
                            Console.WriteLine("\tNo, wind speed is " + wind_speed);
                    }
                    catch
                    {
                        if (jToken["success"].ToString() == "False")
                            Console.WriteLine("\n" + jToken["error"]["info"].ToString());
                        else
                            Console.WriteLine("Unable to find the current zip code");
                    }

                    
                }

            }
        }
    }
}
