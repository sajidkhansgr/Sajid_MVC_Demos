using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UniversityProject.Models;
using UniversityProject.Dal;
using PagedList;

namespace UniversityProject.Controllers
{ 
    public class StudentController : Controller
    {
        private SchoolContext db = new SchoolContext();

        //
        // GET: /Student/

        public ViewResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "Name desc" : "Name Asc";
            ViewBag.LNameSortParam = String.IsNullOrEmpty(sortOrder) ? "LName desc" : "LName Asc";
            ViewBag.DateSortParm = sortOrder == "Date" ? "Date desc" : "Date";
            if (Request.HttpMethod == "GET")
            {
                searchString = currentFilter;
            }
            else
            {
                page = 1;
            }
            ViewBag.CurrentFilter = searchString; 

            var students = from s in db.Students
                           select s;
            

            switch (sortOrder)
            {
                case "Name desc":              
                    students = students.OrderByDescending(s=>s.FirstMidName);
                    ViewBag.NameSortParm = "Name Asc";
                    break;
                case "Name Asc":
                    students = students.OrderBy(s => s.FirstMidName);
                    ViewBag.NameSortParm = "Name desc";
                    break;
                case "LName desc":
                    students = students.OrderByDescending(s => s.LastName);
                    ViewBag.LNameSortParam = "LName Asc";
                    break;
                case "LName Asc":
                    students = students.OrderBy(s => s.LastName);
                    ViewBag.LNameSortParam = "LName desc";
                    break;
                case "Date":
                    students = students.OrderBy(s => s.EnrollmentDate);
                    break;
                case "Date desc":
                    students = students.OrderByDescending(s => s.EnrollmentDate);
                    break;
                default:
                    students = students.OrderByDescending(s => s.FirstMidName);
                    break;
            }
            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(s => s.LastName.ToUpper().Contains(searchString.ToUpper())
                                       || s.FirstMidName.ToUpper().Contains(searchString.ToUpper()));
            }
            int pageSize = 5;
            int pageNumber = (page ?? 1); 

            
           // return View(students.ToList()); 
            return View(students.ToPagedList(pageNumber, pageSize));

        }
        /// <summary>
        /// return value calling from autocomplete textbox
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>

        public JsonResult AutocompleteSuggestions(string term)
        {
            var suggestions = from s in db.Students
                              select s.FirstMidName ;
            var namelist = suggestions.Where(n => n.ToLower().StartsWith(term.ToLower()));
           // return namelist.ToList();
            return Json(namelist, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Student/Details/5

        public ViewResult Details(int id)
        {
            Student student = db.Students.Find(id);
            return View(student);
        }

        //
        // GET: /Student/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Student/Create

        [HttpPost]
        public ActionResult Create(Student student)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Students.Add(student);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch(DataException)
            {
                ModelState.AddModelError("","Eror while saving");
            }

            return View(student);
        }
        
        //
        // GET: /Student/Edit/5
 
        public ActionResult Edit(int id)
        {
            Student student = db.Students.Find(id);
            return View(student);
        }

        //
        // POST: /Student/Edit/5

        [HttpPost]
        public ActionResult Edit(Student student)
        {
            if (ModelState.IsValid)
            {
                db.Entry(student).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(student);
        }

        //
        // GET: /Student/Delete/5

        public ActionResult Delete(int id, bool? saveChangesError)
        {
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
            }
            return View(db.Students.Find(id));
        }

        //
        // POST: /Student/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                //Student student = db.Students.Find(id);
                //db.Students.Remove(student);
                //db.SaveChanges();
                ////Student studentToDelete = new Student() { StudentID = id };
                ////db.Entry(studentToDelete).State = EntityState.Deleted;
                Student student = new Student() { StudentID = id };
                db.Entry(student).State = EntityState.Deleted;
                db.SaveChanges();
            }
            catch (DataException)
            {
                //Log the error (add a variable name after DataException) 
                return RedirectToAction("Delete",
                    new System.Web.Routing.RouteValueDictionary {  
                { "id", id },  
                { "saveChangesError", true } });
            }
            return RedirectToAction("Index"); 

        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}