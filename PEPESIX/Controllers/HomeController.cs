using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using pitpm_pr1.Models;
using System.Data;
using ProjectLibrary.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectLibrary.ViewModel;
using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Microsoft.Extensions.Hosting;
using FluentAssertions.Common;

namespace pitpm_pr1.Controllers
{
    public class HomeController : Controller
    {
        #region
        //public class Seat
        //{
        //    public int Number { get; set; }
        //    public string Status { get; set; }
        //}
        private readonly CinemaDbContext _db;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(IConfiguration configuration, CinemaDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _configuration = configuration;
            _db = context;
            _webHostEnvironment = webHostEnvironment;
        }
        


        #region  Выбор фильма
        public async Task<IActionResult> Index()
        {

            ViewBag.Films = await _db.Films.OrderBy(x => x.Name).ToListAsync();
            return View();
        }

        public async Task<IActionResult> IndexAdmin() 
        {
            ViewBag.Films = await _db.Films.OrderBy(x => x.Name).ToListAsync();
            return View();
        }
        
        public async Task<JsonResult> GetFilmById(int? id)
        {

            var result = id switch
            {
                null => $"Фильма с идентификатором {id} не найдено!",
                _ => JsonConvert.SerializeObject(await _db.Films.FindAsync(id))
            };

            return Json(result);
        }
        
        public async Task<IActionResult> CurrentFilm(int? id)
        {

            ViewBag.Sessions = await _db.Sessions.Where(x => x.FilmId == id).ToListAsync();

            return View();
        }
       

        #endregion



        #region Сеансы 

        public async Task<IActionResult> Sessions()
        {
            ViewBag.Sessions = await _db.Sessions.OrderBy(x => x.SessionId).ToListAsync();
            return View();
        }
        public async Task<IActionResult> Sessions(int? id)
        {

            if (id == null)
                return Json(new
                {
                    success = false,
                    message = "Данный сеанс не найден!"
                });
            var session = await _db.Sessions.FindAsync(id);
            if (session == null) return NotFound();
            var model = new SessionsViewModel
            {
                SessionId = session.SessionId,
                FilmId = session.FilmId,
                Price = session.Price,
                DateTime = session.DateTime,
                NumberZal = session.NumberZal,
                Film = session.Film,
                Tickets = session.Tickets

            };

            return View(model);
        }
        public async Task<JsonResult> GetSessionById(int? id)
        {
            var result = id switch
            {
                null => $"Сеанса с идентификатором {id} не найдено!",
                _ => JsonConvert.SerializeObject(await _db.Sessions.FindAsync(id)),
            };
            return Json(result);
        }
        #endregion

        public IActionResult Billets() { return View(); }

        #region Зал
        public IActionResult Zal()
        {
            return View();
        }


        [HttpPost]
        public ActionResult SaveSeatSelections(TicketsModel tickets ,int RowNumber, int SeatNumber, int UserId, int SessionId)
        {
            // Создание экземпляра модели Seat и заполнение значений
            var ticket = new Ticket
            {
                RowNumber = RowNumber,
                SeatNumber = SeatNumber,
                UserId = UserId,
                SessionId = SessionId

            };

            using (var db = new CinemaDbContext())
            {
                // Добавление экземпляра модели в контекст базы данных
                db.Tickets.Add(ticket);

                // Сохранение изменений в базе данных
                db.SaveChanges();
            }
            return Json(new { success = true, message = "Данные успешно сохранены." });

            // Возвращение результата или перенаправление на другую страницу
            //return RedirectToAction("Index");
        }
        //public async Task<IActionResult> ()
        //{
        //    return View();  
        //}
        #endregion

        #region Админ 

        public async Task<IActionResult> IndexUnAutoriz()
        {
            ViewBag.Films = await _db.Films.OrderBy(x => x.Name).ToListAsync();
            return View();
        }

        
        public async Task<IActionResult> CurrentFilmUnAutoriz(int? id)
        {
            ViewBag.Sessions = await _db.Sessions.Where(x => x.FilmId == id).ToListAsync();

            return View();
        }

        #endregion

     

        public IActionResult Privacy()
        {
            return View();
        }
        public int _counter = 0;
       

        public IActionResult Toyota_Mark_II()
        {
            return View();
        }

        #endregion

        #region Функции администратора

        #region Фильмы

        [HttpPost]
        public async Task<IActionResult> AddFilm(
            string filmName,
            string filmDescription,
            string ageRestriction,
            IFormFile filmImage
            )
        {
            if (string.IsNullOrEmpty(filmName))
                return Json(new
                {
                    success = false,
                    message = "Пожалуйста, введите название фильма!"
                });
            if (string.IsNullOrEmpty(filmDescription))
                return Json(new
                {
                    success = false,
                    message = "Пожалуйста, введите описание фильма!"
                });
            if (string.IsNullOrEmpty(ageRestriction))
                return Json(new
                {
                    success = false,
                    message = "Пожалуйста, введите возрастное ограничение для фильма!"
                });

            if (await _db.Films.AnyAsync(x => x.Name == filmName))
                return Json(new
                {
                    success = false,
                    message = "Данный фильм уже существует!"
                });

            if (filmImage is not { Length: > 0 })
                return Json(new
                {
                    succes = false,
                    message = "Пожалуйста, выберите изображение!"
                });

            if (await _db.Films.AnyAsync(x => x.Image == filmImage.FileName))
                return Json(new
                {
                    success = false,
                    message = "Данное изображение уже имеется в системе!"
                });

            var fileName = Path.GetFileName(filmImage.FileName);
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", fileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await filmImage.CopyToAsync(fileStream);

            var film = new Film
            {
                Name = filmName,
                AgeRestriction = ageRestriction,
                Description = filmDescription,
                Image = fileName
            };

            await _db.Films.AddAsync(film);
            await _db.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = "Успешное добавление фильма!"
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditFilm(
            int filmId,
            string filmName,
            string filmDescription,
            string ageRestriction,
            IFormFile filmImage,
            string filmImageRepeat
            )
        {
            if (string.IsNullOrEmpty(filmName))
                return Json(new
                {
                    success = false,
                    message = "Пожалуйста, введите название фильма!"
                });
            if (string.IsNullOrEmpty(filmDescription))
                return Json(new
                {
                    success = false,
                    message = "Пожалуйста, введите описание фильма!"
                });
            if (string.IsNullOrEmpty(ageRestriction))
                return Json(new
                {
                    success = false,
                    message = "Пожалуйста, введите возрастное ограничение для фильма!"
                });

            var currentFilm = await _db.Films.FirstOrDefaultAsync(x => x.FilmId == Convert.ToInt32(filmId));
            if (await _db.Films.AnyAsync(s => s.Name == filmName && s.FilmId != currentFilm!.FilmId))
                return Json(new
                {
                    success = false,
                    swalMessage = "Данная услуга уже существует!"
                });

            if (currentFilm == null)
                return Json(new
                {
                    success = false,
                    message = $"Фильм с номером {filmId} не найден!"
                });

            if (filmImage is { Length: > 0 })
            {
                if (await _db.Films.AnyAsync(x => x.Image == filmImage.FileName && x.FilmId != currentFilm!.FilmId))
                    return Json(new
                    {
                        success = false,
                        message = "Данное изображение уже имеется в системе!"
                    });
                var fileName = Path.GetFileName(filmImage.FileName);
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Images", fileName);

                await using var fileStream = new FileStream(filePath, FileMode.Create);
                await filmImage.CopyToAsync(fileStream);

                currentFilm!.Image = fileName;
            }
            else currentFilm!.Image = filmImageRepeat;
            currentFilm!.Name = filmName;
            currentFilm!.Description = filmDescription;
            currentFilm!.AgeRestriction = ageRestriction;

            await _db.SaveChangesAsync();

            return Json(new
            {
                success = true,
                message = $"Успешное обновление услуги под номером {filmId}"
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddSession(string filmId, int numberZal, int sessionPrice, DateTime sessionDate)
        {
            var currentId = int.Parse(filmId);
            var currentFilm = await _db.Films.FirstOrDefaultAsync(x => x.FilmId == currentId);
            var session = new Session
            {
                FilmId = int.Parse(filmId),
                NumberZal = numberZal,
                Price = sessionPrice,
                DateTime = sessionDate
            };
            await _db.Sessions.AddAsync(session);
            await _db.SaveChangesAsync();
            return Json(new
            {
                success = true,
                message = $"Успешное добавление сеанса к фильму под названием {currentFilm!.Name}!"
            });
        }

        [HttpPost]
        public async Task<IActionResult> DeleteData(string? filmId)
        {
            var currentId = Convert.ToInt32(filmId);
            var film = await _db.Films.FindAsync(currentId);
            _db.Films.Remove(film!);
            await _db.SaveChangesAsync();
            return Json(new 
            { 
                success = true, 
                message = $"Данные под номером {filmId} успешно удалены!", 
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetFilm(int id) => 
            Json(await _db.Films.FirstOrDefaultAsync(x => x.FilmId == id));
        
        #endregion

        #endregion

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}