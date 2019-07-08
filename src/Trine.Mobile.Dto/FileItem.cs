using System;
using System.Collections.Generic;
using System.Text;

namespace Trine.Mobile.Dto
{
    public class FileItem
    {
        public string Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FileName { get; set; }
        public string FileUrl { get; set; }
    }
}
