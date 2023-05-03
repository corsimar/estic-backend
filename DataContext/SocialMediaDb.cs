using MatchNow;
using Microsoft.EntityFrameworkCore;

namespace SocialMedia.Models
{
    public class SocialMediaDb:DbContext
    {
        public SocialMediaDb(DbContextOptions<SocialMediaDb> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        
       
    }
}
