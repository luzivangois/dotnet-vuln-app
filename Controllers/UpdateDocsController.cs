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
        public async Task<IActionResult> PostNewDoc(IFormFile file, string namefile){
            
            string defaultPathForDocs = Directory.GetCurrentDirectory();

            if(file == null || string.IsNullOrEmpty(namefile))
                return BadRequest("Missing mandatory parameters");

            string filePath = defaultPathForDocs + "/" + namefile;

            try
            {
                byte[] fileContent;
                using (var memoryStream = new MemoryStream())
                {
                    await file.CopyToAsync(memoryStream);
                    fileContent = memoryStream.ToArray();
                }

                OptimisedIO.saveFileRaw(filePath, fileContent);
                string fileId = Guid.NewGuid().ToString();
                
                string mappingPath = Path.Combine(Directory.GetCurrentDirectory(), "file_mappings.txt");
                string mapping = $"{fileId}|{namefile}\n";
                System.IO.File.AppendAllText(mappingPath, mapping);
                
                return Ok(new
                {
                message = "Doc was saved",
                id = fileId
                }); 
            }
            catch(Exception ex)
            {
                return BadRequest("Error while save doc");
            }
        }

    [HttpPost]
    [Route("download")]
    public async Task<IActionResult> getDoc([FromForm] string filename, [FromForm] string size){

        string receiptBasePath = Directory.GetCurrentDirectory();

        if(string.IsNullOrEmpty(filename))
            return BadRequest("Missing filename parameter");

        string receiptPath = filename;

        string papersize = size;
        
        if(string.IsNullOrEmpty(papersize))
            papersize = "a4";

        string receiptPdfPath = "doc.pdf";
        System.Diagnostics.ProcessStartInfo procStartInfo;
        
        string command = $"enscript {receiptPath} -o - | ps2pdf -dFIXEDMEDIA -sPAPERSIZE={papersize} - {receiptPdfPath}";
        procStartInfo = new System.Diagnostics.ProcessStartInfo(
            "/bin/bash", 
            $"-c \"{command}\""
        );
        

        procStartInfo.UseShellExecute = false;
        procStartInfo.CreateNoWindow = true;
        procStartInfo.RedirectStandardError = true;
        procStartInfo.RedirectStandardOutput = true;
        System.Diagnostics.Process proc = new System.Diagnostics.Process();
        proc.StartInfo = procStartInfo;
        proc.Start();
        
        string output = await proc.StandardOutput.ReadToEndAsync();
        string error = await proc.StandardError.ReadToEndAsync();
        proc.WaitForExit();

        string localPdfPath = System.IO.Path.Combine(
            Directory.GetCurrentDirectory(),
            receiptPdfPath
        );

        if(!System.IO.File.Exists(localPdfPath))
        {
            string fileExtension = Path.GetExtension(filename).ToLower();
            if(fileExtension == ".pdf")
            {
                string fullPath = Path.Combine(Directory.GetCurrentDirectory(), filename);
                if(System.IO.File.Exists(fullPath))
                {
                    return PhysicalFile(fullPath, "application/pdf");
                }
            }
            return BadRequest("Erro ao gerar PDF");
        }

        return PhysicalFile(localPdfPath, "application/pdf");
    }

    }
}