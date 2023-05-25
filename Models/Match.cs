namespace MatchNow.Models
{
    public class Match
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public string? Date { get; set; }
    }
}
