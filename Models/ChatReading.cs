namespace MatchNow.Models
{
    public class ChatReading
    {
        public int Id { get; set; }
        public int FromId { get; set; }
        public int ToId { get; set; }
        public Boolean Read { get; set; }
    }
}
