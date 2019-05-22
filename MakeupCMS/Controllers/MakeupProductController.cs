using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MakeupCMS.Models;
using MakeupCMS.Models.ViewModels;
using System.Diagnostics;
using System.IO;

namespace MakeupCMS.Controllers
{
    public class MakeupProductController : Controller
    {
        private MakeupCMSContext db = new MakeupCMSContext();

        // GET: MakeupProduct
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }

        public ActionResult New()
        {
            MakeupProductEdit makeupModel = new MakeupProductEdit
            {
                Brands = db.Brands.ToList()
            };
            return View(makeupModel);
        }

        //MakeupProducts/Create
        [HttpPost]
        public ActionResult Create(string MakeupName_New, string MakeupDescription_New, string MakeupColour_New, string MakeupCategory_New, string MakeupIngredients_New, DateTime MakeupDateOpened_New, 
            int MakeupMonthShelfLife_New, int MakeupRating_New, int? MakeupIsRecommended_New, int MakeupBrand_New)
        {

            string query = "INSERT INTO MakeupProducts (MakeupName, MakeupDescription, MakeupCategory, MakeupIngredients, MakeupDateOpened, MakeupMonthShelfLife, MakeupRating, IsRecommended, HasPic, ImgType, Brand_BrandId) " +
                "VALUES (@name, @description, @category, @ingredients, @dateOpened, @MonthShelfLife, @rating, @isRecommended, 0, 0, @bId)";

            //if makeup product was not recommended (not checked off), sets the value to 0 
            if (MakeupIsRecommended_New != 1)
            {
                MakeupIsRecommended_New = 0;
            }

            SqlParameter[] parameters = {
                new SqlParameter("@name", MakeupName_New),
                new SqlParameter("@description", MakeupDescription_New),
                new SqlParameter("@category", MakeupCategory_New),
                new SqlParameter("@ingredients", MakeupIngredients_New),
                new SqlParameter("@dateOpened", MakeupDateOpened_New),
                new SqlParameter("@MonthShelfLife", MakeupMonthShelfLife_New),
                new SqlParameter("@rating", MakeupRating_New),
                new SqlParameter("@isRecommended", MakeupIsRecommended_New),
                new SqlParameter("@bId", MakeupBrand_New),
            };

            db.Database.ExecuteSqlCommand(query, parameters);
            //Debug.WriteLine(query);
            return RedirectToAction("List");
        }
       
        public ActionResult Details(int? id)
        {
            //check to see if makeup product with this id exists in the database 
            if (id == null || db.MakeupProducts.Find(id) == null)
            {
                return HttpNotFound();
            }
            else
            {
                string query = "SELECT * from MakeupProducts WHERE makeupProductId = @id";
                return View(db.MakeupProducts.SqlQuery(query, new SqlParameter("@id", id)).FirstOrDefault());
            }
        }

        public ActionResult Edit(int? id)
        {
            //check to see if makeup product with this id exists in the database 
            MakeupProduct makeupProduct = new MakeupProduct();
            makeupProduct = db.MakeupProducts.Find(id);

            if (id==null|| makeupProduct == null)
            {
                return HttpNotFound();
            }
            else
            {
                MakeupProductEdit makeupEdit = new MakeupProductEdit
                {
                    MakeupProduct = makeupProduct,
                    Brands = db.Brands.ToList()
                };
                return View(makeupEdit);
            }
        }

        //MakeupProduct/Edit/makeupProductId
        [HttpPost]
        public ActionResult Edit(int id, string MakeupName_Edit, string MakeupDescription_Edit, string MakeupColour_Edit, string MakeupCategory_Edit, string MakeupIngredients_Edit, DateTime MakeupDateOpened_Edit,
           int MakeupMonthShelfLife_Edit, int MakeupRating_Edit, int? MakeupIsRecommended_Edit, int MakeupBrand_Edit, HttpPostedFileBase makeupImg)
        {
            string query = "UPDATE MakeupProducts " +
                "SET MakeupName=@name, MakeupDescription=@description, MakeupCategory=@category, MakeupIngredients=@ingredients," +
                "MakeupDateOpened=@dateOpened, MakeupMonthShelfLife=@MonthShelfLife, MakeupRating=@rating, IsRecommended=@isRecommended, HasPic=@pic, ImgType=@type, Brand_BrandId=@bId " +
                "WHERE MakeupProductId=@mId";

            MakeupProduct makeupProduct = db.MakeupProducts.Find(id);

            //current image values of makeup product 
            int MakeupHasPic_Edit = makeupProduct.HasPic;
            string MakeupImgType_Edit = makeupProduct.ImgType;

            //Referenced Christines FirstMVC example
            if (makeupImg?.ContentLength > 0)
            {
                //file extensioncheck taken from https://www.c-sharpcorner.com/article/file-upload-extension-validation-in-asp-net-mvc-and-javascript/
                var validTypes = new[] { "jpeg", "jpg", "png", "gif" };
                var extension = Path.GetExtension(makeupImg.FileName).Substring(1);

                if (validTypes.Contains(extension))
                {

                    string fileName = id + "." + extension;
                    string path = Path.Combine(Server.MapPath("~/imgs/makeupProducts"), fileName);
                    makeupImg.SaveAs(path);
                    
                    //New HasPic and ImgType value for makeup product 
                    MakeupHasPic_Edit = 1;
                    MakeupImgType_Edit = extension;
               
                }
            }

            //if makeup product was not recommended (not checked off), changes value to 0
            if (MakeupIsRecommended_Edit != 1)
            {
                MakeupIsRecommended_Edit = 0;
            }

            SqlParameter[] parameters = 
            {
                new SqlParameter("@name", MakeupName_Edit),
                new SqlParameter("@description", MakeupDescription_Edit),
                new SqlParameter("@category", MakeupCategory_Edit),
                new SqlParameter("@ingredients", MakeupIngredients_Edit),
                new SqlParameter("@dateOpened", MakeupDateOpened_Edit),
                new SqlParameter("@MonthShelfLife", MakeupMonthShelfLife_Edit),
                new SqlParameter("@rating", MakeupRating_Edit),
                new SqlParameter("@isRecommended", MakeupIsRecommended_Edit),
                new SqlParameter("@pic", MakeupHasPic_Edit),
                new SqlParameter("@type", MakeupImgType_Edit),
                new SqlParameter("@bId", MakeupBrand_Edit),
                new SqlParameter("@mId", id),
            };

            db.Database.ExecuteSqlCommand(query, parameters);
            return RedirectToAction("Details/" + id);
        }


        //MakeupProduct/Delete/makeupProductId
        public ActionResult Delete(int? id)
        {
            //check to see if makeup product with this id exists in the database 
            if ((id == null) || (db.MakeupProducts.Find(id) == null))
            {
                return HttpNotFound();

            }
            else
            {
                string query = "DELETE FROM MakeupProducts WHERE MakeupProductId = @id";
                db.Database.ExecuteSqlCommand(query, new SqlParameter("@id", id));
                return RedirectToAction("List");
            }
        }

        public ActionResult List()
        {
            return View(db.MakeupProducts.ToList());
        }

        [HttpGet]
        public ActionResult List(string search)
        {
            //if search value is empty, list all makeup products 
            if (search == "")
            {
                return View(db.MakeupProducts.ToList());
            }
            else
            {
                string query = "SELECT * FROM MakeupProducts WHERE MakeupName LIKE @name OR MakeupCategory LIKE @name";
                /*Referenced https://stackoverflow.com/questions/251276/howto-parameters-and-like-statement-sql */
                return View(db.MakeupProducts.SqlQuery(query, new SqlParameter("@name", "%" + search + "%")).ToList());
            }
        }
    }
}