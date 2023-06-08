using Microsoft.AspNetCore.Mvc;
using WebApplications.DataAccess.Data;
using WebApplications.Models;

namespace WebApplication1.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db) {
            _db = db;
        } 
        public IActionResult Index()
        {
            List<Category> objectCategoryList = _db.Categories.ToList();
            return View(objectCategoryList);
        }

        public IActionResult Create() 
        { 
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("name", "The displayOrder canNot match the name");
            }
            if (obj.Name.ToLower() == "test")
            {
                ModelState.AddModelError("", "test is an invalid value");
            }
            if (ModelState.IsValid) { 
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if(id==null || id == 0)
            {
                return NotFound();
            }
            Category singleCategoryfromDb = _db.Categories.Find(id);
            //Category singleCategoryfromDb = _db.Categories.FirstOrDefault(u=>u.Id==id);
            //Category singleCategoryfromDb = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            if (singleCategoryfromDb == null) 
            {
                return NotFound();
            }
            return View(singleCategoryfromDb);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {            
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Category? singleCategoryfromDb = _db.Categories.Find(id);

            if (singleCategoryfromDb == null)
            {
                return NotFound();
            }
            return View(singleCategoryfromDb);
        }

        [HttpPost, ActionName("delete")]
        public IActionResult DeletePOST(int? id)
        {
            Category? obj = _db.Categories.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index", "Category");
        }
    }
}
