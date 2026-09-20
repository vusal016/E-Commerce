using Identity.Application.Common.Mapper;

namespace Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<IdentityModuleDbContext>((sp, options)=>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                var interceptor = sp.GetServices<ISaveChangesInterceptor>();
                options.AddInterceptors(interceptor);
            });

            var jwtSection = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey=new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection.Secret)),
                    ValidateIssuer = true,
                    ValidIssuer = jwtSection.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSection.Audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddIdentityCore<User>(options =>
            {
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.AllowedForNewUsers = true;
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 8;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<IdentityModuleDbContext>()
            .AddDefaultTokenProviders();
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.AddScoped<DatabaseInitializer>();
            services.AddScoped<IIdentityDbContext>(sp => sp.GetRequiredService<IdentityModuleDbContext>());
            services.AddScoped<ITokenProvider, TokenProvider>();
            services.AddMediatR(cfg=>cfg.RegisterServicesFromAssembly(typeof(UserRegisterCommandHandler).Assembly));
            services.AddAutoMapper(cfg => cfg.AddProfile<IdentityMapper>());
            return services;
        }
    }
}