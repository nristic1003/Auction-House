using System.ComponentModel;
using System.Security.AccessControl;
using System.IO;
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
using Microsoft.EntityFrameworkCore;
using System.Threading;

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
        public async Task<IActionResult> myAuctions(int pageNumber = 1)
        {
          User user = await this.userManager.GetUserAsync(base.User);
             var data = context.auction.Where(a =>a.owner.Id == user.Id);  
         
            return View(await PaginatedList< Auction>.CreateAsync(data, pageNumber,4));
        }
 

       [Authorize(Roles="Administrator")]
       public async  Task<IActionResult> AdminView(int pageNumber =1)
       { 
           var data = this.context.auction;
           return View(await PaginatedList< Auction>.CreateAsync(data, pageNumber,10));
       }
       
       [Authorize(Roles="Administrator")]
       public async  Task<IActionResult> allUsers(int pageNumber =1)
       { 
           var data = this.context.user;
           return View(await PaginatedList< User>.CreateAsync(data, pageNumber,10));
       }

        [Authorize(Roles="Administrator")]
        public async  Task<IActionResult> deleteFromDatabase(string? id)
           {
                User user = this.context.user.Where(u=>u.Id ==id).FirstOrDefault();
                if(user!=null)
                {
                    this.context.user.Remove(user);
                    await this.context.SaveChangesAsync();
                }
                   return RedirectToAction(nameof(UserController.allUsers), "User");
           }
        
        
        [Authorize(Roles="Administrator")]
        public async  Task<IActionResult> banUser(string? id)
           {
                User user = this.context.user.Where(u=>u.Id ==id).FirstOrDefault();
                if(user!=null)
                {
                   user.state = 'B';
                     this.context.user.Update(user);
                    await this.context.SaveChangesAsync();
                }
                   return RedirectToAction(nameof(UserController.allUsers), "User");
           }

        [Authorize(Roles="Administrator")]
        public async  Task<IActionResult> confirmAuction(int? id)
           {
                Auction auction = this.context.auction.Where(u=>u.Id ==id).FirstOrDefault();
                if(auction!=null)
                {
                   auction.state ="READY";
                  
                     this.context.auction.Update(auction);
                    await this.context.SaveChangesAsync();
                }
                   return RedirectToAction(nameof(UserController.AdminView), "User");
           }

           [Authorize(Roles="Administrator")]
        public async  Task<IActionResult> dismissAuction(int? id)
           {
                Auction auction = this.context.auction.Where(u=>u.Id ==id).FirstOrDefault();
                if(auction!=null)
                {
                   auction.state ="DELETED";
                     this.context.auction.Update(auction);
                    await this.context.SaveChangesAsync();
                }
                   return RedirectToAction(nameof(UserController.AdminView), "User");
           }

      
        public    IActionResult editAuction(int? id)
        {
            Auction auction =   this.context.auction.Find(id);
            if(auction!=null)
            {
             AuctionModel aucModel = new AuctionModel(){
                 Id = auction.Id,
                name = auction.name,
                description = auction.description,
                startPrice = auction.startPrice,
                createDate = auction.createDate,
                openDate = auction.openDate,
                closeDate = auction.closeDate,
            };
            return View(aucModel);
            }else{
               return RedirectToAction(nameof (Index) , "HomeController/Index");
            }
         
        }
        [HttpPost]
        [Authorize]
         public   async Task<IActionResult> editAuction(AuctionModel model)
         {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            Auction auction = this.context.auction.Find(model.Id);
            User user = await this.userManager.GetUserAsync(base.User);
           using (BinaryReader reader = new BinaryReader(model.file.OpenReadStream()))
           {
              
                   auction.name = model.name;
                   auction.description = model.description;
                  auction.image = reader.ReadBytes(Convert.ToInt32(reader.BaseStream.Length));
                   auction.createDate = DateTime.UtcNow;
                   auction.startPrice = model.startPrice;
                   auction.currentPrice = model.startPrice;
                   auction.openDate = model.openDate;
                   auction.closeDate = model.closeDate;
                   auction.state = "DRAFT";
                   auction.owner = user;

           }
             this.context.auction.Update(auction);
            await this.context.SaveChangesAsync();
            return RedirectToAction(nameof(UserController.UserInfo), "User");
            
         }
         [Authorize]
        public async  Task<IActionResult> deleteAuction(int? id)
           {
                Auction auction = this.context.auction.Find(id);
                if(auction!=null)
                {
                    this.context.auction.Remove(auction);
                    await this.context.SaveChangesAsync();
                }
                   return RedirectToAction(nameof(UserController.myAuctions), "User");
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

        [Authorize]
        public IActionResult NewAuction()
        {
            AuctionModel au = new AuctionModel();
            return View(au);
        }

       [HttpPost]
       [ValidateAntiForgeryToken]
        [Authorize]
          public async Task<IActionResult> NewAuction(AuctionModel model)
        {
           if(!ModelState.IsValid)
           {
               return View(model);
           }
             User user = await this.userManager.GetUserAsync(base.User);
           using (BinaryReader reader = new BinaryReader(model.file.OpenReadStream()))
           {
               Auction aucMod = new Auction()
               {
                   name = model.name,
                   description = model.description,
                   image = reader.ReadBytes(Convert.ToInt32(reader.BaseStream.Length)),
                   createDate = DateTime.UtcNow,
                   startPrice = model.startPrice,
                   currentPrice = model.startPrice,
                   openDate = model.openDate,
                   closeDate = model.closeDate,
                   state = "DRAFT",
                   owner = user

               };
               await this.context.auction.AddAsync(aucMod);
               await this.context.SaveChangesAsync();
           }
           return RedirectToAction( nameof(UserController.NewAuction));
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

            User u = this.context.user.Where(u=> u.UserName == model.username).FirstOrDefault();
            if(u.state=='B')
            {
                   ModelState.AddModelError("", "This account has been banned");
                return View(model);
            }



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