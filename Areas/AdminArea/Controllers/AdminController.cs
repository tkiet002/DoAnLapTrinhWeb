using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnLapTrinhWeb.Models;

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

        [HttpPost]
        [ValidateAntiForgeryToken]
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
    }
}