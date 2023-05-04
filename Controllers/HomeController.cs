using FastReport;
using FastReport.Barcode;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
            string path = Server.MapPath("~/A.frx");
            rep.Load(path);

            List<Models.Assure> assu = new List<Models.Assure>();
            assu.Add(new Models.Assure() { FirstName = "Sabrine", LastName = "Mokhtar", ContactNo = "26987452" });
            //assu.Add(new Models.Assure() { FirstName = "Ahmed" , LastName = "Mezghani" , ContactNo = "26987452"});
            //assu.Add(new Models.Assure() { FirstName = "Amine" , LastName = "Douiri" , ContactNo = "26987452"});

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
                // Save PDF to SQL database
                byte[] pdfData = ms.ToArray();
                string connectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=EmailEditor;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand("INSERT INTO Report (PDFData) VALUES (@PDFData)", connection);
                    command.Parameters.AddWithValue("@PDFData", pdfData);
                    connection.Open();
                    command.ExecuteNonQuery();
                }

                // Return the PDF file
                return File(ms, "application/pdf", "Adhésion - Demande de pièces_à_la_souscription.pdf");
            }
            else
            {
                return null;
            }
        }

        public FileResult GetReport(int reportId)
        {
            string connectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=EmailEditor;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand("SELECT PDFData FROM Report WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", reportId);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    byte[] pdfData = (byte[])reader["PDFData"];
                    return File(pdfData, "application/pdf", "Report.pdf");
                }
                else
                {
                    return null;
                }
            }
        }

        public FileResult ViewReport(int reportId)
        {
            return GetReport(reportId);
        }


        // public ActionResult GetReportsFromDatabase()
        // {
        //List<FileResult> reports = new List<FileResult>();
        //string connectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;Initial Catalog=EmailEditor;Integrated Security=True";
        // using (SqlConnection connection = new SqlConnection(connectionString))
        // {
        //  SqlCommand command = new SqlCommand("SELECT PDFData FROM Report", connection);
        // connection.Open();
        //SqlDataReader reader = command.ExecuteReader();
        //while (reader.Read())
        // {
        //  byte[] pdfData = (byte[])reader["PDFData"];
        // MemoryStream ms = new MemoryStream(pdfData);
        //reports.Add(new FileContentResult(ms.ToArray(), "application/pdf"));
        // }
        //}
        //return View(reports);
        //}


    }
}
    