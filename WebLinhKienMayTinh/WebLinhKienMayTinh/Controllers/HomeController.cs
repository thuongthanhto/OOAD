using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebLinhKienMayTinh.Models;

namespace WebLinhKienMayTinh.Controllers
{
    public class HomeController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            // Lần lượt tạo các viewbag để lấy list sản phẩm từ cơ sở dữ liệu
            // List Laptop mới
            var listLaptopMoi = db.SanPhams.Where(n => n.MaLoaiSP == 1 && n.Moi == true && n.DaXoa == false);
            ViewBag.listLapTopMoi = listLaptopMoi;
            // List PC mới
            var listPCMoi = db.SanPhams.Where(n => n.MaLoaiSP == 2 && n.Moi == true && n.DaXoa == false);
            ViewBag.listPCMoi = listPCMoi;
            // List Màn hình mới
            var listManHinhMoi = db.SanPhams.Where(n => n.MaLoaiSP == 3 && n.Moi == true && n.DaXoa == false);
            ViewBag.listManHinhMoi = listManHinhMoi;
            // List Phụ kiện mới
            var listPhuKienMoi = db.SanPhams.Where(n => (n.MaLoaiSP == 4 || n.MaLoaiSP == 5 || n.MaLoaiSP == 6) && n.Moi == true && n.DaXoa == false);
            ViewBag.listPhuKienMoi = listPhuKienMoi;
            return View();
        }
        public ActionResult MenuPartial()
        {
            // Truy vấn lấy về 1 list các sản phẩm là laptop, PC và màn hình LCD
            var lstSanPham1 = db.SanPhams.Where(n => n.MaLoaiSP == 1 || n.MaLoaiSP == 2 || n.MaLoaiSP == 3);
            ViewBag.lstSanPham1 = lstSanPham1;
            // Truy vấn lấy về 1 list các sản phẩm khác 3 loại trên 
            var lstSanPham2 = db.SanPhams.Where(n => n.MaLoaiSP == 4 || n.MaLoaiSP == 5 || n.MaLoaiSP == 6);
            ViewBag.lstSanPham2 = lstSanPham2;
            return PartialView();
        }

        //Load câu hỏi bí mật
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích?");
            lstCauHoi.Add("Ca sĩ mà bạn yêu thích?");
            lstCauHoi.Add("Hiện tại bạn đang làm công việc gì?");
            return lstCauHoi;
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            if (ModelState.IsValid)
            {
                ViewBag.ThongBao = "Thêm thành công";
                //Mã hóa mật khẩu rồi lưu xuông CSDL.

                //Thêm khách hàng vào csdl
                db.ThanhViens.Add(tv);
                db.SaveChanges();
            }
            else
            {
                ViewBag.ThongBao = "Thêm thất bại";
            }
            return View();          
        }

        //Xây dựng action đăng nhập
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            // Kiểm tra tên đăng nhập và mật khẩu
            string sTaiKhoan = f["txtTenDangNhap"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if (tv != null)
            {
                Session["TaiKhoan"] = tv;
                return RedirectToAction("Index", "Home");
            }
            return Content("Tài khoản hoặc mật khẩu không đúng!");
        }

        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index");
        }
    }
}