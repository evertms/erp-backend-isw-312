namespace Core.Domain.Entities;

public class Company
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public virtual ICollection<User> Users { get; private set; } = new List<User>();
    public virtual ICollection<Customer> Customers { get; private set; } = new List<Customer>();

    protected Company() { }

    private Company(Guid id, string name, bool isActive, DateTime createdAt)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public static Company Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la empresa no puede estar vacío.", nameof(name));

        return new Company(Guid.NewGuid(), name, true, DateTime.UtcNow);
    }
}