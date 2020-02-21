using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using iText.Html2pdf;
using iText.Kernel.Pdf;

namespace HTMLtoPDF.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PDFitController : ControllerBase
    {

        private readonly ILogger<PDFitController> _logger;

        public PDFitController(ILogger<PDFitController> logger)
        {
            _logger = logger;
        }

        public string HtmlString
        {
            get;
            set;
        }


        /// <summary>
        /// Creates a PDF File.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /GetPdfFromHTML
        ///     {
        ///        "string" : "<HTML> Content</HTML>"
        ///     }
        ///
        /// </remarks>
        /// <param name="htmlString"></param>
        /// <returns>A PDF File</returns>
        /// <response code="201">Returns the PDF File</response>
        /// <response code="400">If the item is null</response>       
        [HttpPost]        
        public async Task<IActionResult> Post()
        {
            HtmlString = Request.Form[nameof(HtmlString)];

            byte[] pdfBytes;
            using (var stream = new MemoryStream())
            using (var wri = new PdfWriter(stream))
            using (var pdf = new PdfDocument(wri))
            {
                ConverterProperties converterProperties = new ConverterProperties();
                HtmlConverter.ConvertToPdf(HtmlString, pdf, converterProperties);
                pdfBytes = stream.ToArray();
            }

            return await Task.FromResult(new FileContentResult(pdfBytes, "application/pdf"));
        }
    }
}
