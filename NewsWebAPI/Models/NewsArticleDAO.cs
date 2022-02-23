namespace NewsWebAPI.Models
{
    public class NewsArticleDAO
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Uri { get; set; }
        public string? PostedBy { get; set; }
        public DateTime PostTime { get; set; }
        public int Score { get; set; }
        public int CommentCount { get; set; }
        public DateTime Timestamp { get; set; }
    }
}