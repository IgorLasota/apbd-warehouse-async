using Microsoft.EntityFrameworkCore;
using WarehouseAPI.DbContext;
using WarehouseAPI.Interfaces;
using WarehouseAPI.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<WarehouseDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("WarehouseDB")));
        
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers().AddXmlSerializerFormatters();
        
        builder.Services.AddScoped<WarehouseAPI.Interfaces.IWarehouseService, WarehouseService>();



        var app = builder.Build();
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}