using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.DbEntity
{
    [Table("Users")]
    public class Users
    {
        public Users()
        {
            this.CreatedAt = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [MaxLength(50)]
        public string Password { get; set; }
        [MaxLength(12)]
        public string Phone { get; set; }
        public bool Type { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}