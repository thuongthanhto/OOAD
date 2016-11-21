using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLinhKienMayTinh.Models;
using PagedList;

namespace WebLinhKienMayTinh.Controllers
{
    public class TimKiemController : Controller
    {
        // GET: TimKiem
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: TimKiem
        public ActionResult KQTimKiem(string sTuKhoa, int? page)
        {
            if (Request.HttpMethod != "GET")
            {
                page = 1;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            //tìm kiếm theo ten sản phẩm
            var lstSP = db.SanPhams.Where(n => n.TenSP.Contains(sTuKhoa));
            ViewBag.TuKhoa = sTuKhoa;
            return View(lstSP.OrderBy(n => n.TenSP).ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult LayTuKhoaTimKiem(string sTuKhoa)
        {

            return RedirectToAction("KQTimKiem", new { @sTuKhoa = sTuKhoa });
        }
    }
}