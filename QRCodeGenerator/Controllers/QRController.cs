using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QRCodeGenerater.Models;
using QRCoder;
using System.Drawing;

namespace QRCodeGenerater.Controllers
{
    public class QRController : Controller
    {

        private readonly AppDBContext _context;
        public QRController(AppDBContext context)
        {
            _context = context;
        }


        public IActionResult Index(int? id)
        {
            string data="";

            if (id.HasValue)
            {
                // Retrieve the specific item based on the provided ID
                Models.QRCodeData result = _context.QRCodeData.FirstOrDefault(item => item.Id == id);
                if (result != null)
                {
                    data = $"{result.FirstName} - {result.LastName}- {result.Email}- {result.Mobile}";
                }
                //string resultString = $"{result.FirstName} {result.Last}";
            }
            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCoder.QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
            var qrCode = new QRCoder.QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            // Convert Bitmap to byte array for display in Razor view
            using (MemoryStream stream = new MemoryStream())
            {
                qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                byte[] qrCodeBytes = stream.ToArray();

                return File(qrCodeBytes, "image/jpg");
            }
            return View();
        }
        [HttpPost]
        public  ActionResult Create(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                Models.QRCodeData user = new Models.QRCodeData
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Mobile = model.Mobile

                };
               
                    _context.Add(user);
                    var affectedRecords = _context.SaveChanges();
                var insertedItem = _context.QRCodeData.OrderBy(item => item.Id).LastOrDefault();
                return RedirectToAction("Index", new { id = insertedItem?.Id });

            }
            else
            {
            

                return View(model);
            }
            
        }

        public IActionResult Create()
        {
            return View();
        }
    }
}
