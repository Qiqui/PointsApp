namespace PointsApp.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string? BackgroundColor { get; set; } = "#FFFFFF";
        public int PointId { get; set; }
        public Point? Point { get; set; }
    }
}
