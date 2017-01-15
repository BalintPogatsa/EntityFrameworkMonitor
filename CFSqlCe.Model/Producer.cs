using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace CFSqlCe.Model
{
    public class Producer
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public String FullName { get; set; }
        [Required]
        [StringLength(50)]
        public String Email { get; set; }
        [StringLength(200)]
        public String Note { get; set; }

        public List<FilmTitle> FileTitles { get; set; }
    }
}
