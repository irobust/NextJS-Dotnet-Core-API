using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using InvoiceBackend.Validators;

namespace InvoiceBackend.Models{
    public class Product{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [MaxLength(10)]
        public string Name { get; set; }
        public decimal Price { get; set; }

        // [ForeignKey("InvoiceId")]
        public Invoice Invoice {get; set;}
        public int InvoiceId {get; set;}
    }
}