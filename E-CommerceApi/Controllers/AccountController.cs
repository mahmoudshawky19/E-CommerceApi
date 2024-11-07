using E_CommerceApi.DTOs;
using E_CommerceApi.Models;
using E_CommerceApi.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using static System.Net.Mime.MediaTypeNames;

namespace E_CommerceApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        [HttpPost("Register")]

        public async Task<IActionResult> Register(ApplicationUserDTOs applicationUserDT)
        {
            if (roleManager.Roles.IsNullOrEmpty())
            {
                await roleManager.CreateAsync(new(Sw.adminRole));
                 await roleManager.CreateAsync(new(Sw.CustomerRole));
            }
            if (ModelState.IsValid) {
                ApplicationUser application = new()
                {
                    UserName = applicationUserDT.name , Address = applicationUserDT.Address , Email = applicationUserDT.email
                };
                var result = await userManager.CreateAsync(application, applicationUserDT.Password);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(application, Sw.CustomerRole);

                    await signInManager.SignInAsync(application, false);

                    return Ok();
                }
                return BadRequest(result.Errors);
            }
            return BadRequest(applicationUserDT);
        }
        [HttpPost("Login")]

        public async Task <IActionResult> Login(LoginDTOs loginDTOs)
        {
            var user = await userManager.FindByNameAsync(loginDTOs.UserName);
            if(user!=null)
            {
              var result =  await userManager.CheckPasswordAsync(user , loginDTOs.Password);
                if (result)
                {
                    await signInManager.SignInAsync(user, false);
                    return Ok();
                }
                else ModelState.AddModelError(string.Empty, "there are Errors");
            }
            return NotFound();
        }
        [HttpDelete("Logout")]
        public async Task<IActionResult> Logout()
        {
          await  signInManager.SignOutAsync();
            return Ok(); 
        }
    }
}
