using LNS_API.Interfaces;
using LNS_API.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();
builder.Services.AddScoped<ILogin, LoginServices>();
builder.Services.AddScoped<IPapel_FileMaker, Papel_Servicios>();
builder.Services.AddScoped<IInsumos, Insumos_Servicios>();
builder.Services.AddScoped<IPlacas, Placas_Servicios>();
builder.Services.AddScoped<ICajas, Cajas_Servicios>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
