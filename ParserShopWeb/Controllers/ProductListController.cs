using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ParserShopWeb.Models;
using System.Web.Mvc;

namespace ParserShopWeb.Controllers
{
    public class ProductListController : Controller
    {
        // GET: ProductList
        public ActionResult Index()
        {
            var start_url = Program.start_url;
            var result = Program.Entities(Program.Product_Urls(Program.html(start_url), start_url));
            return View(result); 
        }
    }
}