using Microsoft.AspNetCore.Identity;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Models;

public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public ICollection<TaskAssignmentRequest>? AssignedTaskRequests { get; set; }
    public ICollection<BaseNotification>? Notifications { get; set; }

    public void SetFirstName(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            throw new DomainException("First name cannot be null or empty.");
        FirstName = firstName;
    }

    public void SetLastName(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
            throw new DomainException("Last name cannot be null or empty.");
        LastName = lastName;
    }
}