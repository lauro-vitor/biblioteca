using Biblioteca.Configuracao;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Biblioteca.Apresentacao.Filter;

[ExcludeFromCodeCoverage]
public class Program
{
    public static void Main(string[] args)
    {

        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services
            .AddControllersWithViews()
            .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
         
        new Configuracao().Configurar(builder.Services);

        builder.Services.AddSwaggerGen(c =>
        {
            var info = new Microsoft.OpenApi.Models.OpenApiInfo 
            { 
                Title = "Biblioteca API", 
                Version = "v1"
            };

            c.SwaggerDoc("v1", info);
        });

        builder.Services.AddMvc(options =>
        {
            options.Filters.Add(new ErrorHandlingFilter());
        });

		var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        //Ativa o Swagger
        app.UseSwagger();

        // Ativa o Swagger UI
        app.UseSwaggerUI(opt =>
        {
            opt.RoutePrefix = "swagger";
            opt.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        });


        app.Run();
    }

}
