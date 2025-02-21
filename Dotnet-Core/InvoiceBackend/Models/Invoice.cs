using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InvoiceBackend.Models{
    public class Invoice{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id {get; set;}
        [Required]
        [MaxLength(50)]
        public string Name {get; set;}
        public string Email {get; set;}
        public string ImageUrl {get; set;}
        public decimal Amount {get; set;}

        public ICollection<Product> Products {get; set;} = new List<Product>();
    }
}