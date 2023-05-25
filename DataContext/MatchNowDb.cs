using MatchNow.Models;
using Microsoft.EntityFrameworkCore;

namespace MatchNow.Models
{
    public class MatchNowDb:DbContext
    {
        public MatchNowDb(DbContextOptions<MatchNowDb> options) : base(options)
        {
        
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<ChatReading> ChatReadings { get; set; }
        
        public string[] StringToArray(string s)
        {
            string[] result = s.Split(';');
            return result;
        }
    }
}
