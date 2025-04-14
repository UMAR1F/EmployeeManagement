using BaseLibrary.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServerLibrary.Data;
using ServerLibrary.Helper;
using ServerLibrary.Requirements.Contracts;
using ServerLibrary.Requirements.Implementations;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<JwtSection>(builder.Configuration.GetSection("JwtSection"));
var jwtSection = builder.Configuration.GetSection(nameof( JwtSection)).Get<JwtSection>();
//starting
var constring = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Server=(localdb)\\MSSQLLocalDB;Database=EmployeeDb;Trusted_Connection=true; Trust Server Certificate=True;";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(constring));


builder.Services.AddScoped<IUserAccount, UserAccountRepository>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSection!.Issuer,
        ValidAudience = jwtSection!.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Key))


    };


});
builder.Services.AddScoped<IUserAccount,UserAccountRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface1<Generaldepartment>, GeneralDepartmentRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface1<Department>, DepartmentRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface1<Branch>, BranchRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface1<Town>, TownRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface1<City>, CityRepository>();
builder.Services.AddScoped<IGenericRepositoryInterface1<Country>, CountryRepository>();







builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWasm",
    builder=>builder
    
       .WithOrigins("http://localhost:5194", "https://localhost:7022")
       .AllowAnyMethod()
       .AllowAnyHeader()
       .AllowCredentials());
    //.WithExposedHeaders("HeaderKey")



});



var app = builder.Build();

//Auto migration configuration
//try
//{
//    using var scope = app.Services.CreateScope();
//    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    context.Database.Migrate();
//}
//catch (Exception ex)
//{
//    Console.WriteLine(ex.ToString());
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowBlazorWasm");
app.UseAuthorization();
app.UseAuthentication();
app.MapControllers();
app.Run();


