using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnLapTrinhWeb.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace DoAnLapTrinhWeb.Areas.AdminArea.Controllers
{
    public class AdminController : Controller
    {
        //database
        dbQLBanDanDataContext db = new dbQLBanDanDataContext();
        // GET: AdminArea/Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdminLogin()
        {
            return View();
        }

        public ActionResult Dan(int ?page) {
            int pageNumber = (page ?? 1);
            int pageSize = 7;
            return View(db.DANs.ToList().OrderBy(n => n.MaDAN).ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult AdminLogin(FormCollection form)
        {
            var tendn = form["username"];
            var matkhau = form["password"];

            if (String.IsNullOrEmpty(tendn) || String.IsNullOrEmpty(matkhau))
            {
                ViewData["LoiDangNhap"] = "Mật Khẩu Hoặc Tên Đăng Nhập Không Được Để Trống";
            }
            else
            {
                Admin admin = db.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin== matkhau);
                if (admin != null)
                {
                    ViewData["LoiDangNhap"] = "Đăng Nhập Thành Công";
                    Session["TaiKhoan"] = admin;
                    Session["Username"] = admin.UserAdmin;

                    //return RedirectToAction("Index", "Home");
                }
                else
                    ViewData["LoiDangNhap"] = "Đăng Nhập Thất Bại";
            }

            return RedirectToAction("Index", "Admin");
        }

        public ActionResult DangXuat()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult Themdanmoi() {
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenTHUONGHIEU), "MaTH", "TenTHUONGHIEU");
            ViewBag.MaLoaiDAN = new SelectList(db.LoaiDans.ToList().OrderBy(n => n.MaLoaiDan), "MaLoaiDan", "TenLoaiDan");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themdanmoi(DAN dan, HttpPostedFileBase fileupload,FormCollection form) {
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenTHUONGHIEU), "MaTH", "TenTHUONGHIEU");
            ViewBag.MaLoaiDAN = new SelectList(db.LoaiDans.ToList().OrderBy(n => n.MaLoaiDan), "MaLoaiDan", "TenLoaiDan");

            if (fileupload == null)
            {
                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else {
                if (ModelState.IsValid) {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Images/"), fileName);

                    var file_path_upload_to_database = "/Images/";
                    //upload lên folder của server
                    if (form["MaLoaiDan"].Equals("1") && form["MaTH"].Equals("1"))
                    {
                        filePath = Path.Combine(Server.MapPath("~/Images/Violin/Selmer"), fileName);
                        file_path_upload_to_database = "/Images/Violin/Selmer/" + fileName;
                    }
                    else if (form["MaLoaiDan"].Equals("1") && form["MaTH"].Equals("2"))
                    {
                        filePath = Path.Combine(Server.MapPath("~/Images/Violin/SAH"), fileName);
                    }
                    else if (form["MaLoaiDan"].Equals("1") && form["MaTH"].Equals("3"))
                    {
                        filePath = Path.Combine(Server.MapPath("~/Images/Violin/Suzuki"), fileName);
                    }
                    else if (form["MaLoaiDan"].Equals("1") && form["MaTH"].Equals("4"))
                    {
                        filePath = Path.Combine(Server.MapPath("~/Images/Violin/Lazer"), fileName);
                    }
                    else if (form["MaLoaiDan"].Equals("2") && form["MaTH"].Equals("5"))
                    {
                        filePath = Path.Combine(Server.MapPath("~/Images/Piano/Roland"), fileName);
                    }
                    else if (form["MaLoaiDan"].Equals("2") && form["MaTH"].Equals("6"))
                    {
                        filePath = Path.Combine(Server.MapPath("~/Images/Piano/Kawai"), fileName);
                    }
                    else if (form["MaLoaiDan"].Equals("2") && form["MaTH"].Equals("7"))
                    {
                        filePath = Path.Combine(Server.MapPath("~/Images/Piano/Casio"), fileName);
                    }
                    else
                    {
                        filePath = null;
                        ViewBag.ThongBao = "Kiểm Tra Mã Loại Đàn Và Mã Thương Hiệu, Các dữ liệu bạn nhập vào";
                    }

                    //check IO
                    if (System.IO.File.Exists(filePath))
                    {
                        ViewBag.ThongBao = "Đã Tồn Tại file";

                    }
                    else
                    {
                        fileupload.SaveAs(filePath);
                    }

                    dan.AnhMinhHoa = file_path_upload_to_database;
                    dan.MoTa = form["MoTa"];
                    //luu co so du lieu
                    db.DANs.InsertOnSubmit(dan);
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("Dan");
        }//end func

        public ActionResult ChiTietDan(int id) {
            DAN dan = db.DANs.SingleOrDefault(n => n.MaDAN == id);
            ViewBag.MaDan = dan.MaDAN;
            if (dan == null) {
                Response.StatusCode = 404;
                return null;
            }
            return View(dan);
        }

        [HttpGet]
        public ActionResult XoaDan(int id) {
            DAN dan = db.DANs.SingleOrDefault(x => x.MaDAN == id);
            ViewBag.MaDan = id;

            if (dan == null) {
                Response.StatusCode = 404;
                return null;
            }

            return View(dan);
        }

        [HttpPost, ActionName("XoaDan")]

        public ActionResult XacNhanXoa(int id) {
            DAN dan = db.DANs.SingleOrDefault(x => x.MaDAN == id);
            ViewBag.MaDan = dan.MaDAN;
            if (dan == null) {
                Response.StatusCode = 404;
                return null;
            }
            db.DANs.DeleteOnSubmit(dan);
            db.SubmitChanges();
            return RedirectToAction("Dan");
        }


        [HttpGet]
        public ActionResult SuaThongTin(int id)
        {
            DAN dan = db.DANs.SingleOrDefault(x => x.MaDAN == id);
            ViewBag.MaDan = id;

            if (dan == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenTHUONGHIEU), "MaTH", "TenTHUONGHIEU",dan.MaTH);
            ViewBag.MaLoaiDAN = new SelectList(db.LoaiDans.ToList().OrderBy(n => n.MaLoaiDan), "MaLoaiDan", "TenLoaiDan",dan.MaLoaiDan);
            ViewBag.MaDan = dan.MaDAN;
            return View(dan);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaThongTin(DAN dan,HttpPostedFileBase uploadFile, FormCollection form) {
     
            ViewBag.MaTH = new SelectList(db.THUONGHIEUs.ToList().OrderBy(n => n.TenTHUONGHIEU), "MaTH", "TenTHUONGHIEU");
            ViewBag.MaLoaiDAN = new SelectList(db.LoaiDans.ToList().OrderBy(n => n.MaLoaiDan), "MaLoaiDan", "TenLoaiDan");

            if (uploadFile == null)
            {

                ViewBag.ThongBao = "Vui lòng chọn ảnh bìa";
                return View(dan);
            }
            else {
                if (ModelState.IsValid) {
                    var fileName = Path.GetFileName(uploadFile.FileName);
                    var filePath = Path.Combine(Server.MapPath("~/Images/"), fileName);
                    var file_path_upload_to_database = "/Images/";
                    //upload lên folder của server
                    if (form["MaLoaiDan"].Equals("1") && form["MaTH"].Equals("1"))
                    {
                        filePath = Path.Combine(Server.MapPath("~/Images/Violin/Selmer"), fileName);
                        file_path_upload_to_database = "/Images/Violin/Selmer/" + fileName;
                    }
                    else if (form["MaLoaiDan"].Equals("1") && form["MaTH"].Equals("2"))
                    {
                        filePath = Path.Combine(Server.MapPath("~/Images/Violin/SAH"), fileName);
                    }
                    else if (form["MaLoaiDan"].Equals("1") && form["MaTH"].Equals("3"))
                    {
                        filePath = Path.Combine(Server.MapPath("~/Images/Violin/Suzuki"), fileName);
                    }
                    else if (form["MaLoaiDan"].Equals("1") && form["MaTH"].Equals("4"))
                    {
                        filePath = Path.Combine(Server.MapPath("~/Images/Violin/Lazer"), fileName);
                    }
                    else if (form["MaLoaiDan"].Equals("2") && form["MaTH"].Equals("5"))
                    {
                        filePath = Path.Combine(Server.MapPath("~/Images/Piano/Roland"), fileName);
                    }
                    else if (form["MaLoaiDan"].Equals("2") && form["MaTH"].Equals("6"))
                    {
                        filePath = Path.Combine(Server.MapPath("~/Images/Piano/Kawai"), fileName);
                    }
                    else if (form["MaLoaiDan"].Equals("2") && form["MaTH"].Equals("7"))
                    {
                        filePath = Path.Combine(Server.MapPath("~/Images/Piano/Casio"), fileName);
                    }
                    else
                    {
                        filePath = null;
                        ViewBag.ThongBao = "Kiểm Tra Mã Loại Đàn Và Mã Thương Hiệu, Các dữ liệu bạn nhập vào";
                    }

                    //check IO
                    if (System.IO.File.Exists(filePath))
                    {
                        ViewBag.ThongBao = "Đã Tồn Tại file";
                        System.IO.File.Delete(filePath);
                    }
                    else
                    {
                        uploadFile.SaveAs(filePath);
                    }

                    int id = dan.MaDAN;
                    
                    //luu co so du lieu
                    UpdateModel(dan);
                    dan = (from d in db.DANs where d.MaDAN == id select d).SingleOrDefault();
                    dan.TenDAN = form["TenDAN"];
                    dan.AnhMinhHoa = file_path_upload_to_database;
                    dan.MoTa = form["Mota"];
                    db.SubmitChanges();
                }
            }
            return RedirectToAction("Dan", "Admin");
        }
    }
}