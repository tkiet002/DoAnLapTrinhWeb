using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DoAnLapTrinhWeb.Controllers
{
    public class GetDataFromSession
    {
        private HttpContextBase context { get; set; }

        public GetDataFromSession(HttpContextBase context) {
            this.context = context;
        }

        public  void GetData() {
            var value = this.context.Session["TaiKhoan"];
        }
    }
}