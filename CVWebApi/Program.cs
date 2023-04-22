using AutoMapper;
using CVWebApi.Entities;
using CVWebApi.Mapper;
using CVWebApi.Repository;
using CVWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<ICVServices, CVServices>();
builder.Services.AddScoped<ICVRepo, CVRepo>();
builder.Services.AddDbContext<CVContext>(item =>
{
item.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    providerOptions => providerOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null));
});

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

var logger = new LoggerConfiguration().WriteTo.Console()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);
builder.Host.UseSerilog(((ctx, lc) => lc.ReadFrom.Configuration(ctx.Configuration)));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
if (app.Environment.IsDevelopment())
{
    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors("AllowAll");

app.Run();
