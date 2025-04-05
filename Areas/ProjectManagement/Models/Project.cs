using System.ComponentModel.DataAnnotations;
using COMP2139_ICE.Areas.ProjectManagement.Models;

namespace COMP2139_ICE.Areas.ProjectManagement.Models;

public class Project
{
    /// <summary>
    /// The unique primary key for projects
    /// </summary>
    [Display(Name = "Project Id")]
    public int ProjectId { get; set; }
    
    /// <summary>
    /// The name of the project
    /// Required - Ensures the property must be provided (must have a value) 
    /// </summary>
    [Required]
    [Display(Name = "Project Name")]
    [StringLength(100, ErrorMessage = "Project Name cannot be longer than 100 characters.")]
    public required string Name { get; set; }
    
    /// <summary>
    /// This is the description of the project
    /// </summary>
    [Display(Name = "Project Description")]
    [StringLength(500, ErrorMessage = "Project Description cannot be longer than 500 characters.")]
    public string? Description { get; set; }
    
    /// <summary>
    /// This is the start date of the project
    /// </summary>
    [Display(Name = "Project Start Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime StartDate { get; set; }
    
    /// <summary>
    ///  This is the end date of the project
    /// </summary>
    [Display(Name = "Project End Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
    public DateTime EndDate { get; set; }
    
    /// <summary>
    /// The current status of the proejct (e.g., "In Progress", "Completed").
    /// - Nullable: Allows this property to have a null value if the status is unknown.
    /// </summary>
    [Display(Name = "Project Status")]
    public string? Status { get; set; }
    
    //This allows EF Core to understand that one Project has many ProjectTasks
    public List<ProjectTask>? ProjectTasks { get; set; }
    
}