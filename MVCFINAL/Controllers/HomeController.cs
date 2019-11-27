using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVCFINAL.Models;


namespace MVCFINAL.Controllers
{
    public class HomeController : Controller
    {
        MVCFINALEntities db = new MVCFINALEntities();

        public ActionResult Create()
        {
            user obj = new user();
            return View("Create", obj);
        }

        [HttpPost]
        public ActionResult Create(user obj, HttpPostedFileBase UserImage)
        {
            string filename = System.IO.Path.GetFileName(UserImage.FileName);
            string filepath = "~/image/" + filename;
            obj.UserImage = filepath;
            UserImage.SaveAs(HttpContext.Server.MapPath(filepath));

            db.users.Add(obj);
            db.SaveChanges();
            var datashow = db.users.ToList();
            return View("Display", datashow);
        }


        public ActionResult Display()
        {
            var datashow = db.users.ToList();
            return View("Display", datashow);
        }

        //Display to the textbox
        public ActionResult Update(int personid)
        {
            var personrec = (from item in db.users
                             where item.ID == personid
                             select item).FirstOrDefault();
            return View("Update", personrec);
        }

        [HttpPost]
        public ActionResult Update(user obj, HttpPostedFileBase UserImage)
        {

            var model = db.users.Find(obj.ID);
            string oldfilepath = model.UserImage;
            if (UserImage != null && UserImage.ContentLength > 0)
            {
                string filename = System.IO.Path.GetFileName(UserImage.FileName);
                string filepath = System.IO.Path.Combine(Server.MapPath("~/image/"), filename);
                UserImage.SaveAs(filepath);
                model.UserImage = "~/image/" + UserImage.FileName;
                string fullpath = Request.MapPath("~" + oldfilepath);
            }

            var personrec = (from item in db.users
                             where item.ID == obj.ID
                             select item).FirstOrDefault();

            personrec.Firstname = obj.Firstname;
            personrec.Address = obj.Address;
            personrec.Hobbies = obj.Hobbies;
            personrec.Gender = obj.Gender;
            personrec.Mobile = obj.Mobile;
            personrec.Designation = obj.Designation;
            personrec.Email = obj.Email;
            personrec.Dob = obj.Dob;
            model.UserImage = model.UserImage;

            db.SaveChanges();

            var datashow = db.users.ToList();
            return View("Display", datashow);
        }

        public ActionResult Delete(int personid)
        {
            var personrec = (from item in db.users
                             where item.ID == personid
                             select item).FirstOrDefault();
            db.users.Remove(personrec);
            db.SaveChanges();
            var datashow = db.users.ToList();
            return View("Display", datashow);
        }
        //public ActionResult DelSelect(string[] empid)
        //{
        //    int[] getid = null;
        //    if (empid != null)
        //    {
        //        getid = new int[empid.Length];
        //        int j = 0;
        //        foreach (string i in empid)
        //        {
        //            int.TryParse(i, out getid[j++]);
        //        }

        //        List<user> getempids = new List<user>();

        //        getempids = db.users.Where(x => getid.Contains(x.ID)).ToList();
        //        foreach (var s in getempids)
        //        {
        //            db.users.Remove(s);
        //        }
        //        db.SaveChanges();
        //    }
        //    var datashow = db.users.ToList();
        //    return View("Display", datashow);
        //}

        public ActionResult DelSelect(FormCollection formCollection)
        {
            string[] ids = formCollection["ID"].Split(new char[] { ',' });
            foreach (string id in ids)
            {
                var employee = db.users.Find(int.Parse(id));
                db.users.Remove(employee);
                db.SaveChanges();
            }
            var datashow = db.users.ToList();
            return View("Display", datashow);
        }


    }
}