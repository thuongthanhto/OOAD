using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using WebLinhKienMayTinh.Models;
using WebLinhKienMayTinh.Models.ViewModel;

namespace WebLinhKienMayTinh.Controllers
{
    public class QuanLyDonHangController : Controller
    {

        // GET: QuanLyDonHang
        QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        /// <summary>
        /// Coder: Trần Ngọc Tú
        /// Hiển thị danh sách các thẻ chưa thanh toán, chưa giao, đã giao và đã thanh toán
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// Coder: Trần Ngọc Tú
        /// Hiển thị danh sách đơn hàng chưa thanh toán
        /// </summary>
        /// <returns></returns>
        public ActionResult ChuaThanhToan()
        {
            //Lấy danh sách các đơn hàng Chưa duyệt
            var c = from a in db.DonDatHangs
                    where a.DaThanhToan == false && a.DaHuy == false
                    select new DonDatHangViewModel()
                    {
                        MaDDH = a.MaDDH,
                        TenKH = a.KhachHang.TenKH,
                        TinhTrangGiaoHang = a.TinhTrangGiaoHang,
                        NgayDat = a.NgayDat,
                    };
            var lst = c.OrderBy(n => n.NgayDat);
            return View(lst);
        }
        /// <summary>
        /// Coder: Trần Ngọc Tú
        /// Hiển thị danh sách đơn hàng chưa giao
        /// </summary>
        /// <returns></returns>
        public ActionResult ChuaGiao()
        {
            //Lấy danh sách đơn hàng chưa giao 
            var c = from a in db.DonDatHangs
                    where a.TinhTrangGiaoHang == false && a.DaThanhToan == true && a.DaHuy == false
                    select new DonDatHangViewModel()
                    {
                        MaDDH = a.MaDDH,
                        TenKH = a.KhachHang.TenKH,
                        NgayDat = a.NgayDat,
                    };
            var lst = c.OrderBy(n => n.NgayDat);        
            return View(lst);
        }
        /// <summary>
        /// Coder: Trần Ngọc Tú
        /// Hiển thị danh sách đơn hàng đã giao và đã thanh toán
        /// </summary>
        /// <returns></returns>
        public ActionResult DaGiaoDaThanhToan()
        {
            //Lấy danh sách đơn hàng đã giao đã thanh toán
            var c = from a in db.DonDatHangs
                    where a.TinhTrangGiaoHang == true && a.DaThanhToan == true && a.DaHuy == false
                    select new DonDatHangViewModel()
                    {
                        MaDDH = a.MaDDH,
                        TenKH = a.KhachHang.TenKH,
                        NgayGiao = a.NgayGiao,
                    };
            var lst = c.OrderBy(n => n.NgayGiao); 
            return View(lst);
        }
        /// <summary>
        /// Coder: Trần Ngọc Tú
        /// Chọn đơn hàng để duyệt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult DuyetDonHang(int? id)
        {
            //Kiểm tra xem id hợp lệ không
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            //Kiểm tra đơn hàng có tồn tại không
            if (model == null)
            {
                return HttpNotFound();
            }
            //Lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }
        /// <summary>
        /// Coder: Trần Ngọc Tú
        /// Thực hiện duyệt đơn hàng
        /// </summary>
        /// <param name="ddh"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DuyetDonHang(DonDatHang ddh)
        {
            //Truy vấn lấy ra dữ liệu của đơn hàn đó 
            DonDatHang ddhUpdate = db.DonDatHangs.Single(n => n.MaDDH == ddh.MaDDH);
            if (ddhUpdate != null)
            {
                if (ddh.TinhTrangGiaoHang == true && ddh.DaThanhToan==true)
                {
                    ddhUpdate.NgayGiao = DateTime.Now;
                }
                ddhUpdate.DaThanhToan = ddh.DaThanhToan;
                ddhUpdate.TinhTrangGiaoHang = ddh.TinhTrangGiaoHang;
                db.SaveChanges();
                //Lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
                var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == ddh.MaDDH);
                ViewBag.ListChiTietDH = lstChiTietDH;
                //Gửi khách hàng 1 mail để xác nhận việc thanh toán 

                ViewBag.Message = "Lưu thành công";
            }
            else
            {
                ViewBag.Message = "Lưu không thành công";
            }
            return View(ddhUpdate);
        }
        /// <summary>
        /// Coder: Trần Ngọc Tú
        /// Trả về danh sách đơn hàng đã hủy
        /// </summary>
        /// <returns></returns>
        public ActionResult DanhSachDonHangDaHuy()
        {
            //Lấy danh sách đơn hàng đã giao đã hủy
            var c = from a in db.DonDatHangs
                    where a.DaHuy == true
                    select new DonDatHangViewModel()
                    {
                        MaDDH = a.MaDDH,
                        TenKH = a.KhachHang.TenKH,
                    };
            var lst = c.OrderBy(n => n.MaDDH); 
            return View(lst);
        }
        /// <summary>
        /// Coder: Trần Ngọc Tú
        /// Hàm hủy đơn hàng
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult HuyDonHang(int id)
        {
            if (id <= 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang dh = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if (dh == null)
            {
                return HttpNotFound();
            }
            dh.DaHuy = true;
            db.Entry(dh).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TempData["ThongBao"] = "Hủy thành công";
            return RedirectToAction("Index");
        }
        /// <summary>
        /// Coder: Trần Ngọc Tú
        /// Xử lý hoàn tác
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 
        [HttpGet]
        public ActionResult HoanTac(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang dh = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            if (dh == null)
            {
                return HttpNotFound();
            }
            dh.DaHuy = false;
            db.Entry(dh).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            TempData["ThongBao"] = "Hoàn tác thành công";
            return RedirectToAction("DanhSachDonHangDaHuy");
        }

        public ActionResult ChiTietDonDatHang(int? id)
        {
            //Kiểm tra xem id hợp lệ không
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonDatHang model = db.DonDatHangs.SingleOrDefault(n => n.MaDDH == id);
            //Kiểm tra đơn hàng có tồn tại không
            if (model == null)
            {
                return HttpNotFound();
            }
            //Lấy danh sách chi tiết đơn hàng để hiển thị cho người dùng thấy
            var lstChiTietDH = db.ChiTietDonDatHangs.Where(n => n.MaDDH == id);
            ViewBag.ListChiTietDH = lstChiTietDH;
            return View(model);
        }
    }
}