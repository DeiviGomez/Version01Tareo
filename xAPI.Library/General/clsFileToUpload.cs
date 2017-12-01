using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace xAPI.Library.General
{
    public class clsFileToUpload
    {
        public byte[] FileBytes { get; set; }
        public String FileName { get; set; }
        public int ContentLength { get; set; }
        public String ContentType { get; set; }

    }
}
