using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLinhKienMayTinh.Models;

namespace WebLinhKienMayTinh.Controllers
{
    public class QuanLyKhachHangController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyKhachHang
        // Hiển thị danh sách khách hàng ra trang quản trị.
        public ActionResult Index()
        {
            var lstKhachHang = db.KhachHangs;
            return View(lstKhachHang);
        }
    }
}