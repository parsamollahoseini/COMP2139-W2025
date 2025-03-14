using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace COMP2139_ICE.Models;

public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }

    [Required]
    public required string Title { get; set; }
    
    [Required]
    public required string Description { get; set; }
    
    //foreign key

    public int ProjectId { get; set; }
    
    //navigation property
    //this property allow easy access to the related project entity from the project task entity
    public Project? Project { get; set; }
}