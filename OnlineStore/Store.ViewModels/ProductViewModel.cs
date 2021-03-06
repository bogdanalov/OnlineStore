﻿namespace Store.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using Models;
    using Services;

    public class ProductViewModel : BaseViewModel
    {
        private const string DefaultPicturesPath = "/Content/Images/Products/";
        public List<Product> Products { get; set; }

        public Product SearchEntity { get; set; }

        public Product Entity { get; set; }

        public List<SelectListItem> Categories { get; set; }

        public List<SelectListItem> Countries { get; set; }

        public HttpPostedFileBase File { get; set; }

        public string PicturePath
        {
            get
            {
                if (this.Categories == null || this.Entity.CategoryId == 0)
                {
                    return null;
                }

                var category = this.Categories.First(x => x.Value == this.Entity.CategoryId.ToString()).Text;
                string nameAndLocation = $"{DefaultPicturesPath}{category}/";
                return nameAndLocation;
            }
        }

        protected override void Init()
        {
            this.Products = new List<Product>();
            this.SearchEntity = new Product();
            this.Entity = new Product();
            this.InitDependancies();

            base.Init();
        }

        protected override void Add()
        {
            this.IsValid = true;

            this.Entity = new Product();

            base.Add();
        }

        protected override void Edit()
        {
            ProductService mgr = new ProductService();
            this.Entity = mgr.Get(Convert.ToInt32(this.EventArgument));

            base.Edit();
        }


        protected override void Save()
        {
            ProductService mgr = new ProductService();
            if (this.IsValid)
            {
                this.SavePicture();
                if (this.Mode == "Add")
                {
                    mgr.Insert(this.Entity);
                }
                else
                {
                    mgr.Update(this.Entity);
                }
            }

            this.ValidationErrors = mgr.ValidationErrors;

            base.Save();
        }

        private void SavePicture()
        {
            if (this.File == null)
            {
                return;
            }

            try
            {
                this.File.SaveAs(HttpContext.Current.Server.MapPath(this.PicturePath) + this.File.FileName);
            }
            catch (Exception e)
            {
                this.IsValid = false;
                this.ValidationErrors.Add(new KeyValuePair<string, string>("Picture", e.Message));
            }
        }

        protected override void Delete()
        {
            ProductService mgr = new ProductService();
            this.Entity = new Product {Id = Convert.ToInt32(this.EventArgument)};
            mgr.Delete(this.Entity);
            this.Get();

            base.Delete();
        }

        protected override void ResetSearch()
        {
            this.SearchEntity = new Product();

            base.ResetSearch();
        }

        protected override void Get()
        {
            ProductService mgr = new ProductService();

            this.Products = mgr.Get(this.SearchEntity).OrderByDescending(p=>p.Id).ToList();

            base.Get();
        }

        private void InitDependancies()
        {
            if (this.Categories == null)
            {
                var catService = new CategoryService();
                this.Categories = catService.Get().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();
            }

            if (this.Countries == null)
            {
                var countryService = new CountryService();
                this.Countries = countryService.Get().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                }).ToList();
            }
        }
    }
}
