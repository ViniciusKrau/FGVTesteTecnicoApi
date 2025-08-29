
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Scan(scan => scan
    .FromAssemblyOf<Program>()
        .AddClasses(classes => classes.InNamespaces("TesteTecnicoApi.Repositories"))
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        .AddClasses(classes => classes.InNamespaces("TesteTecnicoApi.Service"))
            .AsSelf()
            .WithScopedLifetime());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
