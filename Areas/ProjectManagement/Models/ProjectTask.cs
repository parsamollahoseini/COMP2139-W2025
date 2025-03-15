using System.ComponentModel.DataAnnotations;

namespace COMP2139_ICE.Areas.ProjectManagement.Models;

public class ProjectTask
{
    [Key]
    public int ProjectTaskId { get; set; }
    
    [Required]
    [Display(Name = "Project Task Title")]
    [StringLength(100, ErrorMessage = "Project Task Title cannot be longer than 100 characters.")]
    public required string Name { get; set; }
    public required string Title { get; set; }
    
    [Required]
    [Display(Name = "Project Task Description")]
    [DataType(DataType.MultilineText)]
    [StringLength(500, ErrorMessage = "Project Task ddescription cannot be longer than 500 characters.")]
    public required string Description { get; set; }
    
    
    //Foreign Key
    [Display(Name = "Parent Project")]
    public int ProjectId { get; set; }
    
    
    //Navigation property
    //This allows for easy access to the related Project entity
    public Project? Project { get; set; }

    public List<ProjectTask> Tasks { get; set; } = new();

}