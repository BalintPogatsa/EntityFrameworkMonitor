using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CFSqlCe.Model
{
    public class FilmTitle
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public String Title { get; set; }
        [Required]
        [StringLength(1000)]
        public String Story { get; set; }
        [Required]
        public int ReleaseYear { get; set; }
        [Required]
        public int Duration { get; set; }
        [StringLength(1000)]
        public String Notes { get; set; }

        [Required]
        public int FilmGenereId { get; set; }
        [ForeignKey("FilmGenereId")]
        public FilmGenere FilmGenere { get; set; }

        public List<Producer> Producers { get; set; }

        public List<FilmActorRole> FilmActorRoles { get; set; }
    }
}
