using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text;
using System.Xml.Linq;
using Web_Reports.Contexts;

namespace Web_Reports.Controllers
{
    public class DefaultController : Controller
    {
        #region APP SETTİNGS İÇERİSİNDEN BAĞLANTI ADRESİNİ OKU

        private readonly string _connectionString;
        public DefaultController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        #endregion

        ReportsContext c = new ReportsContext();


        #region Search İle Rapor Arama
        public IActionResult Index(string raporIsmi)  //Search bölümü dışarıdan gelecek olan rapor ismi parametresini beklemektedir.
        {
            if (!string.IsNullOrEmpty(raporIsmi))   // Gelen parametre boş veya null değil ise 
            {
                var values = c.InfiniaWebReports.Where(x => x.ReportName.Contains(raporIsmi)).Select(y => y.ReportType).FirstOrDefault(); // tablo içerisine rapor ismi içerisinde uyuşanları yakala	
                                                                                                                                          // Reporttype kolonundaki url yönlendirmesini kullan

                if (values != null) // values null eşit değil ise
                {
                    return Redirect($"/{values}"); //bulunan raporismindeki reportype kolonundaki url al ve ilgili rapor sayfasına yönlendir.
                }
            }
            return RedirectToAction("DashboardIndex", "Default"); //gelen parametre boş veya null ise default olarak Dashboard yönlendir.
        }
        #endregion


        #region RaporID İle SqlQuery Veri Çekme 

     
        public IActionResult GetReport(string p1, string p2, int p3) // Dışarıdan p1 ve p2 isminde tarih parametresi bekler. Aynı zamanda p3 isminde reportıd bekleyerek controller içerisinde dinamik
        {                                                            // bir yapı oluşturarak sorguları veri tabanındaki report tablosundan okumasını sağladım.


            if (!string.IsNullOrEmpty(p1) && !string.IsNullOrEmpty(p2)) // Dışarıdan gelen p1 ve  p2 tarih bilgileri boş değil ise işlem gerçekleştirir.

            {
                DataTable dataTable = new DataTable(); //Datatable nesnesi oluşturdum. verileri datatable fill edebilmek için.


                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    connection.Open(); // Bağlantıyı Açar

                    string sqlQuery = c.InfiniaWebReports.Where(x => x.ReportId == p3).Select(y => y.Query).FirstOrDefault(); // Dışarıdan gelen p3 parametresi reportıd ile eşleşirse eşleşen ilk ıdli raporun
                                                                                                                              // query verisini alarak sqlQuery atar.
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection)) //sql komutu gerçekleştirir.
                    {                                                                 //connect yukarıda tanımladığım appsettings globalinden çeker. query bir satır yukarıdaki sqlQuery alanından.
                        // Parametreleri ekle
                        command.Parameters.AddWithValue("@date1", p1);
                        command.Parameters.AddWithValue("@date2", p2);

                        // SqlDataAdapter kullanarak DataTable'ı doldurdum.
                        using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }

                }
                // DataTable'ı HTML olarak dönüştürür.
                string htmlTable = ConvertDataTableToHtml2(dataTable);

                // HTML tablosunu ViewBag'e atar.
                ViewBag.HtmlTable2 = htmlTable;
            }
       
            return View();
     
        }


        // Burada DataTable Html çevirir. kaynak https://stackoverflow.com/questions/19682996/datatable-to-html-table
        static string ConvertDataTableToHtml2(DataTable table)
        {
            StringBuilder html = new StringBuilder();

            // HTML tablosunun başlık kısmı oluşturur.
            html.Append("<div class='table-responsive'>");
            html.Append("<table id='example' class='table table-striped table-bordered dt-responsive nowrap' style='width:100%'>");
            html.Append("<thead><tr>");
            foreach (DataColumn column in table.Columns)
            {
                html.Append("<th>");
                html.Append(column.ColumnName);
                html.Append("</th>");
            }
            html.Append("</tr></thead>");

            // Verileri döngüyle HTML tablosuna ekler.
            html.Append("<tbody>");
            foreach (DataRow row in table.Rows)
            {
                html.Append("<tr>");
                foreach (DataColumn column in table.Columns)
                {
                    html.Append("<td>");
                    html.Append(row[column]);
                    html.Append("</td>");
                }
                html.Append("</tr>");
            }
            html.Append("</tbody>");

            // HTML tablosunun sonunu ekler.
            html.Append("</table>");
            html.Append("</div>");

            // datatables eklentisini etkinleştirir !! 
            html.Append("<script>");
            html.Append("$(document).ready(function() {");
            html.Append("$('#example').DataTable();");
            html.Append("});");
            html.Append("</script>");

            return html.ToString();
        }
        #endregion


        #region Grafik Sayfası
        public IActionResult GrafikIndex(string p1, string p2)  // Dışarıdan p1 ve p2 adında 2 tane tarih filtresi beklemektedir.
        {


            if (!string.IsNullOrEmpty(p1) && !string.IsNullOrEmpty(p2)) //Dışarıdan gelen p1 ve p2 boş veya null değil ise işlem gerçekleştirir.
            {
                DateTime startDate = DateTime.Parse(p1); // p1 ve p2 parametre değerlerini datetime türünde startdate ve enddate değişkenlerine çevirir.
                DateTime endDate = DateTime.Parse(p2);


                ViewBag.v1 = c.OrderHeaders.Where(x => x.OrderDateTime >= startDate && x.OrderDateTime <= endDate).Count(); //İlgili tarih aralığında Toplam Çek Sayısı

                ViewBag.v2 = c.OrderTransactions.Where(x => x.TransactionDateTime >= startDate && x.TransactionDateTime <= endDate).Distinct().Count(); //İlgili tarih aralığında Toplam Satılan Ürün

                ViewBag.v3 = c.OrderHeaders.Where(x => x.OrderDateTime >= startDate && x.OrderDateTime <= endDate).Sum(x => x.AmountDue); //İlgili tarih aralığında Toplam Ciro

                ViewBag.FormattedValue = ViewBag.v3.ToString("N2"); //İlgili tarih aralığında Toplam Ciro (burada n2 formatı kullanmak için mecburen bu şekilde bir yol izlemek durumunda kaldım)

                ViewBag.v4 = c.OrderHeaders.Where(x => x.OrderDateTime >= startDate && x.OrderDateTime <= endDate && x.LineDeleted == true).Count(); //İlgili tarih aralığında Toplam İptal Adisyon

                ViewBag.v5 = c.OrderHeaders.Where(x => x.OrderId >= 1 && x.OrderId <= 100).Sum(x => x.AmountDue);   //Dinamik bir yapı oluşturabilmek için orderıd aralığında verileri çekip listeledim
                ViewBag.v6 = c.OrderHeaders.Where(x => x.OrderId >= 100 && x.OrderId <= 200).Sum(x => x.AmountDue);
                ViewBag.v7 = c.OrderHeaders.Where(x => x.OrderId >= 300 && x.OrderId <= 400).Sum(x => x.AmountDue);
                ViewBag.v8 = c.OrderHeaders.Where(x => x.OrderId >= 500 && x.OrderId <= 600).Sum(x => x.AmountDue);
                ViewBag.v9 = c.OrderHeaders.Where(x => x.OrderId >= 700 && x.OrderId <= 800).Sum(x => x.AmountDue);
                ViewBag.v10 = c.OrderHeaders.Where(x => x.OrderId >= 900 && x.OrderId <= 1000).Sum(x => x.AmountDue);

                ViewBag.v11 = c.OrderTransactions.Where(x => x.TransactionDateTime >= startDate && x.TransactionDateTime <= endDate)  //İlgili tarih aralığında en çok satılan ürün tabloda quantity 
                                                 .GroupBy(x => x.MenuItemText)                                                        // olmadığı için bu şekilde yaklaşım sağladım.
                                                 .Select(g => new { MenuItemText = g.Key, TotalSales = g.Count() })
                                                 .OrderByDescending(x => x.TotalSales).Take(5).ToList();

                ViewBag.v12 = c.OrderTransactions.Where(x => x.TransactionDateTime >= startDate && x.TransactionDateTime <= endDate)  //İlgili tarih aralığında en az satılan ürün tabloda quantity 
                                                 .GroupBy(x => x.MenuItemText)                                                        // olmadığı için bu şekilde yaklaşım sağladım.
                                                 .Select(g => new { MenuItemText = g.Key, TotalSales = g.Count() })
                                                 .OrderBy(x => x.TotalSales).Take(5).ToList();
            }

            return View();
        }

        #endregion


        #region Dashboard Sayfası

        public async Task<IActionResult> DashboardIndex()
        {

            //Weather APi parametre olarak 2 değer ister city ve api statik olarak burada bir değişkende tuttum.
            string city = "istanbul";
            string api = "67db534b921c0d3eaffb2aefa96c322e";


            try // Dışarıdan ücretsiz bir api kullandığım için site tarafından verilen api key herhangi bir bloke yediğinde dashboard sayfası hataya düşmemesi için try catch komutunun içine aldım
            {
                string connection = ($"http://api.openweathermap.org/data/2.5/weather?q={city}&mode=xml&lang=tr&units=metric&appid={api}");  //Api İle hava durumu bilgisini çek.
                XDocument document = XDocument.Load(connection); // Xml verisi yüklemek için nesne türetip documente yolladım. Bu işlem XML belgesinin tüm yapısını ve içeriğini temsil eder.
                ViewBag.v5 = document.Descendants("temperature").ElementAt(0).Attribute("value").Value;  //Xml den gelen verideki ilgili alanın value değerini Viewbag ile taşı.
            }
            catch (Exception)
            {

                return View("Error");
            }

            //statistics
            ViewBag.v1 = c.OrderHeaders.Where(x => x.LineDeleted == true).Count().ToString("#,0");                 //Başarılı Adisyon Sayısı   ( "#,0" Bu Format Önrek 1.955.200 gibi gelmesini sağlar )
            ViewBag.v2 = c.OrderPayments.Where(x => x.PaymentMethodName != null).Count().ToString("#,0");          //Ödemesi Alınan Adisyon Sayısı
            ViewBag.v3 = c.OrderTransactions.Count().ToString("#,0");                                              //Toplam Ürün Satış Sayısı
            ViewBag.v4 = c.OrderHeaders.Where(x => x.LineDeleted == false).Count().ToString("#,0");                //İptal Olan Adisyon Sayısı
            ViewBag.v11 = c.OrderTransactions.OrderByDescending(x => x.ExtendedPrice).Select(y => y.MenuItemText).FirstOrDefault();                  //En Pahalı Ürün
            ViewBag.v12 = c.OrderTransactions.GroupBy(x => x.MenuItemText).OrderByDescending(g => g.Count()).Select(g => g.Key).FirstOrDefault();    //En Çok Satılan Ürün


            var values = c.OrderHeaders.Where(x => x.OrderStatus == 1).OrderByDescending(x => x.OrderDateTime).Take(6).ToList();   //Statu 1 Olan Son 6 Adisyonu Listele
            return View(values);
        }


        #endregion

    }
}
