using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using pitpm_pr1.Models;
using pitpm_pr1.Controllers;

namespace pitpm_pr1.Controllers
{
    public class User
    {

        public string Lastname;
        public string Name;
        public string Birthday;
        public string Phone;
        public string Password;
        public User(string Lastname, string Name, string Birthday, string Phone, string Password)
        {
            this.Lastname = Lastname;
            this.Name = Name;
            this.Birthday = Birthday;
            this.Phone = Phone;
            this.Password = Password;

        }
    }
    
    public class Users
    {
        public static List<User> All_users = new List<User>();
        public Users()
        {

        }
        public static void AddUser(string Lastname, string Name, string Birthday, string Phone, string Password)
        {
           
            Convert.ToDateTime(Birthday);
            Convert.ToInt32(Phone);

           // if (Lastname != null & Name != null &  Birthday != null & Phone != null & Password != null)
           // {
           //     All_users.Add(new User(Lastname, Name, Birthday, Phone, Password)); 
           // }
           //else()

        }
    }
    //public class Close
    //{
    //    public static void Closes()
    //    {

    //    }
        
    //}
    public class HomeController : Controller

    {
        private readonly ILogger<HomeController> _logger;
        

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public int _counter = 0;
        public IActionResult Seans()
        {
            return View();
        }
        public IActionResult Registr()
        {

            return View();
        }
        public IActionResult Autorization()
        {
            return View();
        }

        public IActionResult Toyota_Mark_II()
        {
            return View();
        }
        //public class Close
        //{
        //    public IActionResult Registr()
        //    {
        //        return ViewCo
        //    }

        //}

        //public IActionResult class close(){
        //    }

        //public IActionResult Registr()
        //{
        //    return View();
        //}
        [HttpPost]


        //public IActionResult Mazda_RX_7(string Predlozhenie)
        //{

        //    return View();
        //}
        [HttpPost]
     
       

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}