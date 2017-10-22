using PagedList;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using WebLinhKienMayTinh.Models;

namespace WebLinhKienMayTinh.Controllers
{
    public class SanPhamController : Controller
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        // GET: SanPham
        [ChildActionOnly]
        public ActionResult SanPhamStylePartial()
        {
            return PartialView();
        }

        /// <summary>
        /// Coder: Trần Ngọc Tú
        /// Xem chi tiết một sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult XemChiTiet(int? id)
        {
            //Kiểm tra tham số truyền vào có rổng hay không
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Nếu không thì truy xuất csdl lấy ra sản phẩm tương ứng
            SanPham sp = db.SanPhams.SingleOrDefault(n => n.MaSP == id && n.DaXoa == false);
            if (sp == null)
            {
                //Thông báo nếu như không có sản phẩm đó
                return HttpNotFound();
            }
            return View(sp);
        }

        /// <summary>
        /// Coder: Tô Thành Thương
        /// Trang danh sách sản phẩm theo nhà sản xuất
        /// </summary>
        /// <param name="MaLoaiSP"></param>
        /// <param name="MaNSX"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult SanPham(int? MaLoaiSP, int? MaNSX, int? page)
        {
            // Load sản phẩm dựa theo 2 tiêu chí là Mã loại sản phẩm và mã nhà sản xuất (2 trường này có trong bảng sản phẩm)
            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var listSanPham = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX).OrderBy(n => n.TenSP);
            if (!listSanPham.Any())
            {
                // Thông báo nếu không tìm thấy sản phẩm nào
                return HttpNotFound();
            }

            // Thực hiện chức năng phân trang
            // Tạo biến số sản phẩm trên trang
            const int pageSize = 3;
            // Tạo biến thứ 2: Số trang hiện tại
            int pageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;
            return View(listSanPham.OrderBy(n => n.MaSP).ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// Coder: Trần Huy Thịnh
        /// Sắp xếp sản phẩm từ thấp đến cao
        /// </summary>
        /// <param name="MaLoaiSP"></param>
        /// <param name="MaNSX"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult SanPhamGiaTangDan(int? MaLoaiSP, int? MaNSX, int? page)
        {
            // Load sản phẩm dựa theo 2 tiêu chí là Mã loại sản phẩm và mã nhà sản xuất (2 trường này có trong bảng sản phẩm)
            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var listSanPham = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX).OrderBy(n => n.DonGia);
            if (!listSanPham.Any())
            {
                // Thông báo nếu không tìm thấy sản phẩm nào
                return HttpNotFound();
            }

            // Thực hiện chức năng phân trang
            // Tạo biến số sản phẩm trên trang
            const int pageSize = 3;
            // Tạo biến thứ 2: Số trang hiện tại
            int pageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;
            return View(listSanPham.OrderBy(n => n.DonGia).ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// Coder: Trần Ngọc Tú
        /// Sắp xếp sản phẩm từ cao đến thấp
        /// </summary>
        /// <param name="MaLoaiSP"></param>
        /// <param name="MaNSX"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public ActionResult SanPhamGiaGiamDan(int? MaLoaiSP, int? MaNSX, int? page)
        {
            // Load sản phẩm dựa theo 2 tiêu chsi là Mã loại sản phẩm và mã nhà sản xuất (2 trường này có trong bảng sản phẩm)
            if (MaLoaiSP == null || MaNSX == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var listSanPham = db.SanPhams.Where(n => n.MaLoaiSP == MaLoaiSP && n.MaNSX == MaNSX).OrderBy(n => n.DonGia);
            if (!listSanPham.Any())
            {
                // Thông báo nếu không tìm thấy sản phẩm nào
                return HttpNotFound();
            }

            // Bước 2:
            // Thực hiện chức năng phân trang
            // Tạo biến số sản phẩm trên trang
            const int pageSize = 3;
            // Tạo biến thứ 2: Số trang hiện tại
            int pageNumber = (page ?? 1);
            ViewBag.MaLoaiSP = MaLoaiSP;
            ViewBag.MaNSX = MaNSX;
            return View(listSanPham.OrderByDescending(n => n.DonGia).ToPagedList(pageNumber, pageSize));
        }

        /// <summary>
        /// Coder: Trần Huy Thịnh
        /// Menu danh mục bên trái
        /// </summary>
        /// <returns></returns>
        public ActionResult DanhMucStylePartial()
        {
            // Truy vấn lấy về 1 list các sản phẩm là laptop, PC và màn hình LCD
            var lstSanPham = db.SanPhams;
            ViewBag.lstSanPham = lstSanPham;
            return PartialView();
        }
    }
}