using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Work.Models;

namespace Work.Controllers
{
    public class HomeController : Controller
    {
        public ApplicationContext db;
        public IWebHostEnvironment _appEnvironment;
        public HomeController(ApplicationContext context, IWebHostEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var FIO = from m in db.Users
                      select m;
            if (!string.IsNullOrEmpty(searchString))
            {
                FIO = FIO.Where(s => s.FIO.Contains(searchString));
            }

            return View(FIO);
        }
        [HttpPost]
        public async Task<IActionResult> Сreate(IFormFile uploadedFile, User user)
        {
            db.Users.Add(user);
            if (uploadedFile != null)
            {

                string path = "/Files/" + uploadedFile.FileName;   // путь к папке Files
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))      // сохраняем файл в папку Files в каталоге wwwroot
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                user.PATH = path;
                user.Imagename = uploadedFile.FileName;
                db.SaveChanges();
            }

            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        /* 
               public async Task<IActionResult> Dabataselist()
               {
                 return View(await db.Users.ToListAsync());
               }
       */
        public IActionResult Create()
        {
            return View();
        }
        
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(IFormFile uploadedFile,User user)
        {
        
            if (uploadedFile != null)
            {  
                string path = "/Files/" + uploadedFile.FileName;   // путь к папке Files
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))      // сохраняем файл в папку Files в каталоге wwwroot
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                user.PATH = path;
                user.Imagename = uploadedFile.FileName;
            }   
            db.Users.Update(user);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                    return View(user);
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                User user = await db.Users.FirstOrDefaultAsync(p => p.Id == id);
                if (user != null)
                {
                    db.Users.Remove(user);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}

