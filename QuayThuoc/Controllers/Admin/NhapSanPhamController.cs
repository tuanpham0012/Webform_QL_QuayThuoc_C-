using QuayThuoc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuayThuoc.Controllers.Admin
{
    public class NhapSanPhamController : Controller
    {

        QLQuayThuocDBEntities db = new QLQuayThuocDBEntities();
        DungChung dc = new DungChung();
        // GET: NhapSanPham
        [HttpGet]
        public JsonResult DonNhap(int Id_NhanVien)
        {
            try
            {
                db.Configuration.ProxyCreationEnabled = false;
                var DBDonNhap = (from dh in db.DonMuas
                            join sp in db.SanPhams on dh.Id_SanPham equals sp.Id_SanPham
                            where dh.Id_NhanVien == Id_NhanVien
                            select new
                            {
                                Id = dh.Id_DonMua,
                                MaHang = dh.MaHang,
                                TenSp = sp.TenSP,
                                Id_SanPham = sp.Id_SanPham,
                                SoLuong = dh.SoLuong,
                                GiaBan = dh.GiaBan,
                                GiaNhap = dh.GiaNhap,
                                GiamGia = dh.GiamGia,
                                NgaySx = dh.NgaySx,
                                HanSd = dh.HanSd,
                            }).ToList();
                return Json(new { code = 200, DonMua = DBDonNhap, msg = "Lấy dữ liệu thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Lấy dữ liệu thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult ThemSpNhap(int Id_SanPham, int Id_NhanVien)
        {
            try
            {
                var check = db.DonMuas.Where(dn => dn.Id_NhanVien == Id_NhanVien && dn.Id_SanPham == Id_SanPham).FirstOrDefault();
                if (check == null)
                {
                    var dh = new DonMua();
                    dh.MaHang = "";
                    dh.Id_SanPham = Id_SanPham;
                    dh.SoLuong = 0;
                    dh.GiaBan = 0;
                    dh.GiaNhap = 0;
                    dh.GiamGia = 0;
                    dh.NgaySx = null;
                    dh.HanSd = null;
                    dh.Id_NhanVien = Id_NhanVien;
                    db.DonMuas.Add(dh);
                    db.SaveChanges();
                    return Json(new { code = 200, msg = "Thêm vào đơn thành công!" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 202, msg = "Sản phẩm đã có trong đơn!" }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm vào đơn thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateDonNhap(int Id, String MaHang, int SoLuong, int GiaBan, int GiaNhap, int GiamGia, DateTime NgaySx, DateTime HanSd, Boolean NgaySxCheck, Boolean HanSdCheck)
        {
            try
            {
                var ds = db.DonMuas.Find(Id);
                if (MaHang != "")
                {
                    ds.MaHang = MaHang;
                }
                if (SoLuong != -1)
                {
                    ds.SoLuong = SoLuong;
                }
                if (GiaBan != -1)
                {
                    ds.GiaBan = GiaBan;
                }
                if (GiaNhap != -1)
                {
                    ds.GiaNhap = GiaNhap;
                }
                if (GiamGia != -1)
                {
                    ds.GiamGia = GiamGia;
                }
                if (NgaySxCheck)
                {
                    ds.NgaySx = NgaySx;
                }
                if (HanSdCheck)
                {
                    ds.HanSd = HanSd;
                }

                db.SaveChanges();
                return Json(new { code = 200, msg = "Success" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Thêm vào đơn thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult NhapHang(int Id_NhanVien)
        {
            try
            {
                var DonNhap = db.DonMuas.Where(dn => dn.Id_NhanVien == Id_NhanVien).ToList();
                var idDHNhap = db.HoaDonNhaps.OrderByDescending(hd => hd.Id_HoaDonNhap).FirstOrDefault();
                DateTime date = DateTime.Now;
                var MaHd = "";
                if (idDHNhap == null)
                {
                    MaHd = dc.CreateMaHD("HDN", 1);
                }
                else
                {
                    var id = idDHNhap.Id_HoaDonNhap + 1;
                    MaHd = dc.CreateMaHD("HDN", id);
                }
                var TongTien = 0;
                var TongGiamGia = 0;
                foreach (var item in DonNhap)
                {
                    var NhapHang = new KhoHang();
                    var SpNhap = new SanPhamNhap();
                    Boolean check = true;
                    if (item.MaHang != "")
                    {
                        NhapHang.MaHang = item.MaHang;
                    }
                    else
                    {
                        check = false;
                    }
                    if (item.SoLuong > 0)
                    {
                        NhapHang.SoLuong = item.SoLuong;
                    }
                    else
                    {
                        check = false;
                    }
                    if (item.GiaBan > 0)
                    {
                        NhapHang.GiaBan = item.GiaBan;
                    }
                    else
                    {
                        check = false;
                    }
                    if (item.GiaNhap > 0)
                    {
                        NhapHang.GiaNhap = item.GiaNhap;
                    }
                    else
                    {
                        check = false;
                    }
                    if (item.GiamGia >= 0)
                    {
                        NhapHang.GiamGia = 0;
                    }
                    else
                    {
                        check = false;
                    }
                    if (item.NgaySx != null)
                    {
                        NhapHang.NgaySX = item.NgaySx;
                    }
                    else
                    {
                        check = false;
                    }
                    if (item.HanSd != null)
                    {
                        NhapHang.HanSD = (DateTime)item.HanSd;
                    }
                    else
                    {
                        check = false;
                    }
                    if (check)
                    {
                        NhapHang.Id_SanPham = item.Id_SanPham;
                        db.KhoHangs.Add(NhapHang);

                        SpNhap.MaHoaDon = MaHd;
                        SpNhap.Id_SanPham = item.Id_SanPham;
                        SpNhap.SoLuong = item.SoLuong;
                        SpNhap.GiaNhap = item.GiaNhap;
                        SpNhap.GiamGia = item.GiamGia;
                        SpNhap.NgayNhap = date;
                        db.SanPhamNhaps.Add(SpNhap);

                        TongTien = TongTien + ((int)item.SoLuong * (int)item.GiaNhap);
                        TongGiamGia = TongGiamGia + ((int)item.SoLuong * (int)item.GiamGia);


                        var XoaSanPhamHet = db.KhoHangs.Where(kh => kh.Id_SanPham == item.Id_SanPham && kh.SoLuong == 0).ToList();
                        foreach (var i in XoaSanPhamHet)
                        {
                            db.KhoHangs.Remove(i);
                            db.SaveChanges();
                        }

                        db.DonMuas.Remove(item);

                        db.SaveChanges();
                    }
                }
                var HDNhap = new HoaDonNhap();
                HDNhap.MaHoaDon = MaHd;
                HDNhap.TongTien = TongTien;
                HDNhap.GiamGia = TongGiamGia;
                HDNhap.Id_NhanVien = Id_NhanVien;
                HDNhap.NgayNhap = date;
                HDNhap.DaXoa = false;
                db.HoaDonNhaps.Add(HDNhap);
                db.SaveChanges();


                DateTime day = DateTime.Today;
                var DBDoanhThu = db.DoanhThus.Where(dt => dt.Ngay == day).FirstOrDefault();
                if (DBDoanhThu == null)
                {
                    var Dt = new DoanhThu();
                    Dt.Ngay = day;
                    Dt.DoanhThuBan = 0;
                    Dt.TienNhapHang = 0;
                    Dt.ThuChiKhac = 0;
                    db.DoanhThus.Add(Dt);
                    db.SaveChanges();
                    DBDoanhThu = db.DoanhThus.Where(dt => dt.Ngay == day).FirstOrDefault();
                }
                DBDoanhThu.TienNhapHang = TongTien - TongGiamGia;
                db.SaveChanges();

                return Json(new { code = 200, msg = "Nhập hàng thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Nhập hàng thất bại" + ex.Message }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public JsonResult XoaSpNhap(int Id)
        {
            try
            {
                var dn = db.DonMuas.Find(Id);
                db.DonMuas.Remove(dn);
                db.SaveChanges();

                return Json(new { code = 200, msg = "Xóa thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Xóa thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult XoaDonNhap(int Id_NhanVien)
        {
            try
            {
                var dn = db.DonMuas.Where(dh => dh.Id_NhanVien == Id_NhanVien).ToList();

                if (dn.Count > 0)
                {
                    foreach (var item in dn)
                    {
                        db.DonMuas.Remove(item);
                        db.SaveChanges();
                    }
                }

                return Json(new { code = 200, msg = "Hủy đơn thành công!" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { code = 500, msg = "Hủy đơn thất bại!" + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}