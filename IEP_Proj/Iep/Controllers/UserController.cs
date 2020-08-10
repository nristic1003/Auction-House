using System.Linq;
using System.Threading.Tasks;
using Iep.Models.Database;
using Iep.Models.View;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using Iep.Models;
using System.Collections.Generic;

namespace Iep.Controllers{

    public class UserController : Controller{

        private AuctionContext context;
        private UserManager<User> userManager;
        private IMapper mapper;
        private SignInManager<User> signInManager;
        


        public UserController(AuctionContext context, UserManager<User> userManager, SignInManager<User> signInManager, IMapper mapper)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
            this.signInManager = signInManager;
        }

        

        public IActionResult Register(){
            return View();
        }
        
        public IActionResult Tokens(){
            return View();
        }

        [Authorize]
        public async Task<IActionResult> UserInfo()
        {
            User user = await this.userManager.GetUserAsync(base.User);
            return View(user);
        }
       
        [Authorize]
        public  IActionResult ChangePassword()
        {
            ChangePasswordView cp = new ChangePasswordView();
            return View(cp);
        }
        [HttpPost]
        [Authorize]
        public  async Task<IActionResult> ChangePassword(ChangePasswordView model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
              User user = await this.userManager.GetUserAsync(base.User);
              PasswordVerificationResult result  = this.userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.oldPassword);
                if(result == PasswordVerificationResult.Failed)
                {
                    ModelState.AddModelError("", "Old password incorrect!");
                    return View(model);
                }
              if(model.password!=model.confirmPassword)
              {
                     ModelState.AddModelError("", "Passwords are not the same");
                        return View(model);
              }else
              {
               
                   await this.userManager.ChangePasswordAsync(user,model.oldPassword, model.confirmPassword); // proveriti
                   await this.userManager.UpdateAsync(user);
                   return RedirectToAction(nameof(UserController.UserInfo), "User");
              }
            
        }
        public async Task<IActionResult> Pagination(int pageNumber =1)
        {
             User user = await this.userManager.GetUserAsync(base.User);
             var data = context.transaction.Where(a =>a.idUser == user.Id );  
         
            return View(await PaginatedList< Transaction>.CreateAsync(data, pageNumber,1));
        }
        [Authorize]
        public async Task<IActionResult> EditUser()
        {
                User user = await this.userManager.GetUserAsync(base.User);
                EditUser eu = new EditUser()
                {
                    firstName = user.firstName,
                      lastName = user.lastName,
                        gender = user.gender,
                          email = user.Email

                };
                return View(eu);
        }
         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> buyTokens(string bagName)
         {
             User loggedInUser = await this.userManager.GetUserAsync(base.User);
             int idBag = 0;
             int tokenAmount = 0;
             switch(bagName)
             {
                 case "silver":
                 {
                     idBag=1;
                     tokenAmount = 10;
                 }
                 break;
                 case "gold":
                {
                     idBag=2;
                     tokenAmount = 15;
                 }
                 break;
                 case "platinum":
                {
                     idBag=3;
                     tokenAmount = 20;
                 }
                 break;
             }

             Transaction transaction = new Transaction()
             {
                 idToken = idBag,
                 idUser = loggedInUser.Id,
                 date = DateTime.Now,
             };

             await this.context.transaction.AddAsync(transaction);
             await this.context.SaveChangesAsync();

             loggedInUser.tokens += tokenAmount;
             await this.userManager.UpdateAsync(loggedInUser);

             return Json(true);
         }    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register (RegisterModel model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            /*
                S obzirom da koristimo Identity biblioteku, ona vec u sebi ima mehanizam za dodavanje novog korsnika. Postoji klasa UserManager koja to radi,.
            */
            User user = this.mapper.Map<User>(model);
            IdentityResult result = await this.userManager.CreateAsync(user, model.password);
            if(!result.Succeeded)
            {
                foreach(IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                
                return View(model);
            }

            result = await this.userManager.AddToRoleAsync(user, Roles.user.Name);
            if(!result.Succeeded)
            {
                foreach(IdentityError error in result.Errors)
                    ModelState.AddModelError("", error.Description);
                
                return View(model);
            }

            return RedirectToAction(nameof(UserController.LogIn), "User");

            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditInfo(EditUser model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
             User user = await this.userManager.GetUserAsync(base.User);
              PasswordVerificationResult result  = this.userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.password);
              if(result!=PasswordVerificationResult.Failed)
              {
                 user.firstName = model.firstName;
                user.lastName = model.lastName;
                user.gender = model.gender;
                 if(model.email!=user.Email)
                {
                    bool exists = this.context.Users.Where(user=>user.Email == model.email).Any();
                    if(exists)
                    {
                        ModelState.AddModelError("", "Email already exists");
                        return View(model);
                    }
                    user.Email = model.email;
                    user.NormalizedEmail = model.email.ToUpper();
                    
                }
              await this.userManager.UpdateAsync(user);
                await this.signInManager.RefreshSignInAsync(user);
                return RedirectToAction(nameof(UserController.UserInfo), "User");
             
              }
          return View(model);
           

        }
        public IActionResult LogIn(string returnUrl)
        {
            LogInModel model = new LogInModel()
            {
                returnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogIn(LogInModel model)
        {
            if(!ModelState.IsValid)
                return View(model);

            var result = await this.signInManager.PasswordSignInAsync(model.username, model.password, false, false);
            
            

            if(!result.Succeeded)
            {
                ModelState.AddModelError("", "Username or password not valid!");
                return View(model);
            }

            if(model.returnUrl != null)
                return Redirect(model.returnUrl);
            else
                return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public IActionResult isEmailUnique(string email) 
        {
            /*
                Ova meto da se poziva tako sto se salje zahtev serveru tj. GET zahetv. Sto znaci da se parametri prosledjuju korz adresu, tako sto
                se navodi ImePolja jednako VrednostPOlja. Sto znaci da metoda koja vrsi proveru mora da ima parametar koji se zove isto kao i polje
                u Modelu da bi mogo de se izvrsi povezivanje GET parametara sa tim.
            */

            bool exists = this.context.Users.Where(user=>user.Email == email).Any();

            if(exists)
                return Json("Email already taken!");
            else
                return Json(true);

        }

        public IActionResult isUsernameUnique(string username) 
        {
            /*
                Ova meto da se poziva tako sto se salje zahtev serveru tj. GET zahetv. Sto znaci da se parametri prosledjuju korz adresu, tako sto
                se navodi ImePolja jednako VrednostPOlja. Sto znaci da metoda koja vrsi proveru mora da ima parametar koji se zove isto kao i polje
                u Modelu da bi mogo de se izvrsi povezivanje GET parametara sa tim.
            */

            bool exists = this.context.Users.Where(user=>user.UserName == username).Any();

            if(exists)
                return Json("Username already taken!");
            else
                return Json(true);

        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOut()
        {
            await this.signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), "Home");
      
        }
        
      
      
    }
}