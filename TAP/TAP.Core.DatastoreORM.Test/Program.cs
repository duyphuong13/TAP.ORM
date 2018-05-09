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
            //var country = ormTester.GetCountry(countryCode).Result;
            //Console.WriteLine("Country: Code: {0}, Name: {1}", country.CountryCode, country.Name );

            // playerId = ormTester.AddPlayer().Result;


            var player =  ormTester.GetPlayer(5664248772427776).Result;
            Console.WriteLine("Player: Name: {0}, Score: {1}", player.Name, player.Score);

            var ormTester2 = new ORMTester("striped-helper-186011");
            var player2 = ormTester2.GetPlayer(5664248772427776).Result;
            Console.WriteLine("Player: Name: {0}, Score: {1}", player2.Name, player2.Score);
            Console.ReadLine();

        }



    }
}
