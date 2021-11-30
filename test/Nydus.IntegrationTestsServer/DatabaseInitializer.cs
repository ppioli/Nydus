using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nydus.IntegrationTestsServer;

public class DatabaseInitializer
{
    private readonly CoreKitTestDbContext _context;

    public DatabaseInitializer(CoreKitTestDbContext context)
    {
        _context = context;
    }

    public async Task InitializeDatabase()
    {
        var users = GetInitialUsers();
        _context.Users.AddRange(users);
        await _context.SaveChangesAsync();
    }

    public static List<User> GetInitialUsers()
    {
        return new List<User>
        {
            new()
            {
                Id = 1, UserName = "Alice", Email = "alice@test.com",
                FavoriteAnimal = new Animal { Id = 1, Name = "Jaguar" },
                RegistrationDate = new DateTime(2019, 10, 14),
                LastLoginDate = new DateTime(2019, 10, 15), Gender = "F",
            },
            new()
            {
                Id = 2, UserName = "Bob", Email = "bob@example.com",
                FavoriteAnimal = new Animal { Id = 2, Name = "Impala" },
                RegistrationDate = new DateTime(2019, 10, 15), Gender = "M",
            },
            new()
            {
                Id = 3, UserName = "Caroline", Email = "caroline@example.com",
                FavoriteAnimal = new Animal { Id = 3, Name = "Hornet" },
                RegistrationDate = new DateTime(2019, 10, 16), Gender = "F",
            },
            new()
            {
                Id = 4, UserName = "Dave", Email = "dave@example.com",
                FavoriteAnimal = new Animal { Id = 4, Name = "Giraffe" },
                RegistrationDate = new DateTime(2019, 9, 17), Gender = "M",
            },
            new()
            {
                Id = 5, UserName = "Emilia", Email = "emilia@test.com",
                FavoriteAnimal = new Animal { Id = 5, Name = "Ferret" },
                RegistrationDate = new DateTime(2019, 9, 18), Gender = "F",
            },
            new()
            {
                Id = 6, UserName = "Fred", Email = "fred@test.com",
                FavoriteAnimal = new Animal { Id = 6, Name = "Eagle" },
                RegistrationDate = new DateTime(2019, 9, 19), Gender = "M",
            },
            new()
            {
                Id = 7, UserName = "Georgia", Email = "georgia@test.com",
                FavoriteAnimal = new Animal { Id = 7, Name = "Donkey" },
                RegistrationDate = new DateTime(2019, 11, 20), Gender = "F",
            },
            new()
            {
                Id = 8, UserName = "Hank", Email = "hank@example.com",
                FavoriteAnimal = new Animal { Id = 8, Name = "Cow" },
                RegistrationDate = new DateTime(2019, 11, 21), Gender = "M",
            },
            new()
            {
                Id = 9, UserName = "Iris", Email = "iris@example.com",
                FavoriteAnimal = new Animal { Id = 9, Name = "Beaver" },
                RegistrationDate = new DateTime(2019, 11, 22), Gender = "F",
            },
            new()
            {
                Id = 10, UserName = "Joe", Email = "joe@example.com",
                FavoriteAnimal = new Animal { Id = 10, Name = "Alpaca" },
                RegistrationDate = new DateTime(2019, 10, 23), Gender = "M",
            },
        };
    }
}