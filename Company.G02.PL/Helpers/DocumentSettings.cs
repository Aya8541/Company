namespace Company.G02.PL.Helpers
{
    public static class DocumentSettings
    {
        //Upload 
        //ImageName
        public static string UploadFile(IFormFile file , string folderName)
        {
            // 1. Get Folder Location

            //string folderPath = "D:\\.net\\07 MVC\\Session 03\\Company.G02\\Company.G02.PL\\wwwroot\\files\\" + folderName ;

            //var folderPath = Directory.GetCurrentDirectory()+ "\\wwwroot\\files\\" + folderName;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);

            // 2. Get File Name and make it unique

            var fileName = $"{Guid.NewGuid()}{ file.FileName}";

            //File Path
            var filePath = Path.Combine(folderPath, fileName);

            using var filesteam = new FileStream(filePath, FileMode.Create);

            file.CopyTo(filesteam);
            
            return fileName;
        }

        //Delete
        public static void DeleteFile(string fileName, string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName , fileName);
                        
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }  
    }
}
