using System.ComponentModel.DataAnnotations;

namespace COMP2139_ICE.Models;

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
    [Required]
    
    public required string Name { get; set; }
    
    public string? Description { get; set; }
    
    
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    
    
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
    
    public string? Status { get; set; }
    
    //One-to-Many Relationship : A project can have many project tasks
    public List<ProjectTask>? ProjectTasks { get; set; } = new();
}