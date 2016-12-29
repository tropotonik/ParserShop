using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using System.IO;

namespace ParserShopWeb.Models
{
    public class Program
    {
         const string domen = "https://www.softmagazin.ru";
         public const string start_url = "https://www.softmagazin.ru/soft/antivirusy_i_bezopasnost/laboratoriya_kasperskogo_kaspersky/produkty_dlya_doma_laboratorii_kasperskogo";

        //reading url -> return html
         public static string html(string url)
        {
            var request = (HttpWebRequest)WebRequest.Create(url);
            var page = "";
            try
            {  using (var response = request.GetResponse() as HttpWebResponse)
                    if (request.HaveResponse && response != null)
                        using (var reader = new StreamReader(response.GetResponseStream()))
                             page = reader.ReadToEnd();

            }
            catch (WebException wex)
            {
                if (wex.Response != null)
                    using (var errorResponse = (HttpWebResponse)wex.Response)
                        using (var reader = new StreamReader(errorResponse.GetResponseStream()))
                             page = reader.ReadToEnd();
            }
            return page;
        }

        //reading html -> return product_urls
        public static IEnumerable<string> Product_Urls(string html, string url)
        {
            var htmlpage = new HtmlDocument();
            htmlpage.LoadHtml(html);
            var hrefs = new List<string>();
            url = url.Replace(domen, "");
            foreach (var link in htmlpage.DocumentNode.SelectNodes("//a[@href]"))
            {
                var att = link.Attributes["href"];
                hrefs.Add(att.Value);
            }
            var product_hrefs = hrefs.Where(s => s.Contains(url) && s != url);
            return product_hrefs;
        }

        //forming product list
        public static List<Entities> Entities(IEnumerable<string> product_hrefs)
        {
            var list_entities = new List<Entities>();
            foreach (var link in product_hrefs)
            {
                var page_data = html(domen + link);
                var htmlpage = new HtmlDocument();
                htmlpage.LoadHtml(page_data);

                var name = "";
                var price = "";
                var image_url = "";
                var descr = "";
                var href = domen + link;

                var name_h1 = htmlpage.DocumentNode.SelectNodes("//h1");
                var price_div = htmlpage.DocumentNode.SelectNodes("//div[@class='price-now-sum']");
                var image_url_img = htmlpage.DocumentNode.SelectSingleNode("//div[@class='catalog-detail-image catalog-no-image']//img[@src]");
                var descr_on_p = htmlpage.DocumentNode.SelectNodes("//div[@class='catalog-right-text']//p");
                var descr_on_ul = htmlpage.DocumentNode.SelectNodes("//div[@class='catalog-right-text']//ul");

                if (name_h1 != null && price_div != null && image_url_img != null && (descr_on_p != null || descr_on_ul != null))
                {
                    name = name_h1.FirstOrDefault().InnerText;
                    price = price_div.FirstOrDefault().InnerText;
                    image_url = domen + image_url_img.Attributes["src"].Value;
                    descr = descr_on_p.FirstOrDefault().InnerText + descr_on_ul.FirstOrDefault().InnerText;
                }

                list_entities.Add(new Entities(name, price, href, image_url, descr));
            }
            return list_entities;
        }
    }
}