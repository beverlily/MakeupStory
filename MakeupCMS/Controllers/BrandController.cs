using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MakeupCMS.Models;
using System.Diagnostics;
using System.IO;

namespace MakeupCMS.Controllers
{
    public class BrandController : Controller
    {
        private MakeupCMSContext db = new MakeupCMSContext();

        // GET: Brand
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult New()
        {
            return View();
        }

        //Brand/Create
        [HttpPost]
        public ActionResult New(string BrandName_New, string BrandAbout_New)
        {
            //Starting value for HasPic and ImgType would be 0 since no image or image type on creation of new brand 
            string query = "INSERT into Brands (BrandName, BrandAbout, HasPic, ImgType)" +
                "VALUES (@name, @about, 0, 0)";

            SqlParameter[] parameters = {
                new SqlParameter("@name", BrandName_New),
                new SqlParameter("@about", BrandAbout_New),
            };

            db.Database.ExecuteSqlCommand(query, parameters);
            return RedirectToAction("List");
        }

        public ActionResult Details(int? id)
        {
            //check to see if brand with this id exists in the database 
            Brand brand = db.Brands.Find(id);
            if(id == null || brand == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(brand);
            }
        }

        public ActionResult Edit(int? id)
        {
            //check to see if brand with this id exists in the database 
            Brand brand = db.Brands.Find(id);
            if (id == null || brand == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(brand);
            }
        }

        //Brand/Edit/brandId
        [HttpPost]
        public ActionResult Edit(int id, string BrandName_Edit, string BrandAbout_Edit, HttpPostedFileBase brandImg)
        {
            string query = "UPDATE Brands " +
                "SET BrandName=@name, BrandAbout=@about, HasPic=@pic, ImgType=@type " +
                "WHERE BrandId=@id";

            Brand brand = db.Brands.Find(id);

            //current image values of makeup product 
            int BrandHasPic_Edit = brand.HasPic;
            string BrandImgType_Edit = brand.ImgType;

            //Referenced Christines FirstMVC example
            if (brandImg?.ContentLength > 0)
            {
                //file extensioncheck taken from https://www.c-sharpcorner.com/article/file-upload-extension-validation-in-asp-net-mvc-and-javascript/
                var validTypes = new[] { "jpeg", "jpg", "png", "gif" };
                var extension = Path.GetExtension(brandImg.FileName).Substring(1);

                if (validTypes.Contains(extension))
                {

                    string fileName = id + "." + extension;
                    string path = Path.Combine(Server.MapPath("~/imgs/brands"), fileName);
                    brandImg.SaveAs(path);

                    //New HasPic and ImgType value for makeup product 
                    BrandHasPic_Edit = 1;
                    BrandImgType_Edit = extension;
                }
            }

            SqlParameter[] parameters =
            {
                new SqlParameter("@name", BrandName_Edit),
                new SqlParameter("@about", BrandAbout_Edit),
                new SqlParameter("@pic", BrandHasPic_Edit),
                new SqlParameter("@type", BrandImgType_Edit),
                new SqlParameter("@id", id)
            };

            //Debug.WriteLine(query);
            db.Database.ExecuteSqlCommand(query, parameters);
            return RedirectToAction("Details/" + id);
        }

        //Brand/Delete/brandId
        public ActionResult Delete(int? id)
        {
            //check to see if brand with this id exists in the database 
            if ((id == null) || (db.Brands.Find(id) == null))
            {
                return HttpNotFound();

            }
            else
            {
                //delete from Makeup Products
                string query = "DELETE FROM MakeupProducts WHERE Brand_BrandId = @id";
                db.Database.ExecuteSqlCommand(query, new SqlParameter("@id", id));

                //then delete from brands
                query = "DELETE FROM Brands WHERE BrandId = @id";
                db.Database.ExecuteSqlCommand(query, new SqlParameter("@id", id));

                return RedirectToAction("List");
            }
        }

        public ActionResult List()
        {
            return View(db.Brands.ToList());
        }


        [HttpGet]
        public ActionResult List(string search)
        {
            //if search value is empty, list all makeup products 
            if (search == "")
            {
                return View(db.Brands.ToList());
            }
            else
            {
                string query = "SELECT * FROM Brands WHERE BrandName LIKE @name";
                /*Referenced https://stackoverflow.com/questions/251276/howto-parameters-and-like-statement-sql */
                return View(db.Brands.SqlQuery(query, new SqlParameter("@name", "%" + search + "%")).ToList());
            }
        }
    }
}
