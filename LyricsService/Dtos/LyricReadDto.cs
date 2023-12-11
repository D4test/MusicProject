namespace LyricsService.Dtos
{
    public class LyricReadDto
    {
        public int Id { get; set; }
        public string Lyrics { get; set; }
        public int MusicId { get; set; }
    }
}