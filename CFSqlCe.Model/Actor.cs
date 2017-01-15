using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CFSqlCe.Model
{
    public class Actor
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public String Name { get; set; }
        [Required]
        [StringLength(50)]
        public String Surname { get; set; }
        [StringLength(200)]
        public String Note { get; set; }
    }
}
