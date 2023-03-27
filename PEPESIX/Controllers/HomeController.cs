using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using pitpm_pr1.Models;
using pitpm_pr1.Controllers;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Numerics;

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
        

        public HomeController(ILogger<HomeController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }
        public IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        private List<User> GetAllUsers()
        {
            using(IDbConnection db = Connection)
            {
                var result = db.Query<User>("Select * from Users").ToList();//получение листа
                return result;
            }
        }
        public class User
        {
            public int user_id { get; set; }
            public string surname { get; set; }
            public string name { get; set; }
            public long phone_numb { get; set; }
            public Nullable<System.DateTime> date_birthday { get; set; }
            public string password { get; set; }
            public bool admin_check { get; set; }

        }

        public IActionResult Index()
        {
            var items = GetAllUsers();
            return View(items);
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