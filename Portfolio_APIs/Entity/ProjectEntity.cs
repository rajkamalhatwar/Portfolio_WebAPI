namespace Portfolio_APIs.Entity
{
    public class ProjectEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? ProjectName { get; set; }

        public string? ShortDescription { get; set; }

        public string? GitHubLink { get; set; }

        public string? LiveLink { get; set; }

        public string? DemoLink { get; set; } 

        public string? TechStack { get; set; }
        public int SequenceNo { get; set; }

        public List<ProjectFeaturesEntity> Features { get; set; } = new();
    }

    public class ProjectFeaturesEntity
    { 
        public string? Feature { get; set; } 
    }
}
