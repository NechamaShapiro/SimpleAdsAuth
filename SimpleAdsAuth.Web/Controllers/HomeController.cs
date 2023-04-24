using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using SimpleAdsAuth.Data;
using SimpleAdsAuth.Web.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SimpleAdsAuth.Web.Controllers
{
    public class HomeController : Controller
    {
        private string _connectionString = @"Data Source=.\sqlexpress;Initial Catalog=SimpleAdsAuth;Integrated Security=true;";
        
        //
        public IActionResult Index()
        {
            var repo = new UserRepository(_connectionString);
            var vm = new HomePageViewModel();
            vm.Ads = repo.GetAllAds();
            return View(vm);
        }
        public IActionResult MyAccount()
        {
            var repo = new UserRepository(_connectionString);
            var vm = new MyAccountPageViewModel();
            var currentUserEmail = User.Identity.Name;
            var user = repo.GetByEmail(currentUserEmail);
            vm.MyUserInfo = user;
            vm.MyAds = repo.GetByUserId(user.Id);
            return View(vm);
        }
        [Authorize]
        public IActionResult NewAd()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewAd(string phoneNumber, string details)
        {
            var repo = new UserRepository(_connectionString);
            var currentUserEmail = User.Identity.Name;
            var user = repo.GetByEmail(currentUserEmail);
            repo.AddNewAd(user.Id, phoneNumber, details);
            return Redirect("/home/index");
        }
        public IActionResult DeleteAd(int id)
        {
            var repo = new UserRepository(_connectionString);
            repo.DeleteAd(id);
            return Redirect("/home/index");
        }
    }
}