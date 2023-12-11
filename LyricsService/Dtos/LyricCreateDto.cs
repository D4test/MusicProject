using System.ComponentModel.DataAnnotations;

namespace LyricsService.Dtos
{
    public class LyricCreateDto
    {
        [Required]
        public string Lyrics { get; set; }
    }
}