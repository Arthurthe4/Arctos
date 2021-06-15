using Arctos.Data;
using Arctos.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arctos.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
        }
        

        public IActionResult Index()
        {
            IEnumerable<Product> objList = _db.Product;

            // Loading Product with Category id 
            foreach(var obj in objList)
            {
                obj.Category = _db.Category.FirstOrDefault(u => u.Id == obj.CategoryId);
            };

            return View(objList);
        }

        // --------- CREATE -----------
        // GET - UPSERT

        // Dealing with Editing and Updating the Product
        public IActionResult Upsert(int? id)
        {
            Product product = new Product();
            if (id == null)
            {
                // this is for create
                return View(product);
            }
            else
            {
                product = _db.Product.Find(id);
                if (product == null)
                {
                    return NotFound();
                }
                return View(product);
            }
        }

        // POST - Upsert
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Category.Add(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(obj);
            
        }


        // --------- DELETE -----------
        // GET - Delete
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0) { return NotFound(); }

            var obj = _db.Category.Find(id);

            if (obj == null) { return NotFound(); }

            return View(obj);
        }

        // POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int ? id)
        {
            var obj = _db.Category.Find(id);
            if (obj==null)
            {
                return NotFound();
            }
                _db.Category.Remove(obj);
                _db.SaveChanges();
                return RedirectToAction("Index");
        }
    }
}

