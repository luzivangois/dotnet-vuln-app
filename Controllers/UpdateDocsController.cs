using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetVulnApp.Models;
using Microsoft.AspNetCore.Mvc;
using DotNetVulnApp.Utils;
using Microsoft.AspNetCore.Authorization;

namespace DotNetVulnApp.Controllers
{
    [Route("api/[controller]")]
    public class UpdateDocsController :ControllerBase
    {

        [HttpPost]
        [Authorize]
        public IActionResult PostNewDoc([FromBody] Docs doc){
            
            string defaultPathForDocs = Directory.GetCurrentDirectory();

            if(doc.Content == null || doc.FileName == null)
                return BadRequest("Missing mandatory parameters");

            doc.FileName = defaultPathForDocs + "/" + doc.FileName;

            try
            {
                OptimisedIO.saveFileRaw(doc.FileName, doc.Content);
                return Ok(new
                {
                message = "Doc was saved"
                }); 
            }
            catch(Exception ex)
            {
                return BadRequest("Error while save doc");
            }
        }

    [HttpGet]
    [Route("download/{doc}")]
    public async Task<IActionResult> getDoc(string doc){

        string receiptBasePath = Directory.GetCurrentDirectory();

        string receiptPath = doc + ".txt";

        if(!System.IO.File.Exists(receiptPath))
            return BadRequest("Doc não existe");

        string papersize = Request.Query["size"];


        string receiptPdfPath = "doc.pdf";
        System.Diagnostics.ProcessStartInfo procStartInfo;
        procStartInfo = new System.Diagnostics.ProcessStartInfo(
            "/bin/bash", 
            $"-c \"enscript {receiptPath} -o - | ps2pdf -dFIXEDMEDIA -sPAPERSIZE={papersize} - {receiptPdfPath}\""
        );
        

        procStartInfo.UseShellExecute = false;
        procStartInfo.CreateNoWindow = true;
        procStartInfo.RedirectStandardError = true;
        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo = procStartInfo;
        proc.Start();
        proc.WaitForExit();


        string localPdfPath = System.IO.Path.Combine(
            Directory.GetCurrentDirectory(),
            receiptPdfPath
        );

        return PhysicalFile(localPdfPath, "application/pdf");
    }


    }
}