using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using testFileDownload.Models;

namespace testFileDownload.Controllers
{
  public class HomeController : Controller
  {
    public ActionResult Index()
    {
      ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

      return View();
    }

    public ActionResult About()
    {
      ViewBag.Message = "Your app description page.";

      return View();
    }

    public ActionResult Contact()
    {
      ViewBag.Message = "Your contact page.";

      return View();
    }

    public void Download() 
    {
      try
      {
        // **************************************************
        Response.Buffer = false;

        // Setting the unknown [ContentType]
        // will display the saving dialog for the user
        Response.ContentType = "application/octet-stream";

        // With setting the file name,
        // in the saving dialog, user will see
        // the [strFileName] name instead of [download]!
        Response.AddHeader("Content-Disposition", "attachment; filename=" + "123");

        long lngFileLength = 5000000000;

        // Notify user (client) the total file length
        Response.AddHeader("Content-Length", lngFileLength.ToString());
        // **************************************************

        // Total bytes that should be read
        long lngDataToRead = 5000000000;

        // Read the bytes of file
        while (lngDataToRead > 0)
        {
          // The below code is just for testing! So we commented it!
          //System.Threading.Thread.Sleep(200);
          var data = this.Create();
          var stream = new MemoryStream();
          var strWriter = new StreamWriter(stream);
          var conf = new CsvConfiguration();
          conf.BufferSize = 4096;
          CsvWriter writer = new CsvWriter(strWriter, conf);
          writer.WriteRecords(data);
          strWriter.Flush();
          stream.Position = 0;
          //return File(stream, "text/csv", "123.csv");

          // Open the file
          var oStream = stream;

          // Verify that the client is connected or not?
          if (Response.IsClientConnected)
          {
            // 8KB
            int intBufferSize = 8 * 1024;

            // Create buffer for reading [intBufferSize] bytes from file
            byte[] bytBuffers =
                new System.Byte[intBufferSize];

            // Read the data and put it in the buffer.
            int intTheBytesThatReallyHasBeenReadFromTheStream =
                oStream.Read(buffer: bytBuffers, offset: 0, count: intBufferSize);

            // Write the data from buffer to the current output stream.
            Response.OutputStream.Write
                (buffer: bytBuffers, offset: 0,
                count: intTheBytesThatReallyHasBeenReadFromTheStream);

            // Flush (Send) the data to output
            // (Don't buffer in server's RAM!)
            Response.Flush();

            lngDataToRead =
                lngDataToRead - intTheBytesThatReallyHasBeenReadFromTheStream;
          }
          else
          {
            // Prevent infinite loop if user disconnected!
            lngDataToRead = -1;
          }
        }
      }
      catch { }
      finally
      {
        Response.Close();
      }
    }

    private IEnumerable<Tets1> Create() 
    {
      for (int i = 0; i < 100; i++) 
      {
        yield return new Tets1 { Name = "Name" + i, LastName = "LN" + i, Time = new TimeSpan(6, 5, 8) };
      }
    }
  }
}
