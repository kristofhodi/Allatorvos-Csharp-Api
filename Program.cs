using AllatorvosApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TreatmentDbContext>(options =>
    options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Allatorvos;Trusted_Connection=True;MultipleActiveResultSets=true"));

var app = builder.Build();

app.MapGet("/rendelesek", (TreatmentDbContext DbContext) => DbContext.Treatments.ToList());

app.MapGet("/rendeles/{id:int}", (TreatmentDbContext DbContext, int id) => DbContext.Treatments.Single(t => t.Id == id));

app.MapPost("/ujrendeles", (TreatmentDbContext DbContext, Treatment treatment) =>
{
    DbContext.Treatments.Add(treatment);
    DbContext.SaveChanges();
});

app.MapPut("/modositas/{id:int}", (TreatmentDbContext DbContext, int id, Treatment treatment) =>
{
    DbContext.Treatments.Where(v => v.Id == id).ExecuteUpdate(setters => setters
                .SetProperty(v => v.Name, treatment.Name)
                .SetProperty(v => v.Type, treatment.Type)
                .SetProperty(v => v.StartDate, treatment.StartDate)
                .SetProperty(v => v.EndDate, treatment.EndDate)
                .SetProperty(v => v.Price, treatment.Price)
    );
});

app.MapDelete("/torles/{id:int}", (TreatmentDbContext DbContext, int id) =>
{
    DbContext.Treatments.Where(v => v.Id == id).ExecuteDelete();
});

app.MapGet("/bevetel/{allattipus}", (TreatmentDbContext DbContext, AnimalType allattipus) =>
{
    var allatok = DbContext.Treatments.Where(v => v.Type.Equals(allattipus)).ToList();
    var bevetel = 0;
    foreach (var allat in allatok)
    {
        bevetel += allat.Price;
    }

    return bevetel;
});

app.MapGet("/gyakorisagok", (TreatmentDbContext DbContext) =>
{
    var gyakorisagok = DbContext.Treatments
        .GroupBy(v => v.Type)
        .Select(g => new
        {
            Type = g.Key,
            Count = g.Count()
        })
        .ToList();

    return gyakorisagok;
});

app.Run();