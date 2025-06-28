using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Models;

public class BaseEntity
{
    public BaseEntity()
    {
        CreatedAt = DateTime.Now;
        UpdatedAt = DateTime.Now;
        Id = 0;
        Deleted = false;
    }

    public int Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
    public bool Deleted { get; private set; }

    public void Delete()
    {
        if (!Deleted) Deleted = true;
        else throw new DomainException("Can not undelete an entity");
    }

    public void UpdateUpdatedAt()
    {
        UpdatedAt = DateTime.Now;
    }
}