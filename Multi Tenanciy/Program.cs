var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ITenentServices, TenentServices>();
builder.Services.Configure<TenatnaSettings>(builder.Configuration.GetSection(nameof(TenatnaSettings)));
builder.Services.AddScoped<IProductServices, ProductServices>();
TenatnaSettings options = new();
builder.Configuration.GetSection(nameof(TenatnaSettings)).Bind(options);


var defaultDbProverider = options.Defaluts.DbProvider;

if (defaultDbProverider.ToLower() == "mssql")
{
    builder.Services.AddDbContext<ApplicationDbContext>(m => m.UseSqlServer());
}

foreach (var tenant in options.Tenants)
{
    var connectionString = 
        !string.IsNullOrEmpty(tenant.ConnectionString)
        ? options.Defaluts.ConnectionString :
        tenant.ConnectionString;
    using var scope = builder.Services.BuildServiceProvider().CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.SetConnectionString(connectionString);
    if(dbContext.Database.GetPendingMigrations().Any())
        dbContext.Database.Migrate();
}


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
