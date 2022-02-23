namespace NewsWebAPI.Models
{
    public class NewsArticleAPI
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string By { get; set; }
        public long Time { get; set; }
        public int Score { get; set; }
        public int[] Kids { get; set; }
    }
}