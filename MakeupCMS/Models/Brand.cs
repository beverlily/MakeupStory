using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace MakeupCMS.Models
{
    public class Brand
    {
        [Key]
        public int BrandId { get; set; }

        [Required, StringLength(255), Display(Name = "Name")]
        public string BrandName { get; set; }

        [Required, StringLength(int.MaxValue), Display(Name = "About")]
        public string BrandAbout { get; set; }

        //Referenced Christines MVC example
        //Picture
        //1 => Picture exists (in imgs/brands/{id}.ImgType)
        //0 => Picture doesn't exist
        public int HasPic { get; set; }

        //Referenced Christines MVC example
        //Accepted image formats (jpg/jpeg/png/gif)
        public string ImgType { get; set; }

        public virtual ICollection<MakeupProduct> MakeupProducts { get; set; }

    }
}