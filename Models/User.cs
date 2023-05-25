namespace MatchNow.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public Boolean ProfileCompleted { get; set; }
        public string? ProfilePhotoURL { get; set; }
        public int? Age { get; set; }
        public int? Height { get; set; }
        public int? Weight { get; set; } 
        public string? EyeColor { get; set; }
        public string? HairColor { get; set; }
        public string? Tattoo { get; set; }
        public string? PersonalityTraits { get; set; }
        public string? Hobbies { get; set; }
        public string? Sports { get; set; }
        public string? Music { get; set; }
        public string? InstagramURL { get; set; }
        public string? FacebookURL { get; set; }
        public string? TwitterURL { get; set; }
        public int Matches { get; set; }
        public string? MatchesAdded { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
