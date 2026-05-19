using Microsoft.EntityFrameworkCore;
using PJATK_APBD_Cw8_s32640.Infrastructure;
using PJATK_APBD_Cw8_s32640.Services;

namespace PJATK_APBD_Cw8_s32640;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        
        builder.Services.AddControllers();
        
        builder.Services.AddOpenApi();
        
        builder.Services.AddScoped<IHospitalService, HospitalService>();
        builder.Services.AddDbContext<HospitalDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/openapi/v1.json", "PJATK-APBD-Cw8 v1"));
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();
        
        app.MapControllers();

        app.Run();
    }
}