using PointsApp.Domain.Interfaces;
using PointsApp.Infrastructure;
using PointsApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using PointsApp.Application.Interfaces;
using PointsApp.Application.Services;
using PointsApp.Application.DTOs;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddEndpointsApiExplorer();


builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("PointsAppDb"));

builder.Services.AddScoped<IPointsRepository, PointsRepository>();
builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
builder.Services.AddScoped<IPointService, PointService>();
builder.Services.AddScoped<ICommentService, CommentService>();

var app = builder.Build();


app.UseHttpsRedirection();

app.MapGet("/api/points", async (IPointService service)
    => await service.GetAllAsync());

app.MapPost("/api/points", async (PointDto point, IPointService service) =>
{
   var savedPoint =  await service.AddAsync(point);
    return Results.Ok(savedPoint);
});

app.MapPut("/api/points/{id}", async (int id, PointDto point, IPointService service) =>
{
    try
    {
        point.Id = id;
        await service.UpdateAsync(point);
        return Results.NoContent();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
});

app.MapDelete("/api/points/{id}", async (int id, IPointService service) =>
{
    try
    {
        await service.DeleteAsync(id);
        return Results.NoContent();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
});

app.MapGet("/api/points/{pointId}/comments", async (int pointId, ICommentService service)
    => await service.GetAllByPointIdAsync(pointId));

app.MapPost("/api/comments", async (CommentDto comment, ICommentService service) =>
{
    var savedComment = await service.AddAsync(comment);
    return Results.Ok(savedComment);
});

app.MapPut("/api/comments/{id}", async (int id, CommentDto comment, ICommentService service) =>
{
    try
    {
        await service.UpdateAsync(id, comment);
        return Results.NoContent();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
});

app.MapDelete("/api/comments/{id}", async (int id, ICommentService service) =>
{
    try
    {
        await service.DeleteAsync(id);
        return Results.NoContent();
    }
    catch (KeyNotFoundException)
    {
        return Results.NotFound();
    }
});

app.UseStaticFiles();
app.UseDefaultFiles();

app.Run();