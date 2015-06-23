using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Client.CLI
{
    class Program
    {
        private const string BaseUrl = "http://localhost:54841/";

        static void Main()
        {
            string input;

            Console.Write(" > ");
            while ((input = Console.ReadLine() ?? string.Empty) != "exit")
            {
                var parts = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (!parts.Any())
                {
                    continue;
                }

                var command = parts.First();
                var args = parts.Skip(1).ToList();

                if (command == "get")
                {
                    if (!args.Any())
                    {
                        GetTheMessage();
                    }
                    else
                    {
                        int theNumber;
                        if (int.TryParse(args.First(), out theNumber))
                        {
                            GetTheMessage(theNumber);
                        }
                    }
                }
                else if (command == "set")
                {
                    SetTheMessage(GetTheNewMessage());
                }
                else
                {
                    NoIdea();
                }
                Console.Write(" > ");
            }
        }

        private static void NoIdea()
        {
            Console.WriteLine("What?");
            Console.WriteLine();
        }

        private static string GetTheNewMessage()
        {
            Console.WriteLine("What is the new message?");
            Console.Write(" > ");
            var theNewMessage = Console.ReadLine();
            Console.WriteLine();
            return theNewMessage;
        }

        private static void SetTheMessage(string theNewMessage)
        {
            const string resource = "messages";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);

                var content = new FormUrlEncodedContent(new[]
                {
                    new KeyValuePair<string, string>("message", theNewMessage)
                });

                var result = client.PostAsync(resource, content).Result;
                Console.WriteLine("Status: {0}", result.StatusCode);
                Console.WriteLine("Content: {0}", result.Content.ReadAsStringAsync().Result);
                Console.WriteLine();
            }
        }

        private static void GetTheMessage(int theNumber = 1)
        {
            var resource = "messages";
            if (theNumber > 1)
            {
                resource += "/" + theNumber;
                Console.WriteLine("The Message x {0}:", theNumber);
            }
            else
            {
                Console.WriteLine("The Message:");
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(BaseUrl);
                var theMessage = client.GetStringAsync(resource).Result;
                Console.WriteLine(theMessage);
                Console.WriteLine();
            }
        }
    }
}
