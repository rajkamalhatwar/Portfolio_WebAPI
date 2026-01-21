namespace Portfolio_APIs.Entity
{
    public class ExperianceEntity
    {
        public int Id { get; set; }

        public string? CompanyName { get; set; }
        public string? Designation { get; set; }

        public string? JoiningMonth { get; set; }
        public int? JoiningYear { get; set; }

        public string? ReleaseMonth { get; set; }
        public int? ReleaseYear { get; set; }

        public bool Present { get; set; }

        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }

        public string? CompanyAddress { get; set; }

        public int SequenceNo { get; set; }
        public int UserId { get; set; }

        public bool IsActive { get; set; }
        public List<ExperienceAchievementEntity> Achievements { get; set; } = new();

    }
    public class ExperienceAchievementEntity
    {
        //public int Id { get; set; }
        public string? Achievement { get; set; } 
       // public int ExperienceId { get; set; }    
        
        
    }
}
