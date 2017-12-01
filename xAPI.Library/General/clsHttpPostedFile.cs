using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using xAPI.Library.Base;

namespace xAPI.Library.General
{
    public class clsHttpPostedFile :BaseEntity
    {
        public HttpPostedFile File { get; set; }
        public Int32 FormFieldID { get; set; }
    }
}
