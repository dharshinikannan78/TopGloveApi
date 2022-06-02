using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using topGlove.Data;
using topGlove.Model;

namespace topGlove.Controllers
{
    [EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly UserDbContext dataContext;

        public UserController(UserDbContext userData)
        {
            dataContext = userData;
        }


        [HttpGet("GetUserDetails")] 

        public IActionResult GetUserDetails()
        {
            var UserDetails = dataContext.Login.AsQueryable();
            return Ok(UserDetails);
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

        [HttpGet("UserExist")]
        public IActionResult GetUser(string obj)
        {
            var userDetails = dataContext.Login.AsNoTracking().FirstOrDefault(x=>x.UserName == obj);
            if (userDetails == null)
            {
                return Ok(new
                {
                    message = "You Can Enter"
                }); ;
            }
            else
            {
                return Ok(new
                {
                    message = "already Exist"
                });
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

        [HttpGet("enc")]
        public IActionResult  Encryptdata(string password)
        {          
                string strmsg = string.Empty;
                byte[] encode = new byte[password.Length];
                encode = Encoding.UTF8.GetBytes(password);
                strmsg = Convert.ToBase64String(encode);
                return Ok(strmsg);
            
        }

        [HttpGet("dec")]
        public IActionResult  Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            UTF8Encoding encodepwd = new UTF8Encoding();
            Decoder Decode = encodepwd.GetDecoder();
            byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
            decryptpwd = new String(decoded_char);
            return Ok(decryptpwd);
        }


    }                   
    
}
