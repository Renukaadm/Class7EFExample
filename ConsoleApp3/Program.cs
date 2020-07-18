using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;

namespace ConsoleApp3
{
    class Program
    {
        static void Main(string[] args)
        {
            MyContext myContext = new MyContext();

            Guid guid = Guid.Parse("151a0626-1d8b-54a0-af91-2c2f9b40ff91");

            ApplicantEducationPoco poco = 
                myContext.ApplicantEducations
                  .Where(a => a.Id == guid)
                  .FirstOrDefault();

            Console.WriteLine($"{poco.Id} - {poco.Major}");

            poco.Major = ".NET Bridge";

            myContext.SaveChanges();

        }
    }


    public class MyContext : DbContext
    {
        public static readonly ILoggerFactory MyLoggerFactory
            = LoggerFactory.Create(builder => {
                builder.AddConsole();
            });

        public DbSet<ApplicantEducationPoco> 
            ApplicantEducations { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            config.AddJsonFile(path, false);
            var root = config.Build();
            string connStr = root.GetSection("ConnectionStrings").GetSection("DataConnection").Value;

            optionsBuilder
                .UseLoggerFactory(MyLoggerFactory)
                .UseSqlServer(connStr);
            
            base.OnConfiguring(optionsBuilder);
        }

    }



    [Table("Applicant_Educations")]
    public class ApplicantEducationPoco
    {
        [Key]
        public Guid Id { get; set; }
        public Guid Applicant { get; set; }
        public string Major { get; set; }
        [Column("Certificate_Diploma")]
        public string Certificate { get; set; }

    }




}
