using Calendar.Constants;
using Calendar.Services.UserService;
using Calendar.Services.EventService;
using Calendar.Services.TaskboardService;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddControllers();

    builder.Services
        .AddSingleton<IUserService, UserService>()
        .AddSingleton<IEventService, EventService>()
        .AddSingleton<ITaskboardService, TaskboardService>();

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

// TODO: Create REST API for Models