using System.ComponentModel.DataAnnotations;
using COMP2139_ICE.Areas.ProjectManagement.Models;

namespace COMP2139_ICE.Areas.ProjectManagement.Models;

public class Project
{
    /// <summary>
    ///  The unique identifier for a project
    /// </summary>
    public int ProjectId { get; set; }
    
    /// <summary>
    /// The name of the project
    /// [Required]: Ensures this property must have a value when the object is valdiated
    /// </summary>
    
    [Display(Name = "Project Name")]
    [Required]
    [StringLength(100, ErrorMessage = "Project name cannot be longer than 100 characters.")]
    public required string Name { get; set; }
    
    
    [Display(Name = "Project Description")]
    [DataType(DataType.MultilineText)]
    [Required]
    [StringLength(500, ErrorMessage = "Project description cannot be longer than 500 characters.")]
    public string? Description { get; set; }
    
    
    [Display(Name = "Project Start Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime StartDate { get; set; }
    
    
    [Display(Name = "Project End Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime EndDate { get; set; }
    
    [Display(Name = "Project Status")]
    public string? Status { get; set; }
    
    public List<ProjectTask>? Tasks { get; set; } = new();
}