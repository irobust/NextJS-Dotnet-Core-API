using Microsoft.EntityFrameworkCore;
using Asp.Versioning;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => {
    options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
});

builder.Services.AddControllers(options => {
    options.ReturnHttpNotAcceptable = true;
}).AddJsonOptions(options => {
		options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
});

var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__Default");

if(String.IsNullOrEmpty(connectionString)){
    connectionString = builder.Configuration.GetConnectionString("Default");
}

builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlite(connectionString)
);

builder.Services.AddApiVersioning(
                    options =>
                    {
                        options.ReportApiVersions = true;
                        options.AssumeDefaultVersionWhenUnspecified = true;
                        options.DefaultApiVersion = new ApiVersion(1.0);
                        options.ApiVersionReader = ApiVersionReader.Combine(
                            new QueryStringApiVersionReader(),
                            new HeaderApiVersionReader() { HeaderNames = { "X-Api-Version" } }
                        );
                    }).AddMvc();

var app = builder.Build();


app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapGet("/", () => {
    return Results.Ok(new[]{ "Product A", "Product B", "Product C" });
});

app.Run();
