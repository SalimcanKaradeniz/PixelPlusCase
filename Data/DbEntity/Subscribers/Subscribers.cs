using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.DbEntity
{
    [Table("Subscribers")]
    public class Subscribers
    {
        public Subscribers()
        {
            this.CreatedAt = DateTime.Now;
            this.IsCancelled = false;
            this.IsRefund = false;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int SubscriberNumber { get; set; }
        [Required]
        [MaxLength(150)]
        public string NameSurname { get; set; }
        [MaxLength(11)]
        [Required]
        public string IdentityNumber { get; set; }
        [MaxLength(12)]
        [Required]
        public string Phone { get; set; }
        [MaxLength(200)]
        [Required]
        public string Address { get; set; }
        [Required]
        public bool IsCancelled { get; set; }
        public bool IsRefund { get; set; }
        [Required]
        public decimal DepositFee { get; set; }
        public DateTime? CreatedAt { get; set; }

        [ForeignKey("Id")]
        public Users User { get; set; }
    }
}
