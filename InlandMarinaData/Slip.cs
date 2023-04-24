using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace InlandMarinaData
{
    [Table("Slip")]
    public class Slip
    {
        public int ID { get; set; }
        public int Width { get; set; }
        public int Length { get; set; }

        [Display(Name = "Dock")]
        public int DockID { get; set; }

        // navigation properties
        public virtual Dock Dock { get; set; }
        public virtual ICollection<Lease> Leases { get; set; }
    }
}
