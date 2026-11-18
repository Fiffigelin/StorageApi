
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Linq;
using System.Diagnostics;

namespace StorageApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);
      var connectionString = builder.Configuration.GetConnectionString("StorageApiContext") ?? throw new InvalidOperationException("Connection string 'StorageApiContext' not found.");

      builder.Services.AddDbContext<StorageApiContext>(options => options.UseSqlServer(connectionString));

      builder.Services.AddControllers();

      builder.Services.AddEndpointsApiExplorer();
      builder.Services.AddSwaggerGen();

      var app = builder.Build();

      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();

        app.UseSwaggerUI();

        // Öppna Swagger i webbläsaren när appen har startat
        app.Lifetime.ApplicationStarted.Register(() =>
        {
          var address = app.Services
          .GetRequiredService<IServer>()
          .Features.Get<IServerAddressesFeature>()?
          .Addresses.FirstOrDefault();

          if (address is null) return;

          var url = address
          .Replace("0.0.0.0", "localhost")
          .Replace("[::]", "localhost")
          .TrimEnd('/') + "/swagger";

          try
          {
            Process.Start(new ProcessStartInfo
            {
              FileName = url,
              UseShellExecute = true
            });
          }
          catch (Exception ex)
          {
            Console.WriteLine($"Kunde inte öppna webbläsaren: {ex.Message}");
          }
        });
      }

      app.UseHttpsRedirection();
      app.UseAuthorization();
      app.MapControllers();

      app.Run();
    }
  }
}
