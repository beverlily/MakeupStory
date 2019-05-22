using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace MakeupCMS.Models
{
    public class MakeupProduct
    {
       
        [Key]
        public int MakeupProductId { get; set; }

        [Required, StringLength(255), Display(Name = "Name")]
        public string MakeupName { get; set; }

        [Required, StringLength(int.MaxValue), Display(Name = "Description")]
        public string MakeupDescription { get; set; }

        [Required, StringLength(255), Display(Name = "Category")]
        public string MakeupCategory { get; set; }

        [Required, StringLength(int.MaxValue), Display(Name = "Ingredients")]
        public string MakeupIngredients { get; set; }

        [Required, Display(Name = "Date Opened")]
        public DateTime MakeupDateOpened { get; set; }

        [Required, Display(Name = "Shelf Life")]
        public int MakeupMonthShelfLife { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime MakeupExpiryDate
        {
            get
            {
                return MakeupDateOpened.AddMonths(MakeupMonthShelfLife);
            }
        }
        
        [Required, Display(Name = "Rating")]
        public int MakeupRating { get; set; }

        //1 => Recommended
        //0 => Not recommended
        public int IsRecommended { get; set; }

        //Referenced Christines MVC example
        //Picture
        //1 => Picture exists (in imgs/makeupproducts/{id}.ImgType)
        //0 => Picture doesn't exist
        public int HasPic { get; set; }

        //Referenced Christines MVC example
        //Accepted image formats (jpg/jpeg/png/gif)
        public string ImgType { get; set; }

        public virtual Brand Brand { get; set; }

        }
}
 