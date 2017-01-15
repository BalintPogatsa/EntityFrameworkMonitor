using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CFSqlCe.Model
{
    public class FilmGenere
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public String Name { get; set; }
    }
}
