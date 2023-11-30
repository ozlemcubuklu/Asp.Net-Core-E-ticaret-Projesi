using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using ShopApp.Business.Abstract;
using ShopApp.WebUi.EmailServices;
using ShopApp.WebUi.Extensions;
using ShopApp.WebUi.Identity;
using ShopApp.WebUi.Models;
using System.Threading.Tasks;
using static System.Net.WebRequestMethods;

namespace ShopApp.WebUi.Controllers
{
    public class AccountController : Controller
    {
        private ICartService _cartservice;
        private IEmailSender _emailSender;
        private UserManager<User> _userManager;
        private SignInManager<User> _signInManager;
        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager,IEmailSender emailSender,ICartService cartService)
        {
            _cartservice = cartService;
            _emailSender = emailSender;
            _userManager = userManager;
            _signInManager = signInManager; 
        }
        [HttpGet]
        public IActionResult Login(string ReturnUrl=null)
        {

            return View(new LoginModel() { ReturnUrl=ReturnUrl});


        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user=await _userManager.FindByNameAsync(model.UserName);

            if (user==null)
            {
                ModelState.AddModelError("","Bu kullanıcı adı ile daha önce kayıt yapılmamıştır.");
                return View(model);
            }
            if (!await _userManager.IsEmailConfirmedAsync(user))
            {
                ModelState.AddModelError("","Lütfen mail hesabınıza gelen linki onaylayın.");
                return View(model);

            }
            var result = await _signInManager.PasswordSignInAsync(user,model.Password,false,false);
            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl??"~/");
            }
            ModelState.AddModelError("","Girilen kullanıcı adı veya parola yanlış.");
            return View(model);


        }
        [HttpGet]
        public IActionResult Register() {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid) {
               return View(model);
            }

            var user=new User() {
                FirtsName = model.FirstName, 
                LastName=model.LastName,
                Email=model.Email,
                UserName=model.UserName
            };
            var result=await _userManager.CreateAsync(user,model.Password);

            if (result.Succeeded)
            {
                var code= await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var url = Url.Action("ConfirmEmail", "Account", new { UserId =user.Id,token=code}) ;
                System.Console.WriteLine(url);
                await _emailSender.SendEmailAsync(model.Email,"Hesap onayı",$"Lütfen hesabınızı onaylamak için linke <a href='https://localhost:5001{url}'> tıklayınız</a>");
                return RedirectToAction("Login","Account");
            }

            ModelState.AddModelError("", "Bilinmeyen bir hata oluştu.Lütfen tekrar deneyin.");
            return View(model);
        }


        public async Task<IActionResult> Logout()
        {
            TempData.Put("message", new AlertMessage() { AlertType = "success", Title = "Oturum Sonlandırma", Message = "oturum kapandı." });

            await _signInManager.SignOutAsync();
            return Redirect("~/");
        }
        public async Task<IActionResult> ConfirmEmail(string UserId,string token)
        {
            if (UserId==null || token==null)
            {
                TempData.Put("message",new AlertMessage() { AlertType="danger",Title="hata",Message="Geçersiz token."});
               
                return View();
            }
            var user = await _userManager.FindByIdAsync(UserId);
            if (user!=null)
            {
                var result = await _userManager.ConfirmEmailAsync(user,token);
                if (result.Succeeded)
                {
                    _cartservice.InitializeCart(user.Id);
                    TempData.Put("message", new AlertMessage() { AlertType = "success", Title = "Hesap Onayı", Message = "Hesabınız onaylanmıştır." });
                    
                    return View();
                }
            }
            TempData.Put("message", new AlertMessage() { AlertType = "success", Title = "Hesap Onayı", Message = "Hesabınız onaylanmamıştır." });
           
            return View();
        }
        public IActionResult ForgotPassword() { return View(); }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                return View();
            }
            var user=await _userManager.FindByEmailAsync(Email);
            if (user==null) { return View(); }
            
            
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("ResetPassword", "Account", new { UserId = user.Id, token = code });
            System.Console.WriteLine(url);
            await _emailSender.SendEmailAsync(user.Email, "Reset Password", $"Lütfen şifrenizi değiştirmek için linke <a href='https://localhost:5001{url}'> tıklayınız</a>");
            return RedirectToAction("Login", "Account");
        }


        public IActionResult ResetPassword(string userId, string token) {
            if (userId==null || token==null)
            {
                return RedirectToAction("Home","Index");
            }
            var model = new ResetPasswordModel() { Token=token};
            return View(model); 
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model) {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user==null) {
                return RedirectToAction("Index","Home");
            }

            var result =await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login","Account");

            }
            return View(model); }
        public IActionResult AccessDenied()
        {
            return View();
        }




    }
}
