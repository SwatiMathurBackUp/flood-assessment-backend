using FloodAssessment.API.Models;

namespace FloodAssessment.API.Data
{
    public static class DataSeeder
    {
        public static async Task SeedAsync(AppDbContext context)
        {
            // Only seed if no users exist
            if (context.Users.Any()) return;

            // Create Users
            var manager = new User
            {
                Name = "Admin Manager",
                PinHash = BCrypt.Net.BCrypt.HashPassword("1234"),
                Role = "Manager",
                CreatedAt = DateTime.UtcNow
            };

            var john = new User
            {
                Name = "John Smith",
                PinHash = BCrypt.Net.BCrypt.HashPassword("1111"),
                Role = "Assessor",
                CreatedAt = DateTime.UtcNow
            };

            var sarah = new User
            {
                Name = "Sarah Johnson",
                PinHash = BCrypt.Net.BCrypt.HashPassword("2222"),
                Role = "Assessor",
                CreatedAt = DateTime.UtcNow
            };

            var mike = new User
            {
                Name = "Mike Davis",
                PinHash = BCrypt.Net.BCrypt.HashPassword("3333"),
                Role = "Assessor",
                CreatedAt = DateTime.UtcNow
            };

            context.Users.AddRange(manager, john, sarah, mike);
            await context.SaveChangesAsync();

            // Create Farm Assignments
            var farms = new List<FarmAssignment>
            {
                new FarmAssignment
                {
                    FarmName = "Johnson Poultry Farm",
                    OwnerName = "Robert Johnson",
                    Address = "123 Rural Rd, Madison County, NC",
                    Latitude = 35.9582,
                    Longitude = -82.7541,
                    EstimatedChickens = 12000,
                    Status = FarmStatus.Pending,
                    AssignedToUserId = john.Id,
                    CreatedAt = DateTime.UtcNow
                },
                new FarmAssignment
                {
                    FarmName = "Smith Chicken Farm",
                    OwnerName = "Patricia Smith",
                    Address = "456 Creek Rd, Madison County, NC",
                    Latitude = 35.9123,
                    Longitude = -82.7234,
                    EstimatedChickens = 8500,
                    Status = FarmStatus.Pending,
                    AssignedToUserId = john.Id,
                    CreatedAt = DateTime.UtcNow
                },
                new FarmAssignment
                {
                    FarmName = "Brown Poultry Farm",
                    OwnerName = "James Brown",
                    Address = "789 Hill Rd, Madison County, NC",
                    Latitude = 35.9876,
                    Longitude = -82.8123,
                    EstimatedChickens = 15000,
                    Status = FarmStatus.Pending,
                    AssignedToUserId = sarah.Id,
                    CreatedAt = DateTime.UtcNow
                },
                new FarmAssignment
                {
                    FarmName = "Davis Chicken Farm",
                    OwnerName = "Linda Davis",
                    Address = "321 Valley Rd, Madison County, NC",
                    Latitude = 35.9345,
                    Longitude = -82.7890,
                    EstimatedChickens = 9200,
                    Status = FarmStatus.Pending,
                    AssignedToUserId = sarah.Id,
                    CreatedAt = DateTime.UtcNow
                },
                new FarmAssignment
                {
                    FarmName = "Wilson Farm",
                    OwnerName = "Thomas Wilson",
                    Address = "654 Mountain Rd, Madison County, NC",
                    Latitude = 35.9654,
                    Longitude = -82.8456,
                    EstimatedChickens = 11000,
                    Status = FarmStatus.Pending,
                    AssignedToUserId = mike.Id,
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.FarmAssignments.AddRange(farms);
            await context.SaveChangesAsync();
        }
    }
}