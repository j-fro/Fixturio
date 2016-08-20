using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fixturio.Models
{
    using System.ComponentModel.DataAnnotations;
    public class FilePath
    {
        public int FilePathId { get; set; }
        
        [StringLength(255)]
        public string FileName { get; set; }
        public FileType FileType { get; set; }
        public int DisplayElementId { get; set; }
        public virtual DisplayElement DisplayElement { get; set; }
    }
}