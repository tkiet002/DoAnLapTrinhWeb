using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnLapTrinhWeb.Models
{
    public class ViewModel
    {
        public IEnumerable<DAN> cDan {get; set;}
        public IEnumerable<THUONGHIEU> cThuongHieu { get; set; }
    }
}