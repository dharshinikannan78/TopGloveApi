using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using topGlove.Data;
using topGlove.Model;

namespace topGlove.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly UserDbContext dataContext;

        public UserController(UserDbContext userData)
        {
            dataContext = userData;
        }


        [HttpPost("AddUser")]

        public IActionResult AddUser( [FromBody] UserDetails userData )
        {
           
            if(userData == null)
            {
                return BadRequest();
            }
            else
            {
                dataContext.Login.Add(userData);
                dataContext.SaveChanges();
                return Ok(userData);
            }
        }


      [HttpPost("GetLogin")]
      public IActionResult GetLogin([FromBody] UserDetails data)
        {
            var user = dataContext.Login.Where(x => x.UserName == data.UserName && x.Password == data.Password).FirstOrDefault();
            if (user.Role == "Admin")
            {            
                var userList = dataContext.Login.AsQueryable();
                return Ok(userList);
            }
            if(user.Role == "operator")
            {
                return Ok(user);
            }
            return BadRequest();
        }

        [HttpPut("UpdateLogin")]

        public IActionResult UpdateLogin([FromBody] UserDetails obj)
        {
            if (obj == null)
            {
                return BadRequest();
            }
            var user = dataContext.Login.AsNoTracking().FirstOrDefault(x=>x.UserId == obj.UserId);
            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                dataContext.Entry(obj).State = EntityState.Modified;
                dataContext.SaveChanges();
                return Ok(obj);
            }
        }

        [HttpDelete("DeletUser")]
        public IActionResult DeletUser(int id )
        {
            var deleteUser = dataContext.Login.Find(id);
            if (deleteUser == null)
            {
                return NotFound();
            }
            else
            {
                dataContext.Login.Remove(deleteUser);
                dataContext.SaveChanges();
                return Ok();
            }
        }         




   
    }                   
    
}
