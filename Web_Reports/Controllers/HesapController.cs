using Microsoft.AspNetCore.Mvc;
using Web_Reports.Contexts;
using Web_Reports.Entities;

namespace Web_Reports.Controllers
{
    public class HesapController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HesapController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        ReportsContext c = new ReportsContext();

        [HttpGet]
        public IActionResult Index()
        {
            var namesurname = _httpContextAccessor.HttpContext.Session.GetString("NameSurname");    //Session Bilgilerini Al

            var values = c.InfiniaWebReportUsers.FirstOrDefault(x => x.NameSurname == namesurname); // Session Bilgilerini Veritabanı ile
                                                                                                    // karşılaştır uyuşan ilk veriyi view taşı

            return View(values);
        }

        [HttpPost]
        public IActionResult Index(InfiniaWebReportUser p)
        {
            // Sadece İsim, Soyisim , Şifre ve ResimUrl bilgilerini güncelleyebilir.
            var values = c.InfiniaWebReportUsers.Find(p.AutoId);

            if (values != null)
            {
                values.UserName = p.UserName;
                values.NameSurname = p.NameSurname;
                values.UserPassword = p.UserPassword;
                values.İmageUrl = p.İmageUrl;

                c.SaveChanges();
            }

            return RedirectToAction("Index","Login");
        }
     
    }
}
