namespace Core.Domain.Entities;

public class Company
{
    public int Id { get; private set; }
    public string Cen { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public virtual ICollection<User> Users { get; private set; } = new List<User>();
    public virtual ICollection<Customer> Customers { get; private set; } = new List<Customer>();

    protected Company() { }

    private Company(string cen, string name, bool isActive, DateTime createdAt)
    {
        Cen = cen;
        Name = name;
        IsActive = isActive;
        CreatedAt = createdAt;
    }

    public static Company Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la empresa no puede estar vacío.", nameof(name));

        var cen = $"COM-{Guid.CreateVersion7()}";
        return new Company(cen, name, true, DateTime.UtcNow);
    }

    public static Company CreateWithCen(string cen, string name)
    {
        if (string.IsNullOrWhiteSpace(cen))
            throw new ArgumentException("El CEN no puede estar vacío.", nameof(cen));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre de la empresa no puede estar vacío.", nameof(name));

        return new Company(cen, name, true, DateTime.UtcNow);
    }
}