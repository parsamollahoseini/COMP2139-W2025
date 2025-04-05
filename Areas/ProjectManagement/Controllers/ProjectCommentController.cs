using COMP2139_ICE.Areas.ProjectManagement.Models;
using COMP2139_ICE.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace COMP2139_ICE.Areas.ProjectManagement.Controllers;

[Area("ProjectManagement")]
[Route("[area]/[controller]/[action]")]
public class ProjectCommentController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProjectCommentController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetComment(int projectId)
    {
        //retrieve all comments from the database associated with projectId
        var comments = await _context.ProjectComments
            .Where(c => c.ProjectId == projectId)
            .OrderByDescending(c => c.DatePosted)
            .ToListAsync();
        
        //return the comments as a JSON response
        return Json(comments);
        
    }
    
    [HttpPost]
    public async Task<IActionResult> AddComment([FromBody] ProjectComment comment)
    {
        if (ModelState.IsValid)
        {
            // current date time the comments was posted
            comment.DatePosted = DateTime.UtcNow;
            
            // add the comment to the database
            _context.ProjectComments.Add(comment);
            
            // commit the comment to the database
            await _context.SaveChangesAsync();

            return Json(new { success = true , message = "Comment added successfully" } );

        }
        
        // If the model state is invalid, collect the validation errors
        var errors = ModelState.Values
            .SelectMany(v => v.Errors)
            .Select(e => e.ErrorMessage);
        
        // return a failure JSON response with errors details
        return Json(new { success = false, message = "Invalid comment data.", errors = errors });
        
    }
    
    
}