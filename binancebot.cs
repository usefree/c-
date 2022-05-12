// simple bot to check prices and found price drops

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
namespace binancebotown
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;
            client.DefaultRequestHeaders.Add("X-MBX-APIKEY", "Binance_apikey");
            try
            {
                string telegram_apilToken = "telegram_api_token";
                string telegram_chat_id = "telegram_chat_id";
                float first_candle_close_price = 0;
                float last_candle_close_price = 0;
                float price_drop = 0;
                float price_drop_percent = 0;
                float price_growth = 0;
                float price_growth_percent = 0;
                int time_to_buy = 0;
                int time_to_sell = 0;
                string telegram_message = "";
                int limit_candles = 15;
                string interval = "1h";
                string symbol = "XTZUSDT";
                string binance_url = "https://api.binance.com/api/v3/klines?symbol=" + symbol + "&interval=" + interval + "&limit=" + limit_candles;
                Console.WriteLine("binance url = " + binance_url);
                HttpResponseMessage response = await client.GetAsync(binance_url);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("-------results.items-----");
                var jArray = JArray.Parse(responseBody);
                for (int i = 0; i < limit_candles; i++)
                {
                    if (i == 0)
                    {
                        first_candle_close_price = (float)jArray[i][4];
                        Console.WriteLine("close price of the start " + first_candle_close_price);
                    }
                    else if (i == limit_candles-1)
                    {
                        last_candle_close_price = (float)jArray[i][4];
                        Console.WriteLine("close price of the end " + last_candle_close_price);
                        if (price_drop_percent > 10) {
                            time_to_buy = 1;
                        }
                        else if (price_growth_percent > 10)
                        {
                            time_to_sell = 1;
                        }
                    }
                    if (first_candle_close_price > (float)jArray[i][4])
                    {
                        price_drop = first_candle_close_price - (float)jArray[i][4];
                        price_drop_percent = price_drop * 100 / first_candle_close_price;
                        Console.WriteLine("price drop to candle " + i + " is " + price_drop + " (" + price_drop_percent + "%)");
                    }
                    else if (first_candle_close_price < (float)jArray[i][4])
                    {
                        price_growth = (float)jArray[i][4] - first_candle_close_price;
                        price_growth_percent = price_growth * 100 / first_candle_close_price;
                        Console.WriteLine("price growth to candle " + i + " is " + price_growth + " (" + price_growth_percent + "%)");
                    }
                    else if (first_candle_close_price == (float)jArray[i][4])
                    {
                        Console.WriteLine("No price mooving on candle " + i);
                    }
                }
                if (time_to_buy == 1)
                {
                    telegram_message = "time to buy, price drop more than 10%(" + price_drop_percent + "%) for " + limit_candles + " candles of " + interval + " interval";
                }
                else if (time_to_sell == 1)
                {
                    telegram_message = "time to sell, price growth more than 10%(" + price_growth_percent  + "%) for " + limit_candles + " candles of " + interval + " interval";
                }
                if (telegram_message != "")
                {
                    string urlString = "https://api.telegram.org/bot" + telegram_apilToken + "/sendMessage?chat_id=" + telegram_chat_id + "&text=" + telegram_message;
                    WebClient webclient = new WebClient();
                    webclient.DownloadString(urlString);
                }
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
        }
    }
}

// json with candles info
//[
//  [
//    1499040000000,      // Open time
//    "0.01634790",       // Open
//    "0.80000000",       // High
//    "0.01575800",       // Low
//    "0.01577100",       // Close
//    "148976.11427815",  // Volume
//    1499644799999,      // Close time
//    "2434.19055334",    // Quote asset volume
//    308,                // Number of trades
//    "1756.87402397",    // Taker buy base asset volume
//    "28.46694368",      // Taker buy quote asset volume
//    "17928899.62484339" // Ignore.
//  ]
//]
// 

//how to get telegram chat id for group

//// https://api.telegram.org/bot<YourBOTToken>/getUpdates
//Ex:

//https://api.telegram.org/bot5342354721:AAEcpFCQ74bPCg_3mna1xrROyHHqhD_FDyc/getUpdates

