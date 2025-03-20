using Calendar.Services.UserService;
using Calendar.Services.EventService;
using Calendar.Services.TaskboardService;
using Calendar.Services.SampleDataService;
using Calendar.Services.SchedulingService.DumbScheduler;
using Calendar.Services.SchedulingService.ETF;
using Calendar.Services.SchedulingService.UTMTK;
using Calendar.Constants;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    builder.Services
        .AddEndpointsApiExplorer()
        .AddSwaggerGen()
        .AddControllers();

    builder.Services
        .AddSingleton<IUserService, UserService>()
        .AddSingleton<IEventService, EventService>()
        .AddSingleton<ITaskboardService, TaskboardService>()
        .AddSingleton<ISampleDataService, SampleDataService>()
        .AddSingleton<IDumbSchedulerService, DumbSchedulerService>()
        .AddSingleton<IETFService, ETFService>()
        .AddSingleton<IUTMTKService, UTMTKService>();

    builder.Services.Configure<IISServerOptions>(options =>
    {
        options.MaxRequestBodySize = Constants.API_REQUEST_MAX_BODY_SIZE;
    });

    builder.Services.AddCors(setup => {
        setup.AddDefaultPolicy(policyBuilder => {
            policyBuilder
                .WithOrigins(Constants.FRONTEND_URL)
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .AllowAnyHeader();
        });
    });
}

WebApplication app = builder.Build();
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseCors();
    app.MapControllers();
}

app.Run();
