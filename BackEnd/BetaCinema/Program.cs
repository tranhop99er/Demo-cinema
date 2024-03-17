using BetaCinema.Services.Implements;
using BetaCinema.Services.Interface;
using Microsoft.OpenApi.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Cors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).AddNewtonsoftJson(
    options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//add when error:  Unable to resolve service for type
builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddSwaggerGen(x =>
{
    // Thêm xác thực JWT vào SwaggerGen
    x.AddSecurityDefinition("Auth", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "Mau: Bearer {Token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
    });
});

//ketnoi back vs front

//});//*---------------------------------------------********--------------------------------------------------
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:3000")
               .AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});



// Thêm xác thực JWT
var secretKey = builder.Configuration.GetSection("AppSettings:SecretKey").Value;
Console.WriteLine(secretKey);
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:SecretKey").Value))
            };  
        });

builder.Services.AddScoped<IUserServices, UserServices>();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

//enable cors
app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
    
app.UseHttpsRedirection();
//Xac thuc truoc
app.UseAuthentication();
//Phan quyen sau
app.UseAuthorization();

app.MapControllers();

app.Run();
