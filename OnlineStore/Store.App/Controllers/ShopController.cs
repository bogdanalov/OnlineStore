﻿using Store.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Store.App.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        public ActionResult Index()
        {
            var managerProducts = new ProductService();
            var managerCategories = new CategoryService();

            var products = managerProducts.Get();
            var categories = managerCategories.Get();
            var generic = new { Products = products, Categories = categories };
            return View(generic);
        }
    }
}