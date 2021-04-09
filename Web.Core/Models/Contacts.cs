using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Core.Models
{
    [Table("Contacts")]
    public class Contacts
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        [StringLength(100)] public string Company { get; set; }
        [Required]
        [StringLength(100)] public string Email { get; set; }
        [Required]
        [StringLength(100)] public string PhoneNumber { get; set; }
       
    }
}
