using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnLapTrinhWeb.Models;
namespace DoAnLapTrinhWeb.Controllers
{
    public class GiohangController : Controller
    {
        dbQLBanDanDataContext db = new dbQLBanDanDataContext();
        // GET: Giohang
        public List<Giohang> layGioHang() {
            List<Giohang> lstGioHang = Session["Giohang"] as List<Giohang>;
            if (lstGioHang == null) {
                lstGioHang = new List<Giohang>();
                Session["Giohang"] = lstGioHang;
            }
            return lstGioHang;
        }

        public ActionResult ThemGioHang(int iMaDAN, string strURL) {
            List<Giohang> lstGioHang = layGioHang();
            //kiem tra co ton tai khong
            Giohang sanPham = lstGioHang.Find(n => n.iMaDAN == iMaDAN);
            if (sanPham == null)
            {
                sanPham = new Giohang(iMaDAN);
                lstGioHang.Add(sanPham);
                return Redirect(strURL);
            }
            else {
                sanPham.iSoLuong++;
                return Redirect(strURL);
            
            }
            
        }

        private double TongTien() {
            double dTongTien = 0;
            List<Giohang> lstGioHang = Session["Giohang"] as List<Giohang>;

            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }
        private int TongSoLuong() {
            int iTongSoLuong = 0;
            List<Giohang> lstGioHang = Session["Giohang"] as List<Giohang>;
            if (lstGioHang != null) {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }

        public ActionResult Giohang() {
            List<Giohang> lstGioHang = layGioHang();
            if (lstGioHang.Count == 0) {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }

        public ActionResult GiohangPartial() {
            ViewBag.TongSoLuong = TongSoLuong();
            
            return PartialView();
        }
        public ActionResult TongTienPartial() {
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGioHang(int MaDAN) {
            List<Giohang> lstGioHang = layGioHang();

            Giohang sanpham = lstGioHang.SingleOrDefault(n => n.iMaDAN == MaDAN);

            if (sanpham != null) {
                lstGioHang.RemoveAll(n => n.iMaDAN == MaDAN);
                return RedirectToAction("Giohang");
            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Giohang");
        }

        public ActionResult CapNhatGioHang(int iMaDAN, FormCollection form) {
            List<Giohang> lstGioHang = layGioHang();
            Giohang sanpham = lstGioHang.SingleOrDefault(n => n.iMaDAN == iMaDAN);
            if (sanpham != null) {
                sanpham.iSoLuong = int.Parse(form["txtSoLuong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult DatHang() {
            List<Giohang> lstGioHang = layGioHang();
            
            if (Session["TaiKhoan"] == null) {
                return RedirectToAction("Dangnhap", "Nguoidung");

            }
            if (Session["Giohang"] == null) {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            

            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection form) {
            DonDatHang donDatHang = new DonDatHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<Giohang> lstGioHang = layGioHang();

            donDatHang.MaKH = kh.MaKH;
            donDatHang.NgayDH = DateTime.Now;
            var ngayGiao = form["NgayGiao"].ToString();
            ngayGiao = String.Format("{0:MM/dd/yyyy}", form["NgayGiao"]);
            if (String.IsNullOrEmpty(ngayGiao)) {
                ngayGiao = DateTime.Now.ToString();
            }
            donDatHang.NgayGiao = DateTime.Parse(ngayGiao);
            donDatHang.TinhTrangGiaoHang = false;
            donDatHang.DaThanhToan = false;

            db.DonDatHangs.InsertOnSubmit(donDatHang);
            db.SubmitChanges();

            foreach (var item in lstGioHang) {
                ChiTietDatHang chiTietDatHang = new ChiTietDatHang();
                chiTietDatHang.SoDH = donDatHang.SoDH;
                chiTietDatHang.MaDAN = item.iMaDAN;
                chiTietDatHang.SoLuong = item.iSoLuong;
                chiTietDatHang.DonGia = (decimal)item.dDongia;

                db.ChiTietDatHangs.InsertOnSubmit(chiTietDatHang);  
            }
            db.SubmitChanges();
            Session["Giohang"] = null;
            

            return RedirectToAction("XacNhanDonHang", "Giohang");
        }

        public ActionResult XacNhanDonHang() {
            return View();
        }
    }
}