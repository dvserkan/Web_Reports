using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Web_Reports.ViewComponents.Navbar
{
    public class Navbar : ViewComponent
    {

		private readonly IHttpContextAccessor _httpContextAccessor;
		public Navbar(IHttpContextAccessor httpContextAccessor)
		{
			_httpContextAccessor = httpContextAccessor;
		}

		public IViewComponentResult Invoke()
		{
			var nameSurname = _httpContextAccessor.HttpContext.Session.GetString("NameSurname"); //Sessiondan gelen verileri aldım
			var image = _httpContextAccessor.HttpContext.Session.GetString("İmageUrl");

            ViewBag.image = image;              //Viewbag ile bu verileri view taşıdım ve orada gösterdim.
            ViewBag.NameSurname = nameSurname;

			return View();
		}
	}
}
