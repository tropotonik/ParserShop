using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ParserShopWeb.Models
{
    public class Entities
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Price { get; set; }
        public string Url { get; set; }
        public string Url_Image { get; set; }

        public Entities(string Name, string Price, string Url, string Url_Image, string Description)
        {
            this.Name = Name;
            this.Price = Price;
            this.Url = Url;
            this.Url_Image = Url_Image;
            this.Description = Description;
        }
    }
}