using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebLinhKienMayTinh.Models;
namespace WebLinhKienMayTinh.Controllers
{
    public class QuanLyNhaCungCapController : Controller
    {
        // GET: QuanLyNhaCungCap
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        /// <summary>
        /// Coder: Châu Ngọc Thái Sơn
        /// Lấy ra danh sách nhà cung cấp chưa xóa
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            var lstNCC = db.NhaCungCaps.Where(n=>n.DaXoa.Value == false).OrderBy(n => n.MaNCC);
            return View(lstNCC);
        }
        
        /// <summary>
        /// Coder: Châu Ngọc Thái Sơn
        /// Trả về trang tạo mới
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult TaoMoi()
        {
            return View();
        }
        
        /// <summary>
        /// Coder: Châu Ngọc Thái Sơn
        /// Xử lý thêm nhà cung cấp
        /// </summary>
        /// <param name="ncc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TaoMoi(NhaCungCap ncc)
        {
            var ncc1 = db.NhaCungCaps.FirstOrDefault(n => n.TenNCC == ncc.TenNCC);
            if (ncc1 == null)
            {
                ncc.DaXoa = false;
                db.NhaCungCaps.Add(ncc);
                db.SaveChanges();
                ViewBag.ThongBao = "Thêm thành công";
            }
            else
            {
                ViewBag.ThongBao = "Tên nhà cung cấp đã tồn tại";
            }
            return View();
        }

        /// <summary>
        /// Coder: Châu Ngọc Thái Sơn
        /// Trả về trang cập nhật sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        { 
            //Lấy nhà cung cấp cần chỉnh sửa dựa vào id
            if (id == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            
            if (ncc == null)
            {
                return HttpNotFound();
            }
            return View(ncc);
        }
        
        /// <summary>
        /// Coder: Châu Ngọc Thái Sơn
        /// Xử lý cập nhật nhà cung cấp
        /// </summary>
        /// <param name="ncc"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ChinhSua(NhaCungCap ncc)
        {
            ncc.DaXoa = false;
            db.Entry(ncc).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            ViewBag.ThongBao = "Cập nhật thành công";
   
            return View(ncc);
        }

        /// <summary>
        /// Coder: Châu Ngọc Thái Sơn
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
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            if (ncc == null)
            {
                return HttpNotFound();
            }
            ncc.DaXoa = true;
            db.Entry(ncc).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            ViewBag.ThongBao = "Xóa thành công";
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// Trả về danh sách nhà cung cấp đã xóa
        /// </summary>
        /// <returns></returns>
        public ActionResult DanhSachDaXoa()
        {
            var lstNCC = db.NhaCungCaps.Where(n => n.DaXoa.Value == true).OrderBy(n => n.MaNCC);
            return View(lstNCC);
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
            NhaCungCap ncc = db.NhaCungCaps.SingleOrDefault(n => n.MaNCC == id);
            if (ncc == null)
            {
                return HttpNotFound();
            }
            ncc.DaXoa = false;
            db.Entry(ncc).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            ViewBag.ThongBao = "Đã hoàn tác";
            return RedirectToAction("Index");
        }
    }
}