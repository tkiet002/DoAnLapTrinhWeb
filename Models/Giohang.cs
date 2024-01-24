using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnLapTrinhWeb.Models
{
    public class Giohang
    {
        dbQLBanDanDataContext db = new dbQLBanDanDataContext();
        public int iMaDAN { get; set; }
        public string sTenDAN { get; set; }
        public string sAnhMinhHoa { get; set; }
        public double dDongia { get; set; }
        public int iSoLuong { get; set; }
        public double dThanhTien { 
            get { return iSoLuong * dDongia; }
        }

        public Giohang(int MaDAN) {
            iMaDAN = MaDAN;
            DAN dan = db.DANs.Single(n => n.MaDAN == iMaDAN);
            sTenDAN = dan.TenDAN;
            sAnhMinhHoa = dan.AnhMinhHoa;
            dDongia = double.Parse(dan.GiaBan.ToString());
            iSoLuong = 1;   
        }


    }
}