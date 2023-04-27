using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlandMarinaData
{
    [Table("Dock")]
    public class Dock
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [StringLength(30)]
        //display name in the view
        [Display(Name = "Dock Name")]
        public string Name { get; set; }

        public bool WaterService { get; set; }
        public bool ElectricalService { get; set; }

        // navigation property
        public virtual ICollection<Slip> Slips { get; set; }
    }

}
