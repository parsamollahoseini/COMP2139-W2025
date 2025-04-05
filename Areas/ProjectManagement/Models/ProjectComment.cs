using System.ComponentModel.DataAnnotations;

namespace COMP2139_ICE.Areas.ProjectManagement.Models;

public class ProjectComment
{
    public int ProjectCommentId { get; set; }
    
    [Required]
    [StringLength(500, ErrorMessage = "Project name cannot be longer than 500 characters.")]
    public string? Content { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    private DateTime _datePosted;
    public DateTime DatePosted
    {
        get => _datePosted;
        //Postgres UTC format
        set => _datePosted = DateTime.SpecifyKind(value, DateTimeKind.Utc);
    }
    
    //Foreign key for project
    public int ProjectId { get; set; }
    
    //Navigation property
    public Project? Project { get; set; }
    
}