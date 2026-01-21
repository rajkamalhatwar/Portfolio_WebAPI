using Portfolio_APIs.Entity;

namespace Portfolio_APIs.ViewModel
{
    public class VMProject
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string? ProjectName { get; set; }

        public string? ShortDescription { get; set; }

        public string? GitHubLink { get; set; }

        public string? LiveLink { get; set; }

        public string? DemoLink { get; set; }

        public int SequenceNo { get; set; }

        public string? TechStack { get; set; }

        public List<VMProjectFeatures> Features { get; set; } = new();
    } 

    public class VMProjectFeatures
    {
        public string? Feature { get; set; }
    }
}
