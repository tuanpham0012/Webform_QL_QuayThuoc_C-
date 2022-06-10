using QuayThuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuayThuoc.Controllers.Admin
{
    public class RestoreController : Controller
    {
        QLQuayThuocDBEntities db = new QLQuayThuocDBEntities();
        DungChung dc = new DungChung();
        [HttpGet]
        public JsonResult NhanVien(int pageIndex)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var pageSize = 9;
                var DBNhanVien = db.NhanViens.Join(db.ChucVus, nv => nv.Id_ChucVu,
                                                        cv => cv.Id_ChucVu,
                                                        (nv, cv) => new
                                                        {
                                                            Id = nv.Id_NhanVien,
                                                            TenNv = nv.TenNV,
                                                            Email = nv.Email,
                                                            MaNv = nv.MaNV,
                                                            GioiTinh = nv.GioiTinh,
                                                            NgaySinh = nv.NgaySinh,
                                                            Id_ChucVu = nv.Id_ChucVu,
                                                            ChucVu = cv.TenChucVu,
                                                            DaXoa = nv.DaXoa,
                                                        }).Where(nv => nv.DaXoa == true).OrderByDescending(s => s.Id).ToList();
                
                DBNhanVien = DBNhanVien.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                var pageTotal = (DBNhanVien.Count % pageSize == 0) ? DBNhanVien.Count / pageSize : (DBNhanVien.Count / pageSize) + 1;
                return Json(new { code = 200, data = DBNhanVien, soTrang = pageTotal, msg = "Lấy Danh Sách Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy Danh Sách Thất Bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult KhachHang(int pageIndex)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var pageSize = 9;
                var DBKhachHang = db.KhachHangs.Join(db.LoaiKHs, kh => kh.Id_LoaiKH,
                                                        lsp => lsp.Id_LoaiKH,
                                                        (kh, lkh) => new
                                                        {
                                                            Id = kh.Id_KhachHang,
                                                            TenKh = kh.TenKhachHang,
                                                            DiaChi = kh.DiaChi,
                                                            LienHe = kh.LienHe,
                                                            Id_LoaiKh = kh.Id_LoaiKH,
                                                            LoaiKh = lkh.TenLoaiKH,
                                                            DaXoa = kh.DaXoa,
                                                        }).Where(kh => kh.DaXoa == true).OrderByDescending(s => s.Id).ToList();
                
                DBKhachHang = DBKhachHang.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
                var pageTotal = (DBKhachHang.Count % pageSize == 0) ? DBKhachHang.Count / pageSize : (DBKhachHang.Count / pageSize) + 1;
                return Json(new { code = 200, data = DBKhachHang, soTrang = pageTotal, msg = "Lấy Danh Sách Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy Danh Sách Thất Bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult SanPham(int pageIndex)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var pageSize = 9;
                var DBSanPham = db.SanPhams.Join(db.LoaiSPs, sp => sp.Id_LoaiSP,
                                                        lsp => lsp.Id_LoaiSP,
                                                        (sp, lsp) => new
                                                        {
                                                            Id = sp.Id_SanPham,
                                                            MaSp = sp.MaSP,
                                                            TenSp = sp.TenSP,
                                                            Id_LoaiSp = sp.Id_LoaiSP,
                                                            TenLoaiSp = lsp.TenLoaiSP,
                                                            ThongTin = sp.ThongTin,
                                                            DonVi = sp.DonVi,
                                                            HinhAnh = sp.HinhAnh,
                                                            DaXoa = sp.DaXoa,
                                                        }).Where(sp => sp.DaXoa == true).OrderByDescending(s => s.Id).ToList();

                DBSanPham = DBSanPham.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

                var pageTotal = (DBSanPham.Count % pageSize == 0) ? DBSanPham.Count / pageSize : (DBSanPham.Count / pageSize) + 1;
                return Json(new { code = 200, data = DBSanPham, soTrang = pageTotal, msg = "Lấy Danh Sách Sản Phẩm Thành Công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy Danh Sách Sản Phẩm Thất Bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult RestoreData(int Id, String table)
        {
            try
            {
                if(table == "NhanVien")
                {
                    var tb = db.NhanViens.Find(Id);
                    if(tb != null && tb.DaXoa == true)
                    {
                        tb.DaXoa = false;
                        db.SaveChanges();
                        return Json(new { code = 200, msg = "Khôi phục nhân viên thành công" }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (table == "KhachHang")
                {
                    var tb = db.KhachHangs.Find(Id);
                    if (tb != null && tb.DaXoa == true)
                    {
                        tb.DaXoa = false;
                        db.SaveChanges();
                        return Json(new { code = 200, msg = "Khôi phục khách hàng thành công" }, JsonRequestBehavior.AllowGet);
                    }
                }
                if (table == "SanPham")
                {
                    var tb = db.SanPhams.Find(Id);
                    if (tb != null && tb.DaXoa == true)
                    {
                        tb.DaXoa = false;
                        db.SaveChanges();
                        return Json(new { code = 200, msg = "Khôi phục sản phẩm thành công" }, JsonRequestBehavior.AllowGet);
                    }
                }
                return Json(new { code = 404, msg = "Lỗi không xác định" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Khôi phục thất bại " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}