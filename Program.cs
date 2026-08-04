using System.Runtime.CompilerServices;
using cardapio_digital;
using cardapio_digital.Entities;
using cardapio_digital.Endpoints;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


var builder = WebApplication.CreateBuilder(args); 

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Administrador",policy => policy.RequireRole("Administrador"));

    options.AddPolicy("Cantina",policy => policy.RequireRole("Cantina", "Administrador"));

    options.AddPolicy("Pais", policy => policy.RequireRole("Pais","Administrador"));
});

builder.Services.ConfigureHttpJsonOptions(options =>
{
    // Permite que a API entenda Enums como texto (ex: "Manha") no JSON
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    // Aceita propriedades vindas no JSON com nomes em camelCase ou PascalCase
    options.SerializerOptions.PropertyNameCaseInsensitive = true;
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
.UseSnakeCaseNamingConvention ());

//Configurações do JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) .AddJwtBearer(options =>
{
  options.TokenValidationParameters = new TokenValidationParameters
{
  ValidateIssuer = true,
  ValidateAudience = true,
  ValidateLifetime = true,
  ValidateIssuerSigningKey = true,
  ValidIssuer = builder.Configuration["Jwt:Issuer"],
  ValidAudience = builder.Configuration["Jwt:Audience"],
  IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!))
};

  options.Events = new JwtBearerEvents
{
  OnChallenge = async context =>
  {
    context.HandleResponse();
    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
    context.Response.ContentType = "application/json";
    
    await context.Response.WriteAsJsonAsync("Você precisa estar autenticado para acessar este recurso.");
   },
   OnForbidden = async context =>
   {
    context.Response.StatusCode = StatusCodes.Status403Forbidden;
    context.Response.ContentType = "application/json";

    await context.Response.WriteAsJsonAsync("Você não tem permissão para acessar este recurso.");
        }
    };
  });

//Ativar os Middlewares
var app = builder.Build();

//Indetificar quem é usuário
app.UseAuthentication();

//Verificar se o usuário pode acessar o endpoint
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/healthy", () =>
{
        var result = Results.Ok(new
        {
            status = "Healthy",
            timestamp = DateTime.UtcNow
        });
        return result;
});

app.MapEscolaEndpoints();
app.MapCadastroUsuarioEndpoints();
app.MapCardapioEndpoints();
app.MapCategoriaEndpoints();
app.MapProdutoEndpoints();
app.MapCardapioProdutoEndpoints();
app.MapLoginUsuarioEndpoints();
app.MapCadastroPaisEndpoints();
app.MapCadastroFilhoEndpoints();

app.Run(); 
