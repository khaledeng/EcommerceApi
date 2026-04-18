
using Day1.data;
using Day1.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

namespace Day1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddAutoMapper(typeof(MapperProfile));
            builder.Services.AddDataProtection();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
            });



            var JWT = builder.Configuration.GetSection("JWT");
            var token = JWT["Key"];
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateLifetime = true,
                        //ClockSkew = TimeSpan.Zero,
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(token))

                    };
                });
            builder.Services.AddSingleton(TimeProvider.System);
            //1
            //builder.Services.AddIdentityCore<IdentityUser>()
            //    .AddEntityFrameworkStores<AppDbContext>()
            //    .AddApiEndpoints();

            builder.Services.AddDbContext<AppDbContext>(
                option => option.UseSqlServer(
                    builder.Configuration.
            // للـ global
                    GetConnectionString("MyContString")));
            // للـ local
            //GetConnectionString("LocalContString")));

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler =
                        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            builder.Services.AddCors(
                Option => Option.AddPolicy(
                         "AllowAll",
                           builder =>
                           {
                               builder
                                     .AllowAnyOrigin()
                                     .AllowAnyMethod()
                                     .AllowAnyHeader();

                           }
                    ));

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.Migrate();
            }

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();
            //app.MapIdentityApi<IdentityUser>();

            app.MapControllers();

            app.Run();
        }
    }
}
