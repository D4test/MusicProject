using System.ComponentModel.DataAnnotations;

namespace MusicService.Models
{
    public class Music
    {
        [Key]
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public int Year { get; set; }
    }
}
