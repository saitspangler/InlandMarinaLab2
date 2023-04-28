using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InlandMarinaData
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        //auto-generate id
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please enter your first name")]
        [StringLength(30)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please enter your last name")]
        [StringLength(30)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please enter a valid phone number")]
        [RegularExpression(@"^\d{10}$")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Please enter your city")]
        [StringLength(30)]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter your username")]
        [StringLength(30)]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(30)]
        public string Password { get; set; }

        // navigation property
        public virtual ICollection<Lease> Leases { get; set; }
    }
}
