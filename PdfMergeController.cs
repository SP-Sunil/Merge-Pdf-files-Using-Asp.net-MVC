using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace TesPdfMerge.Controllers
{
    public class PdfMergeController : Controller
    {
        // GET: PdfMerge
        #region FileUpload
        public ActionResult AttachPdf()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AttachPdf(HttpPostedFileBase file)
        {
            try
            {
                if (file.ContentLength > 0)
                {
                    string _FileName = System.IO.Path.GetFileName(file.FileName);
                    string _path = System.IO.Path.Combine(Server.MapPath("~/App_Data/SourcePdfFiles"), _FileName);
                    file.SaveAs(_path);
                }
                ViewBag.Message = "PDF file uploaded successfully !!";
                return View();
            }
            catch
            {
                ViewBag.Message1 = "File uploading has failed. Please select PDF file !!";
                return View();
            }
        }
        #endregion

        #region PdfMerge
        [HttpGet]
        public ActionResult MergePdf()
        {
            return View();
        }
        [HttpPost]
        public ActionResult MergePdf(HttpPostedFileBase file)
        {
            try
            {
                string SourcePdfPath = Server.MapPath("~/App_Data/SourcePdfFiles/");
                string[] filenames = System.IO.Directory.GetFiles(SourcePdfPath);
                string outputFileName = "Combine.pdf";
                string outputPath = Server.MapPath("~/App_Data/DestPdfFile/" + outputFileName);
                Document doc = new Document();
                PdfCopy writer = new PdfCopy(doc, new FileStream(outputPath, FileMode.Create));
                if (writer == null)
                {
                    return View();
                }
                doc.Open();
                foreach (string filename in filenames)
                {
                    PdfReader reader = new PdfReader(filename);
                    reader.ConsolidateNamedDestinations();
                    for (int i = 1; i <= reader.NumberOfPages; i++)
                    {
                        PdfImportedPage page = writer.GetImportedPage(reader, i);
                        writer.AddPage(page);
                    }
                    reader.Close();
                }
                writer.Close();
                doc.Close();
                ViewBag.Message = "PDF files have been successfully merged. Please check in DestPdfFile folder !!";
                return View();
            }
            catch
            {
                ViewBag.Message1 = "You are trying to merge PDF files. Please select the files in the correct format !!";
                return View();
            }
        } 
        #endregion
    }
}
