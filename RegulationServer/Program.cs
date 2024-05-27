using RegulationLib.Models;
using RegulationServer.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Regulation API", Version = "v1" });
});

// Configuration de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

#region Components
Pump pump1 = new Pump(1, "Pompe", 12);
TemperatureSensor sensor1 = new TemperatureSensor(1,"test", "28-00000c96e4f5");
#endregion

#region Controllers
builder.Services.AddSingleton(Pump.Pumps);
builder.Services.AddSingleton(TemperatureSensor.Sensors);
builder.Services.AddControllers();
#endregion



var app = builder.Build();

app.UseCors("AllowAllOrigins");

app.UseRouting(); // Ajout de UseRouting

app.MapControllers();

app.UseDeveloperExceptionPage();
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));


app.Run("http://0.0.0.0:5000");
