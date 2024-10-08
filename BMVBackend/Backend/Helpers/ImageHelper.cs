using Microsoft.Extensions.FileProviders;

namespace Backend.Helpers
{
    public class ImageHelper
    {
        public string storeImage(IFormFile file)
        {


            var fileName = Path.GetFileName(file.FileName);


            var myUniqueFileName = Convert.ToString(Guid.NewGuid());


            var fileExtension = Path.GetExtension(fileName);


            var newFileName = String.Concat(myUniqueFileName, fileExtension);


            var filepath = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images")).Root + $@"\{newFileName}";

            using (FileStream fs = System.IO.File.Create(filepath))
            {
                file.CopyTo(fs);

                fs.Flush();
            }
            return (@"http://localhost:5059/images/" + newFileName);

        }
    }
}
