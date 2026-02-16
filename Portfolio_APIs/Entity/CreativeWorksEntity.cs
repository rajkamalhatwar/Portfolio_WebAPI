namespace Portfolio_APIs.Entity
{
    public class CreativeWorksEntity
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }
        //public IFormFile? formFile { get; set; }
        public string? ImageURL { get; set; }
        public string? RelativeURL { get; set; }
        public int WorkCategoryId { get; set; }
        public int UserId { get; set; }
        public string? CategoryName { get; set; }
    }

    public class WorkCatogoryEntity
    {
        public int Id { get; set; }
        public string? CategoryName { get; set; }
        public int? SequenceNo { get; set; }
        public int UserId { get; set; }
    }
}
