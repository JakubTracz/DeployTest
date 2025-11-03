using DeployTest;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=tcp:deploy-test.database.windows.net,1433;Initial Catalog=deploye-test;Persist Security Info=False;User ID=jakubadmin;Password=Gitara01!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    await using var scope = app.Services.CreateAsyncScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await dbContext.Database.MigrateAsync();
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapGet("/", () => "Hello World 2!");
app.MapGet("/people", (AppDbContext dbContext) => dbContext.People.ToListAsync());
app.MapPost("/people", async (AppDbContext dbContext) =>
{
    var faker = new Bogus.Faker<Person>();
    var person = faker.Generate();
    dbContext.People.Add(person);
    await dbContext.SaveChangesAsync();
    return Results.Created($"/people/{person.Id}", person);
});

app.Run();