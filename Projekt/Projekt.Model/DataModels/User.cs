using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Projekt.Model.DataModels;

public class User : IdentityUser<int>
{
    [MaxLength(100)]
    public string? FirstName { get; set; }

    [MaxLength(100)]
    public string? LastName { get; set; }

    public bool IsPremium { get; set; }

    public virtual IList<Character> Characters { get; set; } = new List<Character>();
}

