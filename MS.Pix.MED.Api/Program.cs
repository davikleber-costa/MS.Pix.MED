using MS.Pix.MED.Integration.Jdpi;
using MS.Pix.MED.Infrastructure.Extensions;
using MS.Pix.MED.Infrastructure.Middlewares;
using MS.Pix.MED.Infrastructure;
using MS.Pix.MED.Application.Extensions;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MS.Pix.MED.Application.AssemblyReference).Assembly));

// Configurar Infrastructure
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddInfrastructure(connectionString);

// Configurar autenticação e autorização
builder.Services.UseIdentityProvider(builder.Configuration);
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

// Configurar integração com API JDPI externa
builder.Services.UseJdpiApiIntegration(builder.Configuration);

// Configurar CORS para API JDPI
builder.Services.AddJdpiCors(builder.Configuration);

// Configurar serviços de autenticação
builder.Services.AddAuthenticationServices();



// Configurar aplicação - handlers MediatR
builder.Services.UseTipoInfracao();
builder.Services.UseTransacao();
builder.Services.UseRetornoJdpi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Configurar middleware personalizado
app.UseMiddleware<RequestContextMiddleware>();

// Configurar CORS para API JDPI
app.UseCors("JdpiApiPolicy");

// Configurar tratamento de exceções
app.UseExceptionHandler("/Error");

app.Run();
