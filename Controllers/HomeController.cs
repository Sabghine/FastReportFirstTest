using FastReport;
using FastReport.Barcode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FastReportTutorial.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        public FileResult Generate()
        {
            FastReport.Utils.Config.WebMode = true;
            Report rep = new Report();
            string path = Server.MapPath("~/Assure.frx");
            rep.Load(path);

            List<Models.Assure> assu = new List<Models.Assure>();
            assu.Add(new Models.Assure() { FirstName = "Sabrine", LastName = "Mokhtar", ContactNo = "26987452" });
            //assu.Add(new Models.Assure() { FirstName = "Ahmed" , LastName = "Mezghani" , ContactNo = "26987452"});
           // assu.Add(new Models.Assure() { FirstName = "Amine" , LastName = "Douiri" , ContactNo = "26987452"});

            rep.RegisterData(assu, "AssureRef");

            if (rep.Report.Prepare())
            {
                FastReport.Export.PdfSimple.PDFSimpleExport pdfExport = new FastReport.Export.PdfSimple.PDFSimpleExport();
                pdfExport.ShowProgress = false;
                pdfExport.Subject = "Subject Report";
                pdfExport.Title = "Report Title";
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                rep.Report.Export(pdfExport, ms);
                rep.Dispose();
                ms.Position = 0;

                return File(ms, "application/pdf", "Adhésion - Demande de pièces_à_la_souscription.pdf");
            }
            else
            {
                return null;
            }
        }
    }
}