using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Data;
using TaskManagement.API.Model.Domain;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly TaskManagementDbContext _context;
        public UsersController(TaskManagementDbContext dbContext)
        {
            _context = dbContext; 
        }
      
    }
}
