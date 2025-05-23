using Microsoft.Extensions.DependencyInjection; //throws an exception at runtime if the service wasn’t registered, and removes the compiler warning because taskService is guaranteed to be non-null.

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<TaskService>();
builder.Services.AddSingleton<TaskDatabase>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure middleware
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

var taskService = app.Services.GetRequiredService<TaskService>(); 

taskService.Run();


app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
