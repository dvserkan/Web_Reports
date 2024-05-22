using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Web_Reports.Contexts;
using Web_Reports.Entities;

namespace Web_Reports.Controllers
{
    public class ManagementController : Controller
    {
        ReportsContext context = new ReportsContext();

        #region Yönetim Şifre Giriş İşlemleri  

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(int p) //Dışarıdan p parametresi bekler bu alanda view da şifre giriş text input'a denk gelir.
        {

            DateTime simdikiZaman = DateTime.Now; //güncel tarihi alır simdiki zamana atar.

            // Şuanki zamanın saat ve dakika bilgisini al
            int simdikiSaat = simdikiZaman.Hour;
            int simdikiDakika = simdikiZaman.Minute;
            int girisToplam = simdikiSaat * 100 + simdikiDakika; //basit bir mantıkla *100 yapmalıyım yoksa saat + dakika değerini toplar örnek 14:35 = 49 gibi
                                                                 //fakat bana direk 14:35 lazım çözümü de bu


            if (girisToplam == p) //giriş toplam dışarıdan gelen p değerli ile eşit ise işlem gerçekleştirir.
            {
                return RedirectToAction("Setting"); // settings sayfasına yönlendirir.
            }
            else
            {
                ModelState.AddModelError("", "Hatalı Şifre"); // şifre hatalı ise view hata mesajı yollar.
            }
            return View();
        }

        public IActionResult Setting()
        {
            return View();
        }
        #endregion

        #region Kulllanıcı Liste-Ekleme-Düzenleme İşlemleri
        public IActionResult UserList()
        {
            var values = context.InfiniaWebReportUsers.ToList(); // kullanıcı listesini döner.
            return View(values);
        }


        [HttpGet]
        public IActionResult AddUser()
        {
            List<SelectListItem> list = (from x in context.InfiniaWebReportCategories.ToList()  // Kullanıcı ekleme sayfasında kategori listesinden veri çekerek
                                                                                                // ekranda dropdownlistfor kullanmana müsade eder
                                         select new SelectListItem
                                         {
                                             Text = x.CategoryName,
                                             Value = x.CategoryId.ToString()
                                         }).ToList();

            ViewBag.category = list; // aynı durum viewbag ile taşıyarak ekranda dropdownlistfor kullanmana müsade eder

            return View();
        }

        [HttpPost]
        public IActionResult AddUser(InfiniaWebReportUser p)
        {
            p.CategoryId = context.InfiniaWebReportCategories.Where(x => x.CategoryName == p.CategoryName).Select(y => y.CategoryId).FirstOrDefault(); //tablolarda ilişki olmadığı için
                                                                                                                                                       //bu şekilde kategori ıd yakaladım
            context.InfiniaWebReportUsers.Add(p); //ekleme işlemi yapar
            context.SaveChanges();                // kayıt işlemi yapar
            return RedirectToAction("UserList");  //ilgili sayafaya yönlendirir.
        }

        public IActionResult DeleteUser(int id) // dışarıdan gelen id değerine göre işlem gerçekleştirir.
        {
            var values = context.InfiniaWebReportUsers.Find(id); // dışarıdan gelen ıd bulur values atar.
            context.InfiniaWebReportUsers.Remove(values);        // values gelen verileri siler.
            context.SaveChanges();                               // kayıt işlemi yapar  
            return RedirectToAction("UserList");                 //ilgili sayafaya yönlendirir.
        }

        [HttpGet]
        public IActionResult EditUser(int id) //Dışarıdan gelen id değerine göre bulma işlemi yapar ve sayfaya yönlendirir.
        {
            List<SelectListItem> list2 = (from x in context.InfiniaWebReportCategories.ToList()
                                          select new SelectListItem
                                          {
                                              Text = x.CategoryName,
                                              Value = x.CategoryId.ToString()
                                          }).ToList();
            ViewBag.category = list2;

            var values = context.InfiniaWebReportUsers.Find(id);
            return View(values);
        }

        [HttpPost]
        public IActionResult EditUser(InfiniaWebReportUser p)
        {
            p.CategoryId = context.InfiniaWebReportCategories.Where(x => x.CategoryName == p.CategoryName).Select(t => t.CategoryId).FirstOrDefault(); //tablolarda ilişki olmadığı için
            context.InfiniaWebReportUsers.Update(p); // update işlemi gerçekleştirir.                                                                   //bu şekilde kategori ıd yakaladım
            context.SaveChanges();                   // kayıt işlemi yapar 
            return RedirectToAction("UserList");     // ilgili sayafaya yönlendirir.
        }
        #endregion

        #region Rapor Liste-Ekleme-Düzemleme İşlemleri
        public IActionResult ReportList()
        {
            var values = context.InfiniaWebReports.ToList(); //Raporları listeler.
            return View(values);
        }


        [HttpGet]
        public IActionResult AddReport() //Sayfa döndürür ekleme işleminde muhakkak olması şarttır aksi taktirde ekranında her yüklenmesinde veri tabanına kayıt atanır.
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddReport(InfiniaWebReport p)
        {
            p.ReportType = ($"Default/GetReport/RaporID?{p.ReportId}"); //Search yönlendirmesi için raporıd değer atama.
            context.InfiniaWebReports.Add(p); //dışarıdan gelen p parametresindeki değerleri veri tabanına gönderir.
            context.SaveChanges();            //veri tabanına ekler.
            return RedirectToAction("ReportList"); //sayfaya yönlendirir.
        }

        public IActionResult DeleteReport(int id) //dışarıdan gelen id parametresine göre silme işlemi gerçekleştirir.
        {
            var values = context.InfiniaWebReports.Find(id); //özetle id değerine göre find yani bulur values atar 
            context.InfiniaWebReports.Remove(values);       // values değerleri silinir.
            context.SaveChanges();                          //kayıt eder.
            return RedirectToAction("ReportList");          //yönlendirir.
        }

        [HttpGet]
        public IActionResult EditReport(int id) //dışarıdan gelen id parametresine göre rapor düzenlemesi gerçekleştirir.
        {
            var values = context.InfiniaWebReports.Find(id); //ilgili rapor id bulur ve sayfaya verileri döndürür.
            return View(values);
        }

        [HttpPost]
        public IActionResult EditReport(InfiniaWebReport p) //p parametresinden gelen verileri günceller.
        {
            p.ReportType = ($"Default/GetReport/RaporID?{p.ReportId}");
            context.InfiniaWebReports.Update(p);
            context.SaveChanges();
            return RedirectToAction("ReportList");
        }
        #endregion
    }
}
