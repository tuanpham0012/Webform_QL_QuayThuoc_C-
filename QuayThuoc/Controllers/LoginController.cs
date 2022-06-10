using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using QuayThuoc.Models;
using System.Text;

namespace QuayThuoc.Controllers
{
    public class LoginController : Controller
    {
        QLQuayThuocDBEntities db = new QLQuayThuocDBEntities();
        DungChung dc = new DungChung();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Login(string email, string pass)
        {
            String passMD5 = dc.EncodePassword(pass);
            db.Configuration.ProxyCreationEnabled = false;
            
            var account = db.NhanViens.Join(db.ChucVus, nv => nv.Id_ChucVu, cv => cv.Id_ChucVu,
                                        (nv, cv) => new
                                        {
                                            Id = nv.Id_NhanVien,
                                            Ten = nv.TenNV,
                                            MaNV = nv.MaNV,
                                            email = nv.Email,
                                            password = nv.Password,
                                            Id_ChucVu = nv.Id_ChucVu,
                                            last_login = nv.last_login,
                                            DaXoa = nv.DaXoa,
                                            ChucVu = cv.TenChucVu,
                                        }).Where(nv => nv.email.Equals(email) && nv.password.Equals(passMD5)).FirstOrDefault();

            if (account != null)
            {
                if(account.DaXoa == false)
                {
                    var ac = db.NhanViens.Find(account.Id);
                    DateTime day = DateTime.Now;
                    ac.last_login = day;
                    db.SaveChanges();
                    return Json(new { code = 200, data = account, msg = "Đăng nhập thành công" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { code = 202, msg = "Tài khoản của bạn đang bị khóa! Vui lòng liên hệ hotline để được tư vấn!" }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(new { code = 500, msg = "Đăng nhập thất bại. Sai tên tài khoản hoặc mật khẩu!" }, JsonRequestBehavior.AllowGet);
            }


        }

    }
}