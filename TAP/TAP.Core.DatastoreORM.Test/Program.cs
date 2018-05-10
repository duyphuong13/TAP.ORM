using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAP.Core.DatastoreORM.Test
{
    public class Program
    {

        public static void Main(string[] args)
        {
            // Use this for local running
            //Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", @"DB.json");

            var ormTester = new ORMTester("striped-helper-186011");
            //var countryCode = ormTester.AddCountry().Result;


            var countries = ormTester.GetAllCountries(1).Result;

            countries = ormTester.GetCountriesByIds("VN2", "UK2").Result;

            //var country = ormTester.GetCountry(countryCode).Result;
            //Console.WriteLine("Country: Code: {0}, Name: {1}", country.CountryCode, country.Name );

            // playerId = ormTester.AddPlayer().Result;


            var player = ormTester.GetPlayer(5631986051842048).Result;
            Console.WriteLine("Player: Name: {0}, Score: {1}", player.Name, player.Description);

            var ormTester2 = new ORMTester("striped-helper-186011");
            var player2 = ormTester2.GetPlayer(5631986051842048).Result;
            Console.WriteLine("Player: Name: {0}, Score: {1}", player2.Name, player2.Description);
            Console.ReadLine();

        }



    }
}
