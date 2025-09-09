using MS.Pix.MED.Integration.Jdpi;
using MS.Pix.MED.Application.JdpiLog;
using MS.Pix.MED.Application.Movimentacao;
using MS.Pix.MED.Application.TipoInfracao;
using MS.Pix.MED.Application.TipoTransaction;
using MS.Pix.MED.Infrastructure.Extensions;
using MS.Pix.MED.Infrastructure.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMemoryCache();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar autenticação e autorização
builder.UseIdentityProvider();
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();

// Configurar integração com API JDPI externa
builder.Services.UseJdpiApiIntegration(builder.Configuration);

// Configurar CORS para API JDPI
builder.Services.AddJdpiCors();

// Configurar serviços de autenticação
builder.Services.AddAuthenticationServices();

// Configurar aplicação - handlers MediatR
builder.Services.UseJdpiLog();
builder.Services.UseMovimentacao();
builder.Services.UseTipoInfracao();
builder.Services.UseTipoTransaction();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configurar CORS
app.UseCors("JdpiApiPolicy");

// Configurar autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

// Configurar controllers
app.MapControllers();

// Configurar middlewares
app.UseMiddleware<RequestContextMiddleware>();
app.UseExceptionHandler("/error");

app.Run();
