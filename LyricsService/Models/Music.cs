using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LyricsService.Models
{
    public class Music
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public int ExternalID { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<Lyric> Lyrics { get; set; } = new List<Lyric>();
     }
}