using Amazon;
using Amazon.SimpleNotificationService;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sanlam.Banking.Data.EF;
using Sanlam.Banking.Data.SQL;
using Sanlam.Banking.Module.Configuration;
using Sanlam.Banking.Module.Metrics;
using Sanlam.Banking.Module.Notification;
using Sanlam.Banking.Module.Processor;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        builder.Services.AddDbContext<BankDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("BankDb")));

        var awsConfig = configuration.GetSection("AwsSns");
        var snsOptions = awsConfig.Get<AwsSnsOptions>();
        builder.Services.Configure<AwsSnsOptions>(awsConfig);

        var region = RegionEndpoint.GetBySystemName(snsOptions.Region);
        builder.Services.AddSingleton<IAmazonSimpleNotificationService>(new AmazonSimpleNotificationServiceClient(region));

        builder.Services.AddScoped<Sanlam.Banking.Data.EF.IAccountRepository, EFAccountRepository>();
        builder.Services.AddScoped<IBankingNotificationManager, BankingNotificationManager>();
        builder.Services.AddScoped<IBankAccountProcessor, BankAccountProcessor>();
        builder.Services.AddScoped<IBankingMetricFactory, BankingMetricFactory>();

        builder.Services.AddControllers();

        var app = builder.Build();
        app.MapControllers();
        app.Run();
    }
}

