using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AimlWPF
{
    class AimlContext : DbContext
    {
        public DbSet<Aiml> Aimls { get; set; }
        public DbSet<Category> Categories { get; set; }
        private string currentDir = Directory.GetCurrentDirectory();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
    => options.UseSqlite("Data Source=" + Path.Combine(currentDir, "Database", "AIMLTest.db"));



    }


    public class Aiml
    {

        public int AimlId { get; set; }
        public string Content { get; set; }
        public string Pattern { get; set; }
        public string Template { get; set; }

        public int Mood { get; set; }

        public int CategoryId { get; set; }

        public Category category { get; set; }


    }

    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public List<Aiml> Aimls { get; } = new List<Aiml>();

        public override string ToString()
        {
            return Name;
        }
    }

    [XmlRoot("category")]
    public class Content
    {
        [XmlElement("pattern")]
        public string pattern { get; set; }

        [XmlElement("template")]
        public string template { get; set; }
    }
}
