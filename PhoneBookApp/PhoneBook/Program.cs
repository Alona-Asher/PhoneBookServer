var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton(typeof(IContactsProvider), typeof(ContactsProvider));
builder.Services.AddSingleton(typeof(IContactsRepository), typeof(ContactsRepository));
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(databaseName: "MockDatabase"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
