using EF_WebApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add dbcontext to the services.
builder.Services.AddDbContext<MoodleContext>(options => {
    options.UseMySql(builder.Configuration.GetConnectionString("MoodleDbConText"), new MySqlServerVersion(new Version(8, 0, 16)));
}).AddDbContext<ShandbContext>(options => {
    options.UseMySql(builder.Configuration.GetConnectionString("ShanDbConText"), new MySqlServerVersion(new Version(8, 0, 16)));
}).AddDbContext<ErpsyncContext>(options => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("ErpSyncDbContext"));
});

// Service allow all request origin 
builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}else{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseCors("AllowAll");

// app.UseForwardedHeaders(new ForwardedHeadersOptions
// {
//     ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
// });

app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
