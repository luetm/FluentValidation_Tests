

using BlazorServer;
using Ductus.FluentDocker.Builders;
using Ductus.FluentDocker.Model.Common;
using Ductus.FluentDocker.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Blazored.FluentValidation.Tests;

public class SharedTestContext: WebApplicationFactory<ITestReferenceServer>, IAsyncLifetime
{
    // WebApplicationFactory
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(l => l.ClearProviders());
        
        builder.ConfigureTestServices(services =>
        {
            services.RemoveAll(typeof(IHostedService));
        });
    }
    
    
    // FluentDocker
    
    public const string AppUrl = "http://localhost:7779";

    private static readonly string DockerComposeFile =
        Path.Combine(Directory.GetCurrentDirectory(), (TemplateString)"../../../docker-compose-tests.yml");

    private readonly ICompositeService _dockerService = new Builder()
        .UseContainer()
        .UseCompose()
        .FromFile(DockerComposeFile)
        .RemoveOrphans()
        .WaitForHttp("test-app", AppUrl)
        .Build();

        
    // Playwright
    
    private IPlaywright _playwright;
    public IBrowser Browser { get; private set; }

    
    // IAsyncLifetime
    
    public async Task InitializeAsync()
    {
        _dockerService.Start();
        _playwright = await Playwright.CreateAsync();
        Browser = await _playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
        {
            Headless = true,
            SlowMo = 20,
        });
    }

    public async Task DisposeAsync()
    {
        _dockerService.Dispose();
        
        await Browser.DisposeAsync();
        _playwright.Dispose();
    }
}