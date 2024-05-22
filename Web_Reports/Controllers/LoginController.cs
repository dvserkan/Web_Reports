using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Protocol.Plugins;
using Web_Reports.Contexts;
using Web_Reports.Entities;

namespace Web_Reports.Controllers
{
    public class LoginController : Controller
    {
        ReportsContext context = new ReportsContext();

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        #region Login
        [HttpPost]
        public async Task<IActionResult> Index(InfiniaWebReportUser p)
        {
            //dışarıdan gelen p parametresinde ilgili alanları user tablosundaki alanları karşılaştırır ve values atar.
            var values = context.InfiniaWebReportUsers.FirstOrDefault(x => x.UserName == p.UserName && x.UserPassword == p.UserPassword);

            if (values != null) //values null eşit değilse işlem gerçekleştirir.
            {
                var deger = values.NameSurname;
                var rol = values.Role;
                var image = values.İmageUrl;

                HttpContext.Session.SetString("NameSurname", deger);  //ViewComponentlere veri taşıma işlemi için mecburen bu şekilde session değerlerini atayarak veri taşıdım.
				HttpContext.Session.SetString("Role", rol);
				HttpContext.Session.SetString("İmageUrl", image);

				return RedirectToAction("DashboardIndex", "Default"); //Giriş başarılı ise Dashboard/Index yönlendirir.
			}
            else
            {
                // Hatalı giriş durumunda, kullanıcıyı aynı sayfaya yönlendirerek hata mesajı gösterir.
                ModelState.AddModelError("", "Hatalı kullanıcı adı veya şifre");
            }
            return View();
        }
        #endregion

        #region LogOut

        public async Task<IActionResult> LogOut()
        {
            return RedirectToAction("Index", "Login"); //Basit bir mantıkla direk login ekranına atar.
        }

        #endregion
    }
}

