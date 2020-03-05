using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace HankerRank_Movie_Titles
{
    class Program
    {
        public static string[] getMovieTitles(string substr)
        {
            string jsonString = getJsonString(substr);

            Dictionary<string, dynamic> jsonData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(jsonString);

            var totalPages = (int)jsonData.FirstOrDefault(x => x.Key.Equals("total_pages")).Value;

            List<string> movieList = new List<string>();
            var movieData = jsonData.FirstOrDefault(x => x.Key.Equals("data")).Value;
            movieList = populateListFromJson(movieList, movieData);

            if (totalPages > 1)
            {
                for (int i = 1; i < totalPages; i++)
                {
                    var currentPage = (int)jsonData.FirstOrDefault(x => x.Key.Equals("page")).Value;
                    string additionalJson = getJsonString(substr, currentPage + 1);

                    Dictionary<string, dynamic> additionalJsonData = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(additionalJson);

                    var additionalMovieData = additionalJsonData.FirstOrDefault(x => x.Key.Equals("data")).Value;

                    movieList = populateListFromJson(movieList, additionalMovieData);
                }
            }
            
            movieList = movieList.OrderBy(x => x).ToList();

            return movieList.ToArray();
        }

        private static string getJsonString(string substr, int page = 0)
        {
            if (page != 0)
            {
                string pageParam = $"&page={page}";
                substr += pageParam;
            }

            string url = "https://jsonmock.hackerrank.com/api/movies/search/?Title=";

            string jsonString;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + substr);
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    jsonString = reader.ReadToEnd();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return jsonString;
        }

        private static List<string> populateListFromJson(List<string> movieList, JArray movieData)
        {
            var data =  movieData.Select(movie => movie["Title"].ToString()).ToList();
            movieList.AddRange(data);

            return movieList;
        }

        static void Main(string[] args)
        {
            var movies = getMovieTitles("waterworld");
            foreach (var movie in movies)
            {
                Console.Out.WriteLine(movie);
            }


            Console.ReadKey();
        }

        public static void fizzBuzz(int n)
        {
            for (int i = 1; i <= n; i++)
            {
                if (i % 3 == 0 && i % 5 == 0)
                {
                    Console.Out.WriteLine("FizzBuzz");
                    continue;
                }

                else if (i % 3 == 0 && i % 5 != 0)
                {
                    Console.Out.WriteLine("Fizz");
                    continue;
                }

                else if (i % 5 == 0 && i % 3 != 0)
                {
                    Console.Out.WriteLine("Buzz");
                    continue;
                }

                Console.Out.WriteLine(i);

            }

            Console.ReadKey();
        }
    }
}
