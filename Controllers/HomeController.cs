using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WeddingPlanner.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;

namespace WeddingPlanner.Controllers
{
    public class HomeController : Controller
    {
        private MyContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, MyContext context)
        {
            _context = context;
            _logger = logger;
            
        }

        [HttpGet("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register (User newUser)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(u => u.Email == newUser.Email))
                {
                ModelState.AddModelError("Email", "Email is already in use");
                return View("Index");
                }
            
            else
            {
                PasswordHasher<User>Hasher = new PasswordHasher<User>();
                newUser.Password = Hasher.HashPassword(newUser, newUser.Password);
                _context.Add(newUser);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId", newUser.UserId);
                return RedirectToAction("Dashboard");
            }
            }
            else
            {
                return View ("Index");
            }
        }

        [HttpPost("login")]
        public IActionResult Login(LogUser loguser)
        {
            if(ModelState.IsValid)
            {
                User userinDb = _context.Users.FirstOrDefault(u => u.Email == loguser.lemail);
                if(userinDb == null)
                {
                    ModelState.AddModelError("lemail", "Invalid Login Attempt");
                    return View("Index");
                }
                PasswordHasher<LogUser> Hasher = new PasswordHasher<LogUser>();
                PasswordVerificationResult result = Hasher.VerifyHashedPassword(loguser, userinDb.Password, loguser.lpassword);
                if(result == 0)
                {
                    ModelState.AddModelError("lemail", "Invalid Login Attempt");
                    return View("Index");
                }
                HttpContext.Session.SetInt32("UserId", userinDb.UserId);
                // To retrieve a string from session we use ".GetString"
                // string LocalVariable = HttpContext.Session.GetString("User");
                return RedirectToAction("Dashboard");
                // why is it redirecttoaction and not just view?
            }
            else{
                return View("Index");
                }
            }

        [HttpGet("Dashboard")]
        public IActionResult Dashboard()
        {
            int? LoginSession = HttpContext.Session.GetInt32("UserId");
            // the session function needs a string to return an integer
            if(LoginSession == null)
            {
                return RedirectToAction("Index");
                // redirect to action takes user to a method
            }
            else{
            ViewBag.LoggedInUser = _context.Users.FirstOrDefault(d => d.UserId == (int)HttpContext.Session.GetInt32("UserId")); 
            ViewBag.AllWeddings = _context.Weddings.Include(a => a.GuestList).ThenInclude(b => b.User).ToList();
            return View();
                
            }
            
        }

        [HttpPost("addToWeddingList")]
        public IActionResult addWedding(Reservation newWedding)
        {
            // newWedding.UserId = HttpContext.Session.GetInt32("UserId");
            _context.Add(newWedding);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        // [HttpPost("removeFromWeddingList")]
        // public IActionResult minusWedding(Reservation lessWedding)
        // {
        //     _context.Remove(lessWedding);
        //     _context.SaveChanges();
        //     return RedirectToAction("Dashboard");
        // }

        [HttpGet("newWedding")]
        public IActionResult NewWedding()
        {
            int? LoginSession = HttpContext.Session.GetInt32("UserId");
            // the session function needs a string to return an integer
            if(LoginSession == null)
            {
                return RedirectToAction("Index");
                // redirect to action takes user to a method
            }
            else
            {
                return View();
            }
        }

        [HttpPost("createWedding")]
        public IActionResult CreateWedding(Wedding oneWedding)
        {
            if(ModelState.IsValid)
            {
                oneWedding.UserId = (int)HttpContext.Session.GetInt32("UserId");
                _context.Add(oneWedding);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View("NewWedding");
            }
        }

        [HttpGet("oneWedding/{weddingId}")]
        public IActionResult OneWedding (int weddingId)
        {
            int? LoginSession = HttpContext.Session.GetInt32("UserId");
            // the session function needs a string to return an integer
            if(LoginSession == null)
            {
                return RedirectToAction("loginPage");
                // redirect to action takes user to a method
            }
            else{
            Wedding getWedding = _context.Weddings.Include(a => a.GuestList).ThenInclude(u => u.User).FirstOrDefault(g => g.WeddingId == weddingId);
            return View(getWedding);                
            }
            
        }

        [HttpGet("delete/{weddingId}")]
        public IActionResult Delete(int weddingId)
        {
            int? LoginSession = HttpContext.Session.GetInt32("UserId");
            // the session function needs a string to return an integer
            if(LoginSession == null)
            {
                return RedirectToAction("Index");
                // redirect to action takes user to a method
            }
            Wedding oneToDelete = _context.Weddings.SingleOrDefault(w => w.WeddingId == weddingId);
            Console.WriteLine(oneToDelete);
            _context.Weddings.Remove(oneToDelete);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("/RSVP/{weddingId}/{userId}")]
        public IActionResult RSVP(int weddingId, int userId)
        {
            int? LoginSession = HttpContext.Session.GetInt32("UserId");
            // the session function needs a string to return an integer
            if(LoginSession == null)
            {
                return RedirectToAction("Index");
                // redirect to action takes user to a method
            }
            Reservation newReservation = new Reservation();
            newReservation.WeddingId = weddingId;
            newReservation.UserId = userId;
            _context.Add(newReservation);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("/unRSVP/{weddingId}/{userId}")]
        public IActionResult unRSVP(int weddingId, int userId)
        {
            int? LoginSession = HttpContext.Session.GetInt32("UserId");
            // the session function needs a string to return an integer
            if(LoginSession == null)
            {
                return RedirectToAction("Index");
                // redirect to action takes user to a method
            }
            Reservation removeReservation = _context.Reservations.FirstOrDefault(d => d.UserId == userId && d.WeddingId == weddingId);
            _context.Reservations.Remove(removeReservation);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    
    }
}



