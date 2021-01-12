using System.Text;
using System.Data;
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

        [Authorize]
        public IActionResult winningAuctionDetails(int id)
        {
              Auction auction  = this.context.auction.Include( a=> a.winner).Include(a => a.owner).Where( a => a.Id == id).FirstOrDefault();

              if(auction!=null)
              {
                  IList<Bids> bids =  this.context.bids.Include( s => s.user).Where( b => b.auctionId == id).OrderByDescending(b => b.bidDate).Take(5).ToList();
                    ViewBag.userID = bids;
                   return View(auction);
              }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    
        public IActionResult Tokens(){
     
            return View();
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> bid(int id, int price, DateTime newDate, int myBid=0)
        {

           Auction auc = this.context.auction.Include(s => s.winner).Where(a => a.Id == id).FirstOrDefault(); 

        
           // Thread.Sleep(5000);
            
           if(auc!=null)
           {
               User user = await this.userManager.GetUserAsync(base.User);
           
               if(myBid==0) myBid=10;

               if(user.tokens<auc.currentPrice+myBid)
               return Json(new { success = false});

               if(auc.owner==user)
               {
                    return Json(new { success = false});
               }
               
                if(auc.winner!=user && auc.winner!=null)
                {
                    User u =  this.context.user.Find(auc.winner.Id);
                     u.tokens +=auc.currentPrice;
                     await this.userManager.UpdateAsync(u);

                 }
               auc.currentPrice += myBid;
              
         
              auc.winner = user;
              user.tokens -=auc.currentPrice;
              auc.closeDate = newDate;
             
              Bids bid = new Bids(){
                  userId = user.Id,
                  auctionId = auc.Id,
                  price = auc.currentPrice,
                  bidDate = DateTime.Now

              };

            
              this.context.bids.Add(bid);   
            await this.context.SaveChangesAsync();
            auc.BidList.Add(bid);


         bool not_saved = true;
        bool flag=true;
         while(not_saved && flag)
         {
            try{
                not_saved=false;
                this.context.auction.Update(auc);
                this.context.SaveChanges();

            }catch(DbUpdateException ex)
            {
                not_saved=true;
                flag=false;
              


               foreach (var entry in ex.Entries)
                {
                if (entry.Entity is Auction)
                {
                    var proposedValues = entry.CurrentValues;
                    var databaseValues = entry.GetDatabaseValues();

                       
                    foreach (var property in proposedValues.Properties)
                    {
                        if(property.Name=="currentPrice"){
                            var proposedValue = proposedValues[property]; // 10
                            var databaseValue = databaseValues[property]; //5050

                            flag =(int)proposedValue > (int)databaseValue;
                          

                        }
        
                        // TODO: decide which value should be written to database
                        // proposedValues[property] = <value to be saved>;
                    }

                    // Refresh original values to bypass next concurrency check
                    entry.OriginalValues.SetValues(databaseValues);
                }
              
            }
           
            }
         }
             return Json(new { success = !not_saved,  winner = auc.winner.UserName, currPrice = auc.currentPrice });
           }else{
            return Json(false);
           }
           
        }

        [Authorize]
        public async Task<IActionResult> winning(int pageNumber=1)
        {
            User user = await this.userManager.GetUserAsync(base.User);
            var data = this.context.auction.Where( s => s.winner==user);
            return View(await PaginatedList< Auction>.CreateAsync(data, pageNumber,12));
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
         
            return View(await PaginatedList< Auction>.CreateAsync(data, pageNumber,12));
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
                   var auctions =  await this.context.auction.Include(s => s.winner).Where(a => a.owner == user).ToListAsync(); 
                    foreach(var item in auctions)
                    {
                        if(item.state!="EXPIRED")
                        {
                            if(item.state == "OPEN" && item.winner!=null)
                            {
                                 User u =  this.context.user.Find(item.winner.Id);
                                 u.tokens +=item.currentPrice;
                                 await this.userManager.UpdateAsync(u);
                            }
                            item.state ="EXPIRED";
                            this.context.Update(item);
                            await this.context.SaveChangesAsync();
                        }
                    }
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
                    if(auction.winner!=null)
                    {
                        auction.winner.tokens += auction.currentPrice - auction.startPrice; 
                         await   this.userManager.UpdateAsync(auction.winner);
                    }
             
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
               
                   await this.userManager.ChangePasswordAsync(user,model.oldPassword, model.confirmPassword); 
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
                   createDate = DateTime.Now,
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
         
            return View(await PaginatedList< Transaction>.CreateAsync(data, pageNumber,12));
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
        public async Task<IActionResult> expired(int id)
        {
          Auction auc = this.context.auction.Find(id);
          if(auc!=null)
          {
              if(auc.winner!=null) auc.state ="SOLD";
              else auc.state = "EXPIRED";
               this.context.Update(auc);
               await this.context.SaveChangesAsync();
               return Json(true);
          }

          return Json(false);
            

        }

         [HttpPost]
         [ValidateAntiForgeryToken]
         public async Task<IActionResult> buyTokens(string option)
         {
             User loggedInUser = await this.userManager.GetUserAsync(base.User);
             int idBag = 0;
             int tokenAmount = 0;
             switch(option)
             {
                 case "silver":
                 {
                     idBag=1;
                     tokenAmount = 5;
                 }
                 break;
                 case "gold":
                {
                     idBag=2;
                     tokenAmount = 10;
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
            if(u!=null)
            {
                if(u.state=='B')
                {
                    ModelState.AddModelError("", "This account has been banned");
                    return View(model);
                }
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
            bool result = this.context.Users.Where(user=>user.Email == email).Any();

            if(result)
                return Json("Email is taken!");
            else
                return Json(true);

        }

        public IActionResult isUsernameUnique(string username) 
        {
            bool result = this.context.Users.Where(user=>user.UserName == username).Any();

            if(result)
                return Json("Username is taken!");
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