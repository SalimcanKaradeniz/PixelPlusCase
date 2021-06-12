using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.DbEntity
{
    [Table("Invoice")]
    public class Invoice
    {
        public Invoice()
        {
            this.CreatedAt = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int InvoiceNumber { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        public decimal? TaxRate { get; set; }
        [Required]
        public bool IsPayment { get; set; }
        [Required]
        public DateTime ExpriyDate { get; set; }
        public DateTime? CreatedAt { get; set; }

        [ForeignKey("Id")]
        public Users User { get; set; }
    }
}
