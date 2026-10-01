namespace CoreBlog.Infrastructure
{
    /// <summary>Speichert hochgeladene Bilder sicher unter wwwroot/uploads.</summary>
    public class ImageUploader
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        private const long MaxBytes = 5 * 1024 * 1024;

        private readonly IWebHostEnvironment _env;
        public ImageUploader(IWebHostEnvironment env) => _env = env;

        /// <summary>Gibt den öffentlichen Pfad zurück oder eine Fehlermeldung.</summary>
        public (string Path, string Error) Save(IFormFile file, string folder)
        {
            if (file == null || file.Length == 0) return (null, null);
            if (file.Length > MaxBytes) return (null, "Das Bild darf höchstens 5 MB groß sein.");

            var ext = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext) || !file.ContentType.StartsWith("image/"))
                return (null, "Bitte laden Sie ein Bild im Format JPG, PNG, WEBP oder GIF hoch.");

            var dir = System.IO.Path.Combine(_env.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(dir);
            var name = $"{Guid.NewGuid():N}{ext}";
            using (var stream = new FileStream(System.IO.Path.Combine(dir, name), FileMode.Create))
                file.CopyTo(stream);

            return ($"/uploads/{folder}/{name}", null);
        }
    }
}
