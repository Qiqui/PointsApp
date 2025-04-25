using PointsApp.Application.DTOs;
using PointsApp.Domain.Entities;
using System.Xml.Linq;
using Point = PointsApp.Domain.Entities.Point;

namespace PointsApp.Application.Helpers
{
    public static class Mapping
    {
        public static Point ToPoint(this PointDto pointDto)
        {
            return new Point()
            {
                Id = pointDto.Id,
                X = pointDto.X,
                Y = pointDto.Y,
                Radius = pointDto.Radius,
                Color = pointDto.Color,
                Comments = pointDto.Comments.ToComments()
            };
        }

        public static PointDto ToPointDto(this Point point)
        {
            return new PointDto()
            {
                Id = point.Id,
                X = point.X,
                Y = point.Y,
                Radius = point.Radius,
                Color = point.Color,
                Comments = point.Comments.ToCommentsDto()
            };
        }

        public static List<PointDto> ToPointsDto(this List<Point> points)
        {
            var pointsDto = new List<PointDto>();

            foreach (var point in points)
                pointsDto.Add(point.ToPointDto());

            return pointsDto;
        }

        public static Comment ToComment(this CommentDto commentDto)
        {
            return new Comment()
            {
                Id = commentDto.Id,
                Text = commentDto.Text,
                BackgroundColor = commentDto.BackgroundColor,
                PointId = commentDto.PointId
            };
        }

        public static List<Comment> ToComments(this List<CommentDto> commentsDto)
        {
            var comments = new List<Comment>();

            foreach (var comment in commentsDto)
                comments.Add(comment.ToComment());

            return comments;
        }

        public static CommentDto ToCommentDto(this Comment comment)
        {
            return new CommentDto()
            {
                Id = comment.Id,
                Text = comment.Text,
                BackgroundColor = comment.BackgroundColor,
                PointId = comment.PointId
            };
        }

        public static List<CommentDto> ToCommentsDto(this List<Comment> comments)
        {
            var commentsDto = new List<CommentDto>();

            foreach (var comment in comments)
                commentsDto.Add(comment.ToCommentDto());

            return commentsDto;
        }
    }
}
