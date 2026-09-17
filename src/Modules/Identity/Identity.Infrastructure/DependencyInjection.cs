using Identity.Infrastructure.Persistence.AutoMig;

namespace Identity.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityModule(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<IdentityModuleDbContext>((sp, options)=>
            {
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));
                var interceptors = sp.GetServices<ISaveChangesInterceptor>();
                options.AddInterceptors(interceptors);
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

            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));
            services.AddScoped<DatabaseInitializer>();
            services.AddScoped<IIdentityDbContext,IdentityModuleDbContext>();
            services.AddScoped<ITokenProvider, TokenProvider>();
            return services;
        }
    }
}