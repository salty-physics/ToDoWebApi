using Microsoft.EntityFrameworkCore;
using ToDoWebApi.Data;

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


app.Run();
