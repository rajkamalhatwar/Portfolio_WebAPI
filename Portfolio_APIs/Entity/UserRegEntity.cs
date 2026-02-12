namespace ProjectAPI.Entity
{
    public class UserRegEntity
    {
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? MobileNo { get; set; }
        public string? Password { get; set; }
        public string? Designations { get; set; }
        public string? HeroLine { get; set; }
        public string? ShortAbout { get; set; }
        public string? LongAbout { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? TwitterLink { get; set; }
        public string? LinkedInLink { get; set; }
        public string? GitHubLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? BehanceLink { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsActive { get; set; } 
        public string? PhotoUrl { get; set; }
        public string? PhotoRelativeUrl { get; set; }
    }

    public class UserRegOperationsEntity
    {
        public List<UserRegEntity> ? GetUsersById { get; set; }
    }
}
