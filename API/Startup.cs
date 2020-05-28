using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Threading.Tasks;
using API.Mapping;
using API.Validators;
using Autofac;
using BLL;
using BLL.Entities;
using BLL.TokenConfiguration;
using DAL;
using DAL.Context;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            AutomapperConfig.Configure();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddMvc()
                .AddFluentValidation(fv =>
                {
                    fv.RegisterValidatorsFromAssemblyContaining<RegisterValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<BookValidator>();
                    fv.RegisterValidatorsFromAssemblyContaining<LoginValidator>();
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddDbContext<ApplicationContext>(options =>
                {
                    options.UseSqlServer(Configuration["ConnectionStrings:NakkisApp"]);
                });

            services.AddIdentity<User, Role>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 6;
                })
                .AddEntityFrameworkStores<ApplicationContext>()
                .AddDefaultTokenProviders();

            

            services.AddScoped(x => x.GetRequiredService<ApplicationContext>().Orders);
            services.AddScoped(x => x.GetRequiredService<ApplicationContext>().OrderItems);
            services.AddScoped(x => x.GetRequiredService<ApplicationContext>().Carts);
            services.AddScoped(x => x.GetRequiredService<ApplicationContext>().CartItems);
            services.AddScoped(x => x.GetRequiredService<ApplicationContext>().Categories);
            services.AddScoped(x => x.GetRequiredService<ApplicationContext>().Products);
            services.AddScoped(x => x.GetRequiredService<ApplicationContext>().Departments);
            services.AddScoped(x => x.GetRequiredService<ApplicationContext>().Variants);
            services.AddScoped(x => x.GetRequiredService<ApplicationContext>().Availabilities);

            services.AddCors(options =>
            {
                options.AddPolicy("Policy",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = TokenConfig.ISSUER,

                        ValidateAudience = true,
                        ValidAudience = TokenConfig.AUDIENCE,

                        ValidateLifetime = true,

                        IssuerSigningKey = TokenConfig.GetSymmetricSecurityKey(),
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddControllers();
            //services.AddControllersWithViews();

            //services.AddSpaStaticFiles(configuration =>
            //{
            //    configuration.RootPath = "ClientApp/build";
            //});
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule<BLLModules>();
            builder.RegisterModule<DALModules>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 204)
                {
                    ctx.Response.ContentLength = 0;
                }
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            //app.UseAuthentication();
            //app.UseHttpsRedirection();
            //app.UseCors("Policy");

            app.UseRouting();
            //app.UseStatusCodePagesWithRedirects("/");
            //app.UseDefaultFiles();

            app.UseStaticFiles();

            //app.UseSpaStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("Policy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapFallbackToFile("index.html");
                // endpoints.MapFallbackToController("Index", "Home");
            });
            //app.UseMvc(route =>
            //{
            //    route.MapRoute("default", "controller/action/{id}");
            //});

            //app.MapWhen(context => context.Response.StatusCode == 404 &&
            //                   !Path.HasExtension(context.Request.Path.Value),
            //        branch => {
            //            branch.Use((context, next) => {
            //                context.Request.Path = new PathString("index.html");
            //                Console.WriteLine("Path changed to:" + context.Request.Path.Value);
            //                return next();
            //            });

            //            branch.UseStaticFiles();
            //        });

            RoleCreation(serviceProvider).Wait();
        }

        private async Task RoleCreation(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();
            List<string> roles = new List<string>();
            roles.Add("Admin");
            roles.Add("User");

            foreach (var role in roles)
            {
                var exist = await roleManager.RoleExistsAsync(role);

                if (exist)
                {
                    continue;
                }
                else
                {
                    var roleToAdd = new Role {Name = role};
                    await roleManager.CreateAsync(roleToAdd);
                }
            }
        }
    }
}
