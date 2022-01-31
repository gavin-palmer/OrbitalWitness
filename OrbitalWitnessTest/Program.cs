using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OrbitalWitnessTest.Persistance;

namespace OrbitalWitnessTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            InitiateDb();
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        public static void InitiateDb()
        {
            using (var dbContext = new OrbitalWitnessDbContext())
            {
                dbContext.Database.EnsureCreated();
            }
        }
    }
}
