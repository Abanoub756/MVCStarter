namespace Demo.PL.Helper
{
    public static class DocumentSettings
    {
        // Upload
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1) Build the folder path
            string folderPath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "Files",
                folderName
            );

            // Create folder if it does not exist (important)
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            // 2) Create unique file name
            string uniqueName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(file.FileName);
            string fileName = uniqueName + extension;

            // 3) Build full file path
            string filePath = Path.Combine(folderPath, fileName);

            // 4) Save file to disk
            using var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);

            // 5) Return file name only (to save in database)
            return fileName;
        }

        // Delete
        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "Files",
                folderName,
                fileName
            );

            if (File.Exists(filePath))
                File.Delete(filePath);
        }
    }
}
