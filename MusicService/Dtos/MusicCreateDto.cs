using System.ComponentModel.DataAnnotations;

namespace MusicService.Dto
{
    public class MusicCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Year { get; set; }
    }
}