using System.Web.Mvc;
using TExperiment.Excel.Loaders;

namespace TExperiment.Web.Controllers
{
    public class ExcelController : Controller
    {
        // GET: Excel
        public ActionResult Index()
        {
            var loader = new TemplateRenderLoader();
            var desc = loader.Load(Server.MapPath("~/App_Data/Templates/template.xlsx"));
            return View(desc);
        }
    }
}