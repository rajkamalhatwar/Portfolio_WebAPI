using Portfolio_APIs.Entity;

namespace Portfolio_APIs.ViewModel
{
    public class VMEducation
    {
        public int Id { get; set; }
        public string? DegreeName { get; set; }
        public string? BranchName { get; set; }
        public string? MarkType { get; set; }
        public decimal? Marks { get; set; }
        public string? AdmissionMonth { get; set; }
        public int? AdmissionYear { get; set; }
        public string? PassingMonth { get; set; }
        public int? PassingYear { get; set; }
        public string? CollegeName { get; set; }
        public string? CollegeAddress { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }

        // 🔥 Navigation Property
        public List<VMSkill> Skills { get; set; } = new();
        public int SequenceNo { get; set; }
    }

    public class VMSkill
    {
        public int Id { get; set; }
        public string? SkillName { get; set; }
        public int? OutOf100 { get; set; }
        public int EducationId { get; set; }
        public int UserId { get; set; }
        public int? SequenceNo { get; set; }
    }
}
