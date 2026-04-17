
using Day1.data;
using Day1.Mapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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
            builder.Services.AddSwaggerGen();
            // Add services to the container.
            builder.Services.AddAutoMapper(typeof(MapperProfile));
            builder.Services.AddDataProtection();

            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
               .AddEntityFrameworkStores<AppDbContext>();

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
                            Encoding.UTF8.GetBytes("YourSecretKey23asldjfklsdjflksdflsadflk"))
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
                    GetConnectionString("MyContString")));

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler =
                        System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });


           


            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddSwaggerGen(options =>
            {
                //options.AddSecurityDefinition
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "token"
                });

                //options.AddSecurityRequirement(new OpenApiSecurityRequirement
                //{
                //    {
                //        //new OpenApiSecurityScheme
                //        //{
                //        //    //Reference = new OpenApiReference
                //        //    //{
                //        //    //    Type = ReferenceType.SecurityScheme,
                //        //    //    Id = "Bearer"
                //        //    //}
                //        //},
                //        //Array.Empty<string>()
                //    }
                //});
            });

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
            //app.MapIdentityApi<IdentityUser>();

            app.MapControllers();

            app.Run();
        }
    }
}
