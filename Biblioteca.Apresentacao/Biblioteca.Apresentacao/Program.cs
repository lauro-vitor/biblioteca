using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;
using Biblioteca.Apresentacao.Filter;
using Biblioteca.Repositorio;
using Biblioteca.Repositorio.EntityFramework;

[ExcludeFromCodeCoverage]
public class Program
{
	public static void Main(string[] args)
	{

		var builder = WebApplication.CreateBuilder(args);

		var policyName = "MyPolicy";

		builder.Services.AddCors(options =>
		{
			options.AddPolicy(policyName, policy =>
			{
				policy.AllowAnyOrigin()
					.AllowAnyMethod()
					.AllowAnyHeader();
			});
		});

		// Add services to the container.
		builder.Services
			.AddControllersWithViews()
			.AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

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


		ConfigurarBancoDeDados(builder.Services);
		InjetarRepositorio(builder.Services);



		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if (!app.Environment.IsDevelopment())
		{
			app.UseExceptionHandler("/Home/Error");
			// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
			app.UseHsts();
		}

		app.UseCors(policyName);

		app.UseHttpsRedirection();

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

	private static void ConfigurarBancoDeDados(IServiceCollection services)
	{
		string caminhoBanco = "./biblioteca-dev.db";

		if (!File.Exists(caminhoBanco))
		{
			File.Create(caminhoBanco);
		}

		services.AddSqlite<BibliotecaContext>("Data Source=" + caminhoBanco);
	}

	private static void InjetarRepositorio(IServiceCollection services)
	{
		services.AddTransient<TurnoRepositorio>();
		services.AddTransient<TurmaRepositorio>();
		services.AddTransient<EditoraRepositorio>();
		services.AddTransient<AutorRepositorio>();
		services.AddTransient<GeneroRepositorio>();
		services.AddTransient<LivroRepositorio>();
		services.AddTransient<LivroAutorRepositorio>();
		services.AddTransient<LivroGeneroRepositorio>();
		services.AddTransient<ParentescoRepositorio>();
		services.AddTransient<AlunoRepositorio>();
		services.AddTransient<AlunoContatoRepositorio>();
		services.AddTransient<EmprestimoRepositorio>();
	}
}
