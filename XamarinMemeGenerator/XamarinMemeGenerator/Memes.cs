using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace XamarinMemeGenerator
{
    public class Memes
    {
        //Gets a list of all available memes on this API
        public static async Task<ObservableCollection<string>> GetMemesList()
        {

            var client = new HttpClient();

            //headers required to call the service (API key and Accept type)
            client.DefaultRequestHeaders.Add("X-Mashape-Key", "XBbhHT1nvvmshsTLVkHJuWlfdUepp17mN4HjsnIpb54NzH04fZ");
            client.DefaultRequestHeaders.Add("Accept", "text/plain");

            //Actually calls the service and returns a json string
            string response = await client.GetStringAsync("https://ronreiter-meme-generator.p.mashape.com/images");

            //converts json string into a ObservableCollection of strings
            var tmpList = JsonConvert.DeserializeObject<ObservableCollection<string>>(response);

            Regex regexFirstCharIsDigit = new Regex(@"\d");
            ObservableCollection<string> memesList = new ObservableCollection<string>();

            //This API has some garbage, so let's clean some of the bad items
            foreach (string item in tmpList.OrderBy(i => i))
            {
                if (!string.IsNullOrWhiteSpace(item) && !regexFirstCharIsDigit.IsMatch(item)) //is item is not null or whitespace and doesn't have digits then add to list
                {
                    memesList.Add(item);
                }
            }
            return memesList;
        }

        //Given a meme, top and bottom texts this will return an image
        public static async Task<byte[]> GenerateMeme(string meme, string topText, string bottomText)
        {
            //This Meme Generator Api has a problem with non-ascii chars, so we strip them just to avoid it crashing.
            bottomText = Regex.Replace(bottomText, @"[^\u0000-\u007F]", string.Empty);
            topText = Regex.Replace(topText, @"[^\u0000-\u007F]", string.Empty);

            var client = new HttpClient();

            //headers required to call the service (API key and Accept type)
            client.DefaultRequestHeaders.Add("X-Mashape-Key", "XBbhHT1nvvmshsTLVkHJuWlfdUepp17mN4HjsnIpb54NzH04fZ");

            //Actually calls the service and returns a byte array for the image
            return await client.GetByteArrayAsync("https://ronreiter-meme-generator.p.mashape.com/meme?bottom=" + bottomText + "&top=" + topText + "&meme=" + meme);
        }
    }
}

