using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnLapTrinhWeb.Models;
namespace DoAnLapTrinhWeb.Controllers
{
    public class NguoidungController : Controller
    {
        dbQLBanDanDataContext db = new dbQLBanDanDataContext();
        // GET: Nguoidung
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]

        public ActionResult Dangky() {
            return View();
        }
        [HttpPost]
        
        public ActionResult Dangky(FormCollection form , KhachHang kh) {
            var hoten = form["HotenKH"];
            var tendn = form["TenDN"];
            var matkhau = form["Matkhau"];
            var matkhaunhaplai = form["MatkhauNhapLai"];
            var diachi = form["Diachi"];
            var email = form["Email"];
            var dienthoai = form["SoDienThoai"];
            if (String.IsNullOrEmpty(hoten)) {
                ViewData["LoiHoten"] = "Họ tên không được để trống";
            }
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["LoiTenDN"] = "Tên đăng nhập không được để trống";
            }
            if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["LoiMatKhau"] = "Mật khẩu không được để trống";
            }
            if (!matkhau.Equals(matkhaunhaplai))
            {
                ViewData["LoiMatKhauLai"] = "Mật khẩu Nhập Lại không giống với mật khẩu bạn nhập";
            }
            if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["LoiMatKhauLai"] = "Xin hãy nhập lại mật khẩu";
            }
            if (String.IsNullOrEmpty(diachi))
            {
                ViewData["LoiDiachi"] = "Địa chỉ không được để trống";
            }
            if (String.IsNullOrEmpty(email))
            {
                ViewData["LoiEmail"] = "Email không được để trống";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["LoiSDT"] = "Số Điện Thoại không được để trống";
            }
            else
            {
                kh.HoTen = hoten;
                kh.TaiKhoan = tendn;
                kh.MatKhau = matkhau;
                kh.Email = email;
                kh.DiachiKH = diachi;
                kh.DienThoaiKH = dienthoai;
                db.KhachHangs.InsertOnSubmit(kh);
                db.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }


            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap() {
            return View();
        }
        [HttpPost]
        
        public ActionResult Dangnhap(FormCollection form)
        {
            var tendn = form["TenDN"];
            var matkhau = form["Matkhau"];
            
            if (String.IsNullOrEmpty(tendn) || String.IsNullOrEmpty(matkhau))
            {
                ViewData["LoiDangNhap"] = "Mật Khẩu Hoặc Tên Đăng Nhập Không Được Để Trống";
            }
            else {
                KhachHang kh = db.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                if (kh != null) {
                    ViewData["LoiDangNhap"] = "Đăng Nhập Thành Công";
                    Session["TaiKhoan"] = kh;
                    Session["Username"] = kh.TaiKhoan;
                    
                    //return RedirectToAction("Index", "Home");
                }else
                    ViewData["LoiDangNhap"] = "Đăng Nhập Thất Bại";
            }
             
            return RedirectToAction("Index", "Home");
        }

        public ActionResult DangXuat() {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}