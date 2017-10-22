using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EncryptPassword.Helper;
using WebLinhKienMayTinh.Models;

namespace WebLinhKienMayTinh.Controllers
{
    public class QuanLyNhanVienController : Controller
    {
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: QuanLyNhanVien

        /// <summary>
        /// Coder: Tô Thành Thương
        /// Trả về view hiển thị danh sách nhân viên còn hoạt động
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var lstNhanVien = db.ThanhViens.Where(n => n.MaLoaiTV == 3 && n.DaXoa.Value == false); // Nhân viên bán hàng trong cửa hàng còn hoạt động
            return View(lstNhanVien);
        }

        /// <summary>
        /// Coder: Tô Thành Thương 
        /// Trang thêm mới nhân viên
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TaoMoi()
        {
            return View();
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// Xử lý thêm mới nhân viên
        /// </summary>
        /// <param name="tv"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TaoMoi(ThanhVien tv)
        {
            var tv1 = db.ThanhViens.FirstOrDefault(n => n.TaiKhoan == tv.TaiKhoan);
            if (tv1 == null)
            {
                tv.MaLoaiTV = 3;
                tv.DaXoa = false;
                // Mã hóa password
                tv.MatKhau = PasswordHelper.ComputeHash(tv.MatKhau, "MD5", GetBytes("Website"));
                // Thêm khách hàng vào csdl
                db.ThanhViens.Add(tv);
                db.SaveChanges();
                ViewBag.ThongBao = "Thêm thành công";
            }
            else
            {
                ViewBag.ThongBao = "Tên tài khoản đã tồn tại";
            }

            return View();
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// lấy ra các byte từ chuỗi str
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        private static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// Chỉnh sửa thông tin nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //Lấy sản phẩm cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (tv == null)
            {
                return HttpNotFound();
            }
            return View(tv);
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// Xử lý chỉnh sửa nhân viên
        /// </summary>
        /// <param name="tv"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChinhSua(ThanhVien tv)
        {
            tv.MaLoaiTV = 3;
            tv.DaXoa = false;
            db.Entry(tv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            ViewBag.ThongBao = "Cập nhật thành công";

            return View(tv);
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// Xóa nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (tv == null)
            {
                return HttpNotFound();
            }
            tv.DaXoa = true;
            db.Entry(tv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            ViewBag.ThongBao = "Xóa thành công";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// Trả về view hiển thị danh sách nhân viên đã xóa
        /// </summary>
        /// <returns></returns>
        public ActionResult DanhSachDaXoa()
        {
            var lstNhanVien = db.ThanhViens.Where(n => n.MaLoaiTV == 3 && n.DaXoa.Value == true); // Nhân viên bán hàng trong cửa hàng còn hoạt động
            return View(lstNhanVien);
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// Xử lý hoàn tác
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult HoanTac(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.MaThanhVien == id);
            if (tv == null)
            {
                return HttpNotFound();
            }
            tv.DaXoa = false;
            db.Entry(tv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            ViewBag.ThongBao = "Đã hoàn tác";
            return RedirectToAction("Index");
        }
    }
}