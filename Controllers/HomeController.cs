using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnLapTrinhWeb.Models;
namespace DoAnLapTrinhWeb.Controllers
{
    
    public class HomeController : Controller
    {
        dbQLBanDanDataContext db = new dbQLBanDanDataContext();
        
        private List<DAN> LayDanMoi(int count) {
            return db.DANs.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }

        // GET: Home
        public ActionResult Index()
        {
            //lay 8 quyen sach moi
            var danMoi = LayDanMoi(8);
            
            
            return View(danMoi);
        }

        public ActionResult Thuonghieu()
        {
            var thuonghieu = from th in db.THUONGHIEUs select th;
            var violin = thuonghieu.Where(c => c.MaLoaiDan == 1);
            return PartialView(violin);
        }

        public ActionResult ThuonghieuPiano()
        {
            var thuonghieu = from th in db.THUONGHIEUs select th;
            var piano = thuonghieu.Where(c => c.MaLoaiDan == 2);
            return PartialView(piano);
        }
        //private ViewResult getThuongHieu()
        //{
        //    var thuonghieu = from th in db.THUONGHIEUs select th;
        //    var ketqua = thuonghieu.ToList();
        //    return ketqua;
        //}
        //private IEnumerable<DAN> getDAN()
        //{
        //    var maloaidan = from mld in db.DANs select mld;
        //    var ketqua = maloaidan.ToList();
        //    return ketqua;
        //}
        public ActionResult SPTheoThuongHieu(int math, int maloaidan)
        {
            //var thuonghieu = from th in db.THUONGHIEUs select th;
            //thuonghieu = thuonghieu.Where(c => c.MaLoaiDan == id).Where(x => x.MaTH == math);
            //var ketqua1 =  thuonghieu.AsEnumerable();

            //var dan = from d in db.DANs select d;
            //var ketqua2 = dan.AsEnumerable();

            //ViewModel myModel = new ViewModel() {
            //    cDan = ketqua2,
            //    cThuongHieu = ketqua1
            //};

            //return View(myModel);

            var dan = from th in db.DANs select th;
            dan = dan.Where(c => c.MaTH == math).Where(x => x.MaLoaiDan== maloaidan);

            return View(dan);
        }

        public ActionResult SPMoiTheoThuongHieu()
        {
            var danMoi = LayDanMoi(3);
            return PartialView(danMoi);
        }

        public ActionResult MoTaThuongHieu(int math) {
            var thuonghieu = from th in db.THUONGHIEUs where th.MaTH == math select th;
            
            return PartialView(thuonghieu.Single());
        }

        public ActionResult Details(int id)
        {
            var sanpham = from sp in db.DANs
                          where sp.MaDAN == id
                          select sp;
            var ketqua = sanpham.Single();
            return View(ketqua);
        }
    }
}