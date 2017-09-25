using System.Web.Mvc;
using TExperiment.Excel.Exports;

namespace TExperiment.Web.Controllers
{
    public class ExcelController : Controller
    {
        // GET: Excel
        public ActionResult Index()
        {
            return File(ExcelExport.ExportToBuffer(Server.MapPath("~/App_Data/Templates/template.xlsx"), new object()), "application/vnd.ms-excel", "test.xlsx");
        }
    }
}