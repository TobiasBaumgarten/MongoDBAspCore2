using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Mongo.Data;
using Mongo.Helpers;
using Mongo.Models;
using Mongo.Provider;

namespace Mongo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddTransient<INoteRepository, NoteRepository>();
            services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString
                    = Configuration.GetSection("MongoConnection:ConnectionString").Value;
                options.Database
                    = Configuration.GetSection("MongoConnection:Database").Value;
            });

            // Identity
            services.AddIdentity<ApplicationUser, IdentityRole>(opt => {
                opt.Password.RequiredLength = 6;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireUppercase = false;
            })
                .AddDefaultTokenProviders();
            services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            services.AddTransient<IUserRepository, UserRepository>();

            // Authentication - Jwt Bearer
            var keySecret = Configuration["JwtSigningKey"];
            var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keySecret));
            services.AddTransient(_ => new JwtSignInHandler(symmetricKey));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = symmetricKey,
                    ValidIssuer = JwtSignInHandler.TokenIssuer,
                    ValidAudience = JwtSignInHandler.TokenAudience
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // ! Use Authentication
            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
