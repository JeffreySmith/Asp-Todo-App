using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Todo.Models;

public class Note
{
    [Key]
    public int Id { get; set; }
    [DisplayName("Note")]
    public string Contents { get; set; }
    [Required(ErrorMessage = "Please enter a startdate")]
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:yyyy/MM/dd}")]
    [DisplayName("Start Date")]
    public DateTime StartDate { get; set; }
    [DataType(DataType.Date)]
    [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:yyyy/MM/dd}")] //yyyy/MM/dd
    [DisplayName("End Date")]
    
    public DateTime EndDate { get; set; }
    public int? Priority { get; set; }
    
    public String? Tags { get; set; }
    
    public bool Share { get; set; }
    public string? Uuid { get; set; }
    
    public string Email { get; set; }
}