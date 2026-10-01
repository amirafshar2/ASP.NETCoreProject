namespace CoreBlog.Infrastructure
{
    /// <summary>Einstellungen für den öffentlichen Demo-Modus (appsettings.json → "Demo").</summary>
    public class DemoOptions
    {
        public bool Enabled { get; set; } = true;
        /// <summary>Datenbank bei jedem Start neu aufbauen (für Hosting).</summary>
        public bool ResetOnStartup { get; set; }
        /// <summary>Automatischer Reset der Demo-Daten (0 = aus).</summary>
        public int ResetIntervalHours { get; set; } = 6;
        public string AdminEmail { get; set; } = "admin@coreblog.demo";
        public string WriterEmail { get; set; } = "autorin@coreblog.demo";
        public string Password { get; set; } = "Demo123!";
        public string PortfolioUrl { get; set; } = "https://amirrezaafshar.de";
        public string ImpressumUrl { get; set; } = "https://amirrezaafshar.de/Impressum";
        public string DatenschutzUrl { get; set; } = "https://amirrezaafshar.de/Datenschutz";
        public string GitHubUrl { get; set; } = "https://github.com/amirafshar2/ASP.NETCoreProject";

        public bool IsDemoAccount(string email) =>
            Enabled && email != null &&
            (email.Equals(AdminEmail, StringComparison.OrdinalIgnoreCase) ||
             email.Equals(WriterEmail, StringComparison.OrdinalIgnoreCase));
    }
}
