using BE.Concrete;
using DAL.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace CoreBlog.Infrastructure
{
    /// <summary>Spielt Beispieldaten in eine leere Datenbank ein (Rollen, Konten, Blogs, Kommentare …).</summary>
    public class DataSeeder
    {
        public const string AdminRole = "Admin";
        public const string WriterRole = "Writer";

        private readonly Context _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly DemoOptions _demo;

        public DataSeeder(Context context, UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager, IOptions<DemoOptions> demo)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _demo = demo.Value;
        }

        public async Task SeedAsync()
        {
            foreach (var role in new[] { AdminRole, WriterRole })
                if (!await _roleManager.RoleExistsAsync(role))
                    await _roleManager.CreateAsync(new AppRole(role));

            if (_context.Blogs.Any()) return;

            var today = DateTime.Today;

            // ---------- Kategorien ----------
            var categories = new List<Category>
            {
                new() { Name = "Softwareentwicklung", Color = "#2F5BEA", Description = "Architektur, sauberer Code und Werkzeuge für den Entwickleralltag." },
                new() { Name = "Produktivität", Color = "#1F8A5B", Description = "Methoden für konzentriertes Arbeiten und gute Gewohnheiten." },
                new() { Name = "Design", Color = "#B03A82", Description = "Gestaltung von Oberflächen, Typografie und Benutzerfreundlichkeit." },
                new() { Name = "Karriere", Color = "#C46A12", Description = "Einstieg, Bewerbung und Weiterentwicklung in der IT." },
                new() { Name = "Remote Work", Color = "#0F7C86", Description = "Zusammenarbeit im verteilten Team und gesundes Homeoffice." },
                new() { Name = "Archiv", Color = "#6B7489", Description = "Ältere Beiträge, die derzeit nicht angezeigt werden.", Status = false }
            };
            _context.Categories.AddRange(categories);
            await _context.SaveChangesAsync();

            // ---------- Konten & Autoren ----------
            var people = new[]
            {
                (Name: "Daniel Krüger", Mail: _demo.AdminEmail, Image: "/img/avatars/a3.jpg", Admin: true,
                 About: "Chefredakteur von CoreBlog. Schreibt über Softwarearchitektur, .NET und den Einstieg in die IT."),
                (Name: "Lena Hoffmann", Mail: _demo.WriterEmail, Image: "/img/avatars/a2.jpg", Admin: false,
                 About: "Produktivitäts-Coach und Frontend-Entwicklerin. Mag gute Notizbücher und klare To-do-Listen."),
                (Name: "Jonas Weber", Mail: "jonas.weber@coreblog.demo", Image: "/img/avatars/a1.jpg", Admin: false,
                 About: "Backend-Entwickler mit Vorliebe für Git, Tests und lesbaren Code."),
                (Name: "Sara Neumann", Mail: "sara.neumann@coreblog.demo", Image: "/img/avatars/a4.jpg", Admin: false,
                 About: "UX-Designerin. Glaubt, dass gute Gestaltung vor allem aus Weglassen besteht.")
            };

            var writers = new Dictionary<string, Writer>();
            foreach (var p in people)
            {
                var user = new AppUser
                {
                    UserName = p.Mail, Email = p.Mail, EmailConfirmed = true,
                    FullName = p.Name, ImageUrl = p.Image, CreatedAt = today.AddDays(-120)
                };
                var result = await _userManager.CreateAsync(user, _demo.Password);
                if (!result.Succeeded)
                    throw new InvalidOperationException(string.Join(", ", result.Errors.Select(e => e.Description)));

                await _userManager.AddToRoleAsync(user, WriterRole);
                if (p.Admin) await _userManager.AddToRoleAsync(user, AdminRole);

                var writer = new Writer
                {
                    Name = p.Name, Mail = p.Mail, Image = p.Image, About = p.About,
                    Status = true, AppUserId = user.Id, CreatedAt = today.AddDays(-120)
                };
                _context.Writers.Add(writer);
                writers[p.Name] = writer;
            }
            await _context.SaveChangesAsync();

            // ---------- Blogs ----------
            var blogs = new Dictionary<string, Blog>();
            foreach (var a in SeedArticles.All)
            {
                var blog = new Blog
                {
                    Title = a.Title, Summary = a.Summary, Content = a.Content.Trim(), Image = a.Image,
                    CreateDate = today.AddDays(-a.DaysAgo).AddHours(9 + a.DaysAgo % 8),
                    Status = true, IsFeatured = a.Featured, ViewCount = a.Views,
                    Category = categories.First(c => c.Name == a.Category),
                    Writer = writers[a.Writer],
                    Rating = new BlogRating()
                };
                _context.Blogs.Add(blog);
                blogs[a.Key] = blog;
            }
            // Entwurf und archivierter Beitrag, damit die Statusfunktionen sichtbar sind
            _context.Blogs.Add(new Blog
            {
                Title = "Entwurf: Unit-Tests mit xUnit für Einsteiger",
                Summary = "Ein Entwurf über die ersten Tests in einem ASP.NET-Core-Projekt.",
                Content = "Dieser Beitrag ist noch nicht veröffentlicht. Er zeigt, wie Entwürfe im Autorenbereich verwaltet werden: Erst wenn der Status auf „veröffentlicht“ steht, erscheint der Artikel auf der Startseite.\n\n## Geplanter Inhalt\n\nAufbau eines Testprojekts, das Arrange-Act-Assert-Muster und Fake-Repositories mit Dependency Injection.",
                Image = "/img/blog/8.jpg", CreateDate = today.AddDays(-1), Status = false,
                Category = categories[0], Writer = writers["Lena Hoffmann"], Rating = new BlogRating()
            });
            _context.Blogs.Add(new Blog
            {
                Title = "Rückblick: Unser erstes Jahr mit CoreBlog",
                Summary = "Ein älterer Beitrag in der ausgeblendeten Kategorie Archiv.",
                Content = "Dieser Beitrag liegt in der Kategorie „Archiv“, die im Admin-Panel deaktiviert wurde. Deshalb ist er öffentlich nicht sichtbar, bleibt aber in der Verwaltung erhalten.\n\n## Warum archivieren?\n\nInhalte zu archivieren statt zu löschen, erhält Statistiken und Kommentare und erlaubt eine spätere Wiederveröffentlichung.",
                Image = "/img/blog/6.jpg", CreateDate = today.AddDays(-150), Status = true, ViewCount = 210,
                Category = categories[5], Writer = writers["Daniel Krüger"], Rating = new BlogRating()
            });
            await _context.SaveChangesAsync();

            // ---------- Kommentare ----------
            var comments = new (string Blog, string Name, string Title, string Text, int Score, int DaysAgo, bool Approved)[]
            {
                ("clean-code", "Markus B.", "Sehr praxisnah", "Die Pfadfinderregel wende ich seit einem Jahr an – unsere Codebasis ist spürbar besser geworden.", 5, 2, true),
                ("clean-code", "Elif T.", "Frühe Rückgaben", "Gerade der Abschnitt zu frühen Rückgaben hat mir geholfen. Meine Methoden sind jetzt viel flacher.", 5, 1, true),
                ("clean-code", "Thomas", "Gute Zusammenfassung", "Kurz und verständlich. Würde mir noch ein Beispiel in C# wünschen.", 4, 1, true),
                ("git-workflow", "Anna K.", "Endlich verständlich", "Wir sind zu dritt und haben genau dieses Chaos gehabt. Danke für die klare Anleitung!", 5, 15, true),
                ("git-workflow", "Kevin", "Commit-Nachrichten", "Der Tipp mit dem Imperativ ist Gold wert.", 4, 10, true),
                ("dependency-injection", "Sophie R.", "Endlich verstanden", "Ich habe DI dreimal in Tutorials gesehen, aber erst hier wirklich verstanden, warum Scoped wichtig ist.", 5, 38, true),
                ("dependency-injection", "Lukas", "Frage zu Singleton", "Kann man den DbContext auch als Singleton registrieren? Oder führt das zu Problemen?", 4, 30, true),
                ("deep-work", "Miriam", "Fokuszeiten", "Seit ich Fokuszeiten blocke, schaffe ich vormittags mehr als früher am ganzen Tag.", 5, 5, true),
                ("deep-work", "Paul H.", "Schwierig im Großraumbüro", "Klingt gut, ist bei uns im Großraumbüro aber schwer umzusetzen. Kopfhörer helfen etwas.", 3, 4, true),
                ("aufschieben", "Nina", "Fünf-Minuten-Start", "Der Fünf-Minuten-Start funktioniert bei mir erstaunlich gut.", 4, 20, true),
                ("weissraum", "Jan", "Schöner Artikel", "Die Abstandsskala übernehmen wir direkt in unser Designsystem.", 5, 10, true),
                ("einstieg-it", "Ahmad S.", "Motivierend", "Ich bin selbst in der Umschulung – dieser Artikel macht Mut. Danke!", 5, 7, true),
                ("einstieg-it", "Christina", "Fragen stellen", "Der Punkt mit den 30 Minuten ist wichtig. Das hätte ich am Anfang gebraucht.", 5, 6, true),
                ("portfolio", "Felix", "Demo-Zugang", "Guter Hinweis mit dem Demo-Zugang ohne Registrierung. Das baue ich in mein Projekt ein.", 5, 28, true),
                ("homeoffice", "Sandra", "Laptopständer", "Ein günstiger Laptopständer hat meine Nackenschmerzen tatsächlich beendet.", 4, 40, true),
                ("async", "Ole", "Asynchron arbeiten", "Bei uns funktionieren Entscheidungsprotokolle super. Gute Tipps!", 4, 70, true),
                ("typografie", "Gast123", "Werbung", "Jetzt günstig Follower kaufen!!! Link in meinem Profil.", 1, 2, false),
                ("deep-work", "Robert", "Kurze Rückfrage", "Gibt es eine Empfehlung für Apps, die Benachrichtigungen bündeln?", 4, 0, false)
            };
            foreach (var c in comments)
            {
                _context.Comments.Add(new Comment
                {
                    Blog = blogs[c.Blog], UserName = c.Name, Title = c.Title, Content = c.Text, Score = c.Score,
                    Email = c.Name.Split(' ')[0].ToLowerInvariant() + "@example.com",
                    Date = today.AddDays(-c.DaysAgo).AddHours(14), Status = c.Approved
                });
            }
            await _context.SaveChangesAsync();

            // Bewertungen aus freigeschalteten Kommentaren berechnen
            foreach (var blog in _context.Blogs.ToList())
            {
                var approved = _context.Comments.Where(c => c.BlogId == blog.Id && c.Status).ToList();
                var rating = _context.BlogRatings.First(r => r.BlogId == blog.Id);
                rating.TotalScore = approved.Sum(c => c.Score);
                rating.RatingCount = approved.Count;
            }

            // ---------- Nachrichten ----------
            var d = writers["Daniel Krüger"]; var l = writers["Lena Hoffmann"];
            var j = writers["Jonas Weber"]; var s = writers["Sara Neumann"];
            _context.Messages.AddRange(
                new Message { Sender = d, Receiver = l, Subject = "Themenplanung November", Details = "Hallo Lena,\n\nkannst du für November zwei Beiträge zum Thema Produktivität übernehmen? Gern etwas zu Zeitplanung im Homeoffice.\n\nViele Grüße\nDaniel", Date = today.AddDays(-1).AddHours(10), IsRead = false },
                new Message { Sender = s, Receiver = l, Subject = "Bilder für deinen Artikel", Details = "Hi Lena, ich habe dir drei Bildvorschläge für den Deep-Work-Artikel herausgesucht. Sag mir, welcher dir am besten gefällt!", Date = today.AddDays(-2).AddHours(15), IsRead = false },
                new Message { Sender = j, Receiver = l, Subject = "Review deines Entwurfs", Details = "Dein Entwurf zu xUnit liest sich gut. Ich würde noch ein Beispiel mit einem Fake-Repository ergänzen.", Date = today.AddDays(-4).AddHours(11), IsRead = true },
                new Message { Sender = l, Receiver = d, Subject = "Re: Themenplanung", Details = "Gern! Ich schicke dir bis Freitag die Gliederung.", Date = today.AddDays(-1).AddHours(12), IsRead = false },
                new Message { Sender = l, Receiver = s, Subject = "Danke für die Bilder", Details = "Das zweite Bild passt perfekt. Danke dir!", Date = today.AddDays(-2).AddHours(16), IsRead = true },
                new Message { Sender = j, Receiver = d, Subject = "Neue Kategorie?", Details = "Sollen wir eine Kategorie „Testing“ einführen? Ich hätte schon drei Ideen für Beiträge.", Date = today.AddDays(-3).AddHours(9), IsRead = false },
                new Message { Sender = s, Receiver = d, Subject = "Neues Logo", Details = "Ich habe zwei Logo-Varianten vorbereitet. Wann passt dir ein kurzer Blick darauf?", Date = today.AddDays(-6).AddHours(14), IsRead = true });

            // ---------- Hinweise für Autoren ----------
            _context.Notifications.AddRange(
                new Notification { Type = "Willkommen in der Demo", TypeSymbol = "fa-solid fa-hand-sparkles", SymbolColor = "#2F5BEA", Details = "Probieren Sie alles aus: Beiträge schreiben, Nachrichten senden, Profil bearbeiten. Die Daten werden regelmäßig zurückgesetzt.", Date = today, Status = true },
                new Notification { Type = "Neue Kategorie Remote Work", TypeSymbol = "fa-solid fa-laptop-house", SymbolColor = "#0F7C86", Details = "Ab sofort gibt es die Kategorie Remote Work für Beiträge rund um verteilte Teams.", Date = today.AddDays(-5), Status = true },
                new Notification { Type = "Redaktionsschluss", TypeSymbol = "fa-solid fa-calendar-check", SymbolColor = "#C46A12", Details = "Beiträge für den Newsletter bitte bis Donnerstag, 12 Uhr, veröffentlichen.", Date = today.AddDays(-9), Status = true },
                new Notification { Type = "Bildrechte beachten", TypeSymbol = "fa-solid fa-image", SymbolColor = "#B03A82", Details = "Bitte nur Bilder verwenden, für die eine Nutzungslizenz vorliegt.", Date = today.AddDays(-20), Status = true },
                new Notification { Type = "Wartung abgeschlossen", TypeSymbol = "fa-solid fa-screwdriver-wrench", SymbolColor = "#6B7489", Details = "Die geplante Wartung wurde erfolgreich abgeschlossen.", Date = today.AddDays(-40), Status = false });

            // ---------- Kontaktanfragen & Newsletter ----------
            _context.Contacts.AddRange(
                new Contact { UserName = "Katharina Vogel", Mail = "k.vogel@example.com", Subject = "Gastbeitrag", Message = "Hallo CoreBlog-Team, ich würde gern einen Gastbeitrag über barrierefreie Formulare schreiben. Ist das möglich?", Date = today.AddDays(-1).AddHours(8), IsRead = false },
                new Contact { UserName = "Peter Schulz", Mail = "peter.schulz@example.com", Subject = "Fehler auf der Seite", Message = "Auf dem Smartphone wird bei mir die Suche im Querformat abgeschnitten.", Date = today.AddDays(-3).AddHours(17), IsRead = false },
                new Contact { UserName = "Aylin Demir", Mail = "aylin@example.com", Subject = "Lob", Message = "Ich lese den Blog jede Woche – besonders die Karriere-Artikel helfen mir sehr. Weiter so!", Date = today.AddDays(-8).AddHours(12), IsRead = true });

            _context.NewsLetters.AddRange(
                new[] { "lea.m@example.com", "tobias@example.com", "info@agentur-beispiel.de", "mara.k@example.com", "dev.ali@example.com", "nora@example.com" }
                .Select((m, i) => new NewsLetter { Mail = m, Status = true, Date = today.AddDays(-i * 9) }));

            // ---------- Über uns ----------
            _context.Abouts.Add(new About
            {
                Title = "Ein Blog für Menschen, die Software bauen",
                Details1 = "CoreBlog ist ein Magazin für Entwicklerinnen und Entwickler, Designerinnen und alle, die in der IT neu anfangen. Wir schreiben über sauberen Code, gute Gestaltung und die kleinen Gewohnheiten, die den Arbeitsalltag leichter machen.",
                Details2 = "Unsere Autorinnen und Autoren kommen aus der Praxis: aus Agenturen, Produktteams und Umschulungen. Jeder Beitrag wird von der Redaktion gegengelesen und von unseren Leserinnen und Lesern bewertet.",
                Image1 = "/img/blog/banner5.jpg",
                Image2 = "/img/blog/m3.jpg",
                MapLocation = "Stuttgart, Baden-Württemberg",
                Status = true
            });

            await _context.SaveChangesAsync();
        }
    }
}
