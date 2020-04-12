using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using MyStore.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MyStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class produitController : Controller
    {
        private IConfiguration _config;
        private produitsContext _produitContext;

        public produitController(produitsContext context, IConfiguration config)
        {
            _produitContext = context;
            _config = config;

        }


        //Get api/produits

        [HttpGet]
        public ActionResult<IEnumerable<Produits>> Get()
        {
            Console.WriteLine(_produitContext.Produits.ToList());
            return  _produitContext.Produits.ToList();
        }

        // GET: api/TodoItems/5
        [HttpGet("/produit/{id}")]
        public async Task<Object> GetProductAsync(int Id)
        {
       
            Produits obj = null;
            Object result = null; string message = "";
            try
            {
                using (_produitContext)
                {
                    obj = await _produitContext.Produits.FirstOrDefaultAsync(x => x.Id == Id);
                   
                }
            }
            catch (Exception ex)
            {
              
                message =ex.ToString() ;
                result = new
                {
                    message
                };
                return result;
            }

            if (obj == null)
            {
               
                return NotFound();
            }
            else
            {
                return obj;
            }
            
        }


        [HttpPost("/addProduct/")]
        public async Task<Object> addProduct(Produits todoItem)
        {
            Object result = null; string message = "";
            if (todoItem == null)
            {
                return BadRequest();
            }
            using (_produitContext)
            {
                using (var _ctxTransaction = _produitContext.Database.BeginTransaction())
                {
                    try
                    {
                        _produitContext.Produits.Add(todoItem);
                        await _produitContext.SaveChangesAsync();
                        _ctxTransaction.Commit();
                        message = "Produit ajoutcé !";
                    }
                    catch (Exception e)
                    {
                        _ctxTransaction.Rollback();
                        e.ToString();
                        message = e.ToString();
                    }

                    result = new
                    {
                        message
                    };
                }
            }
            return result;
        }

        [HttpGet("/AllCommands/")]
        public ActionResult<IEnumerable<Commands>> GetAllCommands()
        {
            Console.WriteLine(_produitContext.Commands.ToList());
            return _produitContext.Commands.ToList();
        }

        [HttpPost("/command/")]
        public async Task<Object> Commander(Commands[] Liste)
        {
            Object result = null; string message = "";
            if (Liste == null)
            {
                return BadRequest();
            }
            using (_produitContext)
            {
                using (var _ctxTransaction = _produitContext.Database.BeginTransaction())
                {
                    try
                    {
                        
                        foreach (Commands a in Liste) {
                           
                            _produitContext.Commands.Add(a);
                            await _produitContext.SaveChangesAsync();
                            
                        }
                        _ctxTransaction.Commit(); 
                        message = "commande bien reçue";
                    }
                    catch (Exception e)
                    {
                        _ctxTransaction.Rollback();
                        e.ToString();
                        message = e.Message;
                    }

                    result = new
                    {
                        message
                    };
                }
            }
            return result;
        }


        [HttpDelete("/produit/DeleteProduit/{id}")]
        public async Task<object> DeleteByID(int id)
        {
            object result = null; string message = "";
            using (_produitContext)
            {
                using (var _ctxTransaction = _produitContext.Database.BeginTransaction())
                {
                    try
                    {
                        var idToRemove =await _produitContext.Produits.SingleOrDefaultAsync(x => x.Id == id);
                        
                        if (idToRemove != null)
                        {
                           
                            _produitContext.Produits.Remove(idToRemove);
                            await _produitContext.SaveChangesAsync();
                            
                       }
                        message = "Produit supprimé de la base de données !";
                        _ctxTransaction.Commit();
                        
                    }
                    catch (Exception e)
                    {
                        _ctxTransaction.Rollback();
                        e.ToString();
                        message =e.ToString();
                    }

                    result = new
                    {
                        message
                    };
                }
            }
            return result;
        }
    



    // GET: api/TodoItems/5
    // POST: api/TodoItems

    [HttpPost("/register")]
        public async Task<Object> RegisterUSer(Users todoItem)
        {
            Object result = null; string message = "";
            if (todoItem == null)
            {
                return BadRequest();
            }
            using (_produitContext)
            {
                using (var _ctxTransaction = _produitContext.Database.BeginTransaction())
                {
                    try
                    {
                        _produitContext.Users.Add(todoItem);
                        await _produitContext.SaveChangesAsync();
                        _ctxTransaction.Commit();
                        message = "registration succeeded";
                    }
                    catch (Exception e)
                    {
                        _ctxTransaction.Rollback();
                        e.ToString();
                        message = "registration failed !";
                    }

                    result = new
                    {
                        message
                    };
                }
            }
            return result;
        }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("/login")]
        public IActionResult PostTodoItem(Users todoItem)
        {
            IActionResult response = Unauthorized();
            var user = AuthenticateUser(todoItem);

            if (user != null)
            {
                var tokenString = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenString });
            }

            return response;

        }
        private Users AuthenticateUser(Users login)
        {
            Users user = null;

            //if(_produitContext.Users.Any(x => (x.Email == login.Email)&&  (x.Password.Equals( login.Password)))) {
             
            
              List<Users> users= _produitContext.Users.ToList();
                for(int i = 0; i < users.Count(); i++)
                {
                    if (users[i].Email==login.Email && (users[i].Password==login.Password) ){
                        
                    user = new Users { Id= users[i].Id ,Prenom = users[i].Prenom, Admin = users[i].Admin };
                    return user;
                    }
                }
           
            return user;
        }


        private string GenerateJSONWebToken(Users userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
          
            var claims = new[] {
                new Claim("id", $"{userInfo.Id}"),
                new Claim("Prenom", $"{userInfo.Prenom}"),
        new Claim("admin", userInfo.Admin ?? "user"),

        
       
    };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
               claims,
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }

    

}