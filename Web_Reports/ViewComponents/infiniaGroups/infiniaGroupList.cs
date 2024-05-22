using Microsoft.AspNetCore.Mvc;
using Web_Reports.Contexts;

namespace Web_Reports.ViewComponents.infiniaGroups
{
    public class infiniaGroupList : ViewComponent
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public infiniaGroupList(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        ReportsContext context = new ReportsContext();
        public async Task<IViewComponentResult> InvokeAsync()
        {
			var role = _httpContextAccessor.HttpContext.Session.GetString("Role"); //Sessiondan gelen veriyi aldım
			ViewBag.role = role; //Viewbag ile view taşıdım ve orada if şartları ile admin tüm raporları görebilir fakat örnek bölge sorumlusu sadece dashboard görebilir yapısını kurdum.

			var values = context.InfiniaWebReportGroups.OrderBy(x=> x.DisplayOrderId).ToList(); //Veri tabanında displayorderıd düzenine göre sıralama yapıp sidebar da sıralama yapıyor.
            return View(values);
        }
    }
}
