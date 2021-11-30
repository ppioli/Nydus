using System;

namespace Nydus.IntegrationTestsServer;

public class User
{
    public Guid RandomSortParameter { get; set; } = Guid.NewGuid();
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public Animal FavoriteAnimal { get; set; }
    public DateTime RegistrationDate { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public string Gender { get; set; }
}