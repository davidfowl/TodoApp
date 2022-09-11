using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sample.Migrations;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("Todos") ?? "Data Source=Todos.db";

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSqlite<TodoDbContext>(connectionString);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = builder.Environment.ApplicationName, Version = "v1" });
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{builder.Environment.ApplicationName} v1"));
}

app.MapFallback(() => Results.Redirect("/swagger"));

app.MapGet("/todos", async (TodoDbContext db) => await db.Todos.ToListAsync());

app.MapGet("/todos/{id:int}", async (TodoDbContext db, int id) =>
{
    return await db.Todos.FindAsync(id) switch
    {
        { } todo => Results.Ok(todo),
        null => Results.NotFound()
    };
});

app.MapPost("/todos", async (TodoDbContext db, Todo todo) =>
{
    await db.Todos.AddAsync(todo);
    await db.SaveChangesAsync();

    return Results.Created($"/todos/{todo.Id}", todo);
});

app.MapPut("/todos/{id:int}", async (TodoDbContext db, int id, Todo todo) =>
{
    if (id != todo.Id) return Results.BadRequest();

    if (!await db.Todos.AnyAsync(x => x.Id == id)) return Results.NotFound();

    db.Update(todo);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.MapDelete("/todos/{id:int}", async (TodoDbContext db, int id) =>
{
    Todo? todo = await db.Todos.FindAsync(id);
    if (todo is null) return Results.NotFound();

    db.Todos.Remove(todo);
    await db.SaveChangesAsync();

    return Results.Ok();
});

app.Run();