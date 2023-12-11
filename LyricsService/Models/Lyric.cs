using System.ComponentModel.DataAnnotations;

namespace LyricsService.Models
{
    public class Lyric
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Lyrics { get; set; }

        [Required]
        public int MusicId { get; set; }

        public Music Music { get; set; }
    }
}
