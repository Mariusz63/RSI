using CinemaReservationAPI.Fonts;
using CinemaReservationAPI.Middleware;
using PdfSharp.Fonts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

GlobalFontSettings.FontResolver = new CustomFontResolver();


app.UseMiddleware<BasicAuthMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();