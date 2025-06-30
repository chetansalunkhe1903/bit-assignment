using API.Middleware;
using Microsoft.AspNetCore.Builder;

namespace API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app, bool isDevelopment)
        {
            if (isDevelopment)
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<SecurityHeadersMiddleware>();

            app.UseCors("AllowAll");

            app.UseAuthentication();
            app.UseAuthorization();

            return app;
        }
    }
}
