namespace TaskManagement.API.Data.Seeder
{
    public static class IdentitySeederExtension
    {
        public static async Task SeedIdentityAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var seeder = scope.ServiceProvider
                .GetRequiredService<IdentitySeeder>();

            await seeder.SeedAsync();
        }
    }
}
