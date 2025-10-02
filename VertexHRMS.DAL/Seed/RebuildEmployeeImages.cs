namespace VertexHRMS.DAL.Seed
{
    public static class RebuildEmployeeImages
    {
        public static async Task RebuildEmployeeImagesAsync(VertexHRMSDbContext db, string contentRoot)
        {
            var filesFolder = Path.Combine(contentRoot, "wwwroot", "Files");
            if (!Directory.Exists(filesFolder))
                throw new DirectoryNotFoundException($"Originals folder not found: {filesFolder}");

            var employees = await db.Employees.ToListAsync();
            int c = 0;
            foreach (var emp in employees)
            {
                c++;
                var originalPath = Path.Combine(filesFolder, $"employee{c}.jpg"); 
                if (!File.Exists(originalPath))
                {
                    continue;
                }

                var newName = $"{Guid.NewGuid():N}_employee{emp.EmployeeId}{Path.GetExtension(originalPath)}";
                var destPath = Path.Combine(filesFolder, newName);
                File.Copy(originalPath, destPath, overwrite: true);

                var dbPath = $"/Files/{newName}";

                db.Entry(emp).Property<string>("ImagePath").CurrentValue = dbPath;
            }

            await db.SaveChangesAsync();
        }

    }
}
