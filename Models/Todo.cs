using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Todo.Models;

public class Todo
{
    [Key]
    public int Id { get; set; }
    public string? Contents { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    [ForeignKey("IdentityUser")]
    public int UserId { get; set; }
    public IdentityUser User { get; set; }
}