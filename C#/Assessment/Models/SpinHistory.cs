namespace Assessment.Models
{
    public class SpinHistory
    {
        public int SpinHistoryId { get; set; }
        public int UserId { get; set; }
        public string? Username { get; set; }
        public int SpinResult { get; set; }
        public DateTime SpinDateTime { get; set; }
    }
}