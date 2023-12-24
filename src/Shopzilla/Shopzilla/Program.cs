using Carter;
using Shopzilla.Client.Pages;
using Shopzilla.Components;
using Shopzilla.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);
var assembly = typeof(Program).Assembly;

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
builder.Services.AddCarter();
builder.Services.AddHttpContextAccessor();
builder.Services.RegisterRepositories();

builder.Services.AddScoped(serviceProvider =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    var currentHttpContext = httpContextAccessor.HttpContext;
    var baseAddress = $"{currentHttpContext?.Request.Scheme}://{currentHttpContext?.Request.Host}";
    var httpClient = new HttpClient
    {
        BaseAddress = new Uri(baseAddress)
    };
    return httpClient;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Counter).Assembly);

app.MapCarter();

app.Run();
