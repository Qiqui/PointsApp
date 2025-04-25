namespace PointsApp.Application.DTOs
{
    public class PointDto
    {
        public int Id { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Radius { get; set; } = 10;
        public string Color { get; set; } = "#FF0000";
        public List<CommentDto> Comments { get; set; } = new();
    }
}
