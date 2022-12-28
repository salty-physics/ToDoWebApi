using Microsoft.EntityFrameworkCore;
using ToDoWebApi.Data;
using ToDoWebApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("SqLiteCS")));

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("api/todo", async (AppDbContext context) => 
{
    var items = await context.ToDos.ToListAsync();

    return Results.Ok(items);
});

app.MapPost("api/todo", async (AppDbContext context, ToDo toDo) =>
{
    await context.ToDos.AddAsync(toDo);
    await context.SaveChangesAsync();

    return Results.Created($"api/todo/{toDo.Id}", toDo);
});

app.MapPut("api/todo/{id}", async (AppDbContext context, int id, ToDo toDo) =>
{
    var item = await context.ToDos.FirstOrDefaultAsync(x =>  x.Id == id);

    if (item == null) return Results.NotFound();

    item.ToDoName = toDo.ToDoName;

    await context.SaveChangesAsync();

    return Results.NoContent();
});


app.MapDelete("api/todo/{id}", async (AppDbContext context, int id) =>
{
    var item = await context.ToDos.FirstOrDefaultAsync(x => x.Id == id);

    if (item == null) return Results.NotFound();

    context.Remove(item);
    await context.SaveChangesAsync();

    return Results.NoContent();
});

app.Run();
