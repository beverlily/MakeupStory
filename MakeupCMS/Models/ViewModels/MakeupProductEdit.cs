using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MakeupCMS.Models.ViewModels
{
    public class MakeupProductEdit
    {
        //Empty constructor
        public MakeupProductEdit()
        {

        }

        public virtual MakeupProduct MakeupProduct { get; set; }

        //To edit makeup, you also need to pick from a list of brands
        public IEnumerable<Brand> Brands { get; set; }
    }
}




