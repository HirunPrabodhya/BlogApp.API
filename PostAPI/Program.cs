using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Service.Repository;
using Service.Repository.IRepository;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PostDBContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("postDbConnection"))
);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {

        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateIssuerSigningKey = true,

        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"))
    };
});
builder.Services.AddScoped<IUser, UserService>();
builder.Services.AddScoped<IPost, PostService>();
builder.Services.AddScoped<IUserType, UserTypeService>();
builder.Services.AddScoped<ICategory, CategoryService>();
builder.Services.AddScoped<IEmail, EmailService>();
builder.Services.AddScoped<IToken, TokenService>();
builder.Services.AddScoped<ISubscriber, SubscriberService>();


builder.Services.AddCors(setup =>
{
    setup.AddPolicy("default", option =>
    {
        option.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("default");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
