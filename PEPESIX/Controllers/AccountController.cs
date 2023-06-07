using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pitpm_pr1.Models;
using ProjectLibrary;
using ProjectLibrary.Models;
using ProjectLibrary.ViewModel;
using System.Diagnostics;


namespace pitpm_pr1.Controllers
{
    public class AccountController : Controller
    {
        private readonly CinemaDbContext _db;

        public AccountController(CinemaDbContext context) => _db = context;

        #region Авторизация

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(User users)
        {

            if (string.IsNullOrEmpty(users.PhoneNumb.ToString()))
                return Json(new
                {
                    success = false,
                    alertMessage = "Пожалуйста, введите телефон!"
                });
            //else
            //{
            //    Class1.user = users;
            //    switch (users.AdminCheck)
            //    {
            //        case 0:
            //            return Redirect("/Home/IndexAdmin");
            //        case 1:
            //            return Redirect("/Home/Index");
                   
            //    }
            //}

            if (string.IsNullOrEmpty(users.Password))
                return Json(new
                {
                    success = false,
                    alertMessage = "Пожалуйста, введите пароль!"
                });

            if (await _db.Users.FirstOrDefaultAsync(x => x.PhoneNumb == users.PhoneNumb) == null)
                return Json(new
                {
                    success = false,
                    alertMessage = "Введенный Вами номер телефона не найден в системе!"
                });

            if (await _db.Users.Where(x => x.PhoneNumb == users.PhoneNumb)
                               .FirstOrDefaultAsync(x => x.Password == users.Password) == null)
                return Json(new
                {
                    success = false,
                    alertMessage = "Введенный Вами пароль не верен!"
                });

            var findUser = await _db.Users.FirstOrDefaultAsync(x => x.PhoneNumb == users.PhoneNumb &&
                                                                    x.Password == users.Password && 
                                                                    x.AdminCheck == users.AdminCheck);

            ManagerViewModel.CurrentUser = findUser;
            return Ok(new
            {
                success = true,
                alertMessage = "Успешная авторизация",
                user = findUser
            });
        }

        #endregion

        #region Регистрация

        public IActionResult Registration() => View();

        [HttpPost]
        public async Task<IActionResult> Registration(User users)
        {
            if (string.IsNullOrEmpty(users.Surname))
                return Json(new
                {
                    success = false,
                    alertMessage = "Пожалуйста, введите фамилию!"
                });

            if (string.IsNullOrEmpty(users.Name))
                return Json(new
                {
                    success = false,
                    alertMessage = "Пожалуйста, введите имя!"
                });

            if (users.DateBirthday > DateTime.Now)
                return Json(new
                {
                    success = false,
                    alertMessage = "Пожалуйста, введите корректную дату рождения!"
                });

            if (users.Password.Length < 6)
                return Json(new
                {
                    success = false,
                    alertMessage = "Пароль должен быть больше 6 символов!"
                });

            var existNumber = await _db.Users.FirstOrDefaultAsync(x => x.PhoneNumb == users.PhoneNumb);
            if (existNumber != null)
                return Json(new
                {
                    success = false,
                    alertMessage = "Данный телефон уже есть в системе!"
                });

            if (!ModelState.IsValid)
                return Json(new
                {
                    success = false,
                    alertMessage = "Ошибка при регистрации аккаунта"
                });

            _db.Users.Add(users);
            await _db.SaveChangesAsync();
            return Ok(new { success = true, alertMessage = "Регистрация прошла успешно!" });
        }

        #endregion



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

