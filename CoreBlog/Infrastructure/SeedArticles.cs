namespace CoreBlog.Infrastructure
{
    /// <summary>Beispielartikel für die Demo-Datenbank.</summary>
    internal static class SeedArticles
    {
        internal record Article(string Key, string Title, string Summary, string Category, string Writer,
            string Image, int DaysAgo, int Views, bool Featured, string Content);

        internal static readonly Article[] All =
        {
            new("clean-code", "Clean Code im Alltag: kleine Regeln mit großer Wirkung",
                "Lesbarer Code entsteht nicht durch große Refactorings, sondern durch viele kleine Entscheidungen. Fünf Gewohnheiten, die sofort helfen.",
                "Softwareentwicklung", "Jonas Weber", "/img/blog/22.jpg", 3, 1240, true, """
                Code wird viel öfter gelesen als geschrieben. Wer heute eine Methode schreibt, liest sie in drei Monaten wieder – oder eine Kollegin muss sie unter Zeitdruck verstehen. Lesbarkeit ist deshalb keine Stilfrage, sondern eine Investition in die Zukunft des Projekts.

                ## Namen, die etwas erklären

                Eine Variable namens `d` sagt nichts. `daysSinceLastLogin` erklärt dagegen schon beim Lesen, was passiert. Gute Namen ersetzen viele Kommentare. Eine einfache Faustregel: Wenn Sie einen Kommentar brauchen, um eine Zeile zu erklären, versuchen Sie zuerst, einen besseren Namen zu finden.

                ## Kleine Methoden mit einer Aufgabe

                Eine Methode sollte genau eine Sache tun. Wenn Sie beim Beschreiben das Wort „und“ verwenden – „Sie lädt die Daten und berechnet den Preis und schreibt das Log“ – ist das ein Hinweis, dass sie geteilt werden kann. Kleine Methoden lassen sich leichter testen und wiederverwenden.

                ## Frühe Rückgaben statt tiefer Verschachtelung

                Statt drei verschachtelter `if`-Blöcke prüfen Sie ungültige Fälle am Anfang und verlassen die Methode sofort. Der eigentliche Ablauf steht dann links ausgerichtet und ist auf einen Blick erkennbar.

                ## Die Pfadfinderregel

                Hinterlassen Sie jede Datei ein bisschen sauberer, als Sie sie vorgefunden haben. Ein besserer Name hier, eine entfernte Doppelung dort. So verbessert sich die Codebasis kontinuierlich, ohne dass jemand ein großes Refactoring planen muss.

                ## Fazit

                Clean Code ist kein Dogma, sondern Rücksichtnahme auf alle, die nach Ihnen am Code arbeiten. Fangen Sie mit einer Regel an und machen Sie sie zur Gewohnheit – die anderen folgen fast von selbst.
                """),

            new("git-workflow", "Git-Workflow für kleine Teams: einfach, aber verbindlich",
                "Feature-Branches, Pull Requests und klare Commit-Nachrichten: ein schlanker Ablauf, der auch in Teams mit drei Personen funktioniert.",
                "Softwareentwicklung", "Jonas Weber", "/img/blog/banner2.jpg", 18, 860, false, """
                Viele kleine Teams arbeiten direkt auf dem Hauptzweig. Das geht gut – bis zwei Personen gleichzeitig an derselben Datei arbeiten oder ein halbfertiges Feature live geht. Ein einfacher, verbindlicher Workflow verhindert genau das.

                ## Ein Branch pro Aufgabe

                Jede Aufgabe bekommt einen eigenen Branch, zum Beispiel `feature/kommentar-bewertung` oder `fix/login-weiterleitung`. Der Hauptzweig bleibt dadurch jederzeit lauffähig, und jeder weiß, woran gerade gearbeitet wird.

                ## Pull Requests als Gespräch

                Ein Pull Request ist keine Kontrolle, sondern eine Einladung zum Mitdenken. Halten Sie ihn klein: Unter 300 geänderten Zeilen bekommen Sie konkrete Rückmeldungen statt eines schnellen „Sieht gut aus“.

                ## Commit-Nachrichten, die man später versteht

                „Fix“ oder „Update“ helfen niemandem. Besser ist eine kurze Zusammenfassung im Imperativ: „Bewertung bei gelöschten Kommentaren neu berechnen“. Wer in einem Jahr nach der Ursache eines Fehlers sucht, wird dankbar sein.

                ## Regeln schriftlich festhalten

                Schreiben Sie die Vereinbarungen in die README-Datei: Wie heißen Branches? Wer darf zusammenführen? Was muss vor einem Merge erfüllt sein? Neue Teammitglieder finden sich so in Minuten zurecht.

                Ein guter Workflow ist so einfach, dass niemand darüber nachdenken muss – und so verbindlich, dass sich alle daran halten.
                """),

            new("dependency-injection", "Dependency Injection in ASP.NET Core verständlich erklärt",
                "Warum ein Controller seine Abhängigkeiten nicht selbst erzeugen sollte und wie der eingebaute DI-Container den Code testbar macht.",
                "Softwareentwicklung", "Daniel Krüger", "/img/blog/m4.jpg", 41, 1530, false, """
                In vielen Einsteigerprojekten erzeugt ein Controller seine Abhängigkeiten selbst: `new BlogManager(new EfBlogRepository())`. Das funktioniert, koppelt aber alles fest aneinander. Dependency Injection löst dieses Problem elegant.

                ## Das Prinzip

                Eine Klasse beschreibt im Konstruktor, was sie braucht – zum Beispiel ein `IBlogService`. Wer das konkrete Objekt erzeugt, entscheidet nicht die Klasse selbst, sondern der DI-Container. Die Klasse kennt nur die Schnittstelle.

                ## Registrierung in Program.cs

                In ASP.NET Core werden Dienste beim Start registriert: `services.AddScoped<IBlogService, BlogManager>()`. „Scoped“ bedeutet: Pro HTTP-Anfrage gibt es genau eine Instanz. Das ist ideal für den Datenbankkontext, denn alle Repositories einer Anfrage teilen sich dieselbe Verbindung.

                ## Die drei Lebensdauern

                Singleton lebt so lange wie die Anwendung, Scoped so lange wie eine Anfrage und Transient wird bei jeder Verwendung neu erzeugt. Als Faustregel gilt: Alles, was den Datenbankkontext verwendet, ist Scoped.

                ## Warum sich das lohnt

                Im Test können Sie statt der echten Datenbank ein Fake-Repository übergeben. Und wenn Sie später von SQL Server auf SQLite wechseln, ändern Sie eine Zeile in der Registrierung – nicht hunderte Stellen im Code.

                Dependency Injection wirkt anfangs wie zusätzlicher Aufwand. Spätestens beim ersten Test oder Datenbankwechsel zeigt sich, wie viel Zeit es spart.
                """),

            new("deep-work", "Deep Work: konzentriert arbeiten in einer lauten Welt",
                "Benachrichtigungen, Chats, Meetings – echte Konzentration ist selten geworden. So schaffen Sie sich wieder Zeit für anspruchsvolle Aufgaben.",
                "Produktivität", "Lena Hoffmann", "/img/blog/7.jpg", 7, 1105, false, """
                Die wertvollste Arbeit entsteht selten zwischen zwei Chatnachrichten. Ein komplexer Algorithmus, ein gutes Konzept oder ein sauberer Text brauchen ungestörte Zeit. Genau diese Zeit muss man sich heute bewusst nehmen.

                ## Fokuszeiten im Kalender blockieren

                Tragen Sie zwei Stunden konzentrierte Arbeit als festen Termin ein – am besten vormittags, wenn die Energie am höchsten ist. Ein Termin mit sich selbst ist genauso verbindlich wie ein Meeting mit anderen.

                ## Benachrichtigungen bündeln

                Statt jede Nachricht sofort zu beantworten, prüfen Sie Mails und Chats zu festen Zeiten, zum Beispiel um 11 und um 15 Uhr. Die meisten Nachrichten sind weniger dringend, als sie wirken.

                ## Eine Aufgabe, ein Ziel

                Beginnen Sie jede Fokusphase mit einem klaren Satz: „Bis 11 Uhr ist die Suchfunktion getestet.“ Ein konkretes Ziel macht es leichter, Ablenkungen abzulehnen.

                ## Pausen sind Teil der Arbeit

                Nach 90 Minuten sinkt die Konzentration deutlich. Ein kurzer Spaziergang oder ein Glas Wasser fern vom Bildschirm bringen mehr als ein schneller Blick auf das Smartphone.

                Deep Work ist eine Fähigkeit, die man trainieren kann. Starten Sie mit einer Fokusphase pro Tag und steigern Sie sich langsam.
                """),

            new("aufschieben", "Die Zwei-Minuten-Regel und andere Tricks gegen das Aufschieben",
                "Prokrastination hat selten mit Faulheit zu tun. Drei einfache Methoden, um unangenehme Aufgaben endlich anzufangen.",
                "Produktivität", "Lena Hoffmann", "/img/blog/banner3.jpg", 25, 740, false, """
                Fast jeder kennt das: Eine Aufgabe ist eigentlich nicht schwer, und trotzdem wird sie seit Tagen verschoben. Aufschieben ist meist ein Gefühlsproblem – die Aufgabe wirkt unklar, groß oder unangenehm.

                ## Die Zwei-Minuten-Regel

                Alles, was weniger als zwei Minuten dauert, wird sofort erledigt: die kurze Antwort, der Termin im Kalender, der Bugreport. So wächst die Liste kleiner Aufgaben gar nicht erst an.

                ## Den ersten Schritt verkleinern

                „Bewerbung schreiben“ ist zu groß. „Stellenanzeige öffnen und drei Anforderungen markieren“ ist machbar. Je kleiner der erste Schritt, desto geringer der innere Widerstand.

                ## Der Fünf-Minuten-Start

                Nehmen Sie sich vor, nur fünf Minuten an der Aufgabe zu arbeiten. Danach dürfen Sie aufhören. In den meisten Fällen arbeiten Sie weiter, weil das Anfangen der schwierigste Teil war.

                ## Fortschritt sichtbar machen

                Streichen Sie erledigte Schritte bewusst ab. Ein sichtbarer Fortschritt motiviert stärker als jede Belohnung, die erst am Ende wartet.

                Aufschieben verschwindet nicht über Nacht. Aber mit kleinen Schritten wird aus „irgendwann“ sehr oft „heute“.
                """),

            new("bullet-journal", "Bullet Journal für Entwickler: Notizen, die wirklich helfen",
                "Ein Notizbuch neben der Tastatur wirkt altmodisch – und ist trotzdem eines der besten Werkzeuge gegen Chaos im Kopf.",
                "Produktivität", "Sara Neumann", "/img/blog/b5.jpg", 55, 520, false, """
                Zwischen Tickets, Code-Reviews und spontanen Fragen geht schnell der Überblick verloren. Ein einfaches Notizbuch kann hier mehr leisten als das fünfte Produktivitäts-Tool.

                ## Das tägliche Log

                Jeder Tag beginnt mit einer neuen Überschrift und drei Zeichen: ein Punkt für Aufgaben, ein Kreis für Termine und ein Strich für Notizen. Erledigte Aufgaben bekommen ein Kreuz, verschobene einen Pfeil.

                ## Probleme aufschreiben, bevor man sie löst

                Wer einen Fehler in einem Satz beschreiben kann, hat ihn oft schon halb verstanden. Notieren Sie vor dem Debuggen kurz, was Sie erwarten und was tatsächlich passiert.

                ## Wochenrückblick am Freitag

                Zehn Minuten am Ende der Woche reichen: Was hat gut funktioniert? Was wurde dreimal verschoben? Diese Notizen sind auch eine wertvolle Grundlage für das Berichtsheft oder das nächste Mitarbeitergespräch.

                ## Analog und digital kombinieren

                Das Notizbuch ersetzt kein Ticketsystem. Es ist der Ort für Gedanken, die noch nicht fertig sind. Was verbindlich wird, wandert ins Team-Tool.

                Probieren Sie es zwei Wochen lang aus – Sie werden überrascht sein, wie viel ruhiger der Kopf wird.
                """),

            new("weissraum", "Weißraum ist kein leerer Raum",
                "Abstand ist eines der stärksten Gestaltungsmittel. Warum gute Oberflächen manchmal vor allem davon leben, was weggelassen wird.",
                "Design", "Sara Neumann", "/img/blog/m2.jpg", 12, 690, false, """
                Wer zum ersten Mal eine Oberfläche gestaltet, möchte jeden freien Pixel nutzen. Dabei ist Weißraum – der Abstand zwischen Elementen – eines der wirkungsvollsten Werkzeuge im Design.

                ## Abstand schafft Zusammenhang

                Elemente, die nah beieinanderstehen, gehören für das Auge zusammen. Ein Label direkt über seinem Eingabefeld und etwas mehr Abstand zum nächsten Feld: Schon ist das Formular ohne zusätzliche Linien verständlich.

                ## Ein festes Abstandssystem

                Statt Abstände nach Gefühl zu setzen, hilft eine feste Skala, etwa 4, 8, 16, 24 und 40 Pixel. Die Oberfläche wirkt dadurch ruhiger, und Entscheidungen im Team werden einfacher.

                ## Weniger Elemente, mehr Wirkung

                Jede zusätzliche Schaltfläche konkurriert um Aufmerksamkeit. Fragen Sie bei jedem Element: Hilft es der Person, ihr Ziel zu erreichen? Wenn nicht, darf es gehen.

                ## Lesbarkeit von Texten

                Großzügige Zeilenabstände und eine Zeilenlänge von etwa 60 bis 75 Zeichen machen lange Texte deutlich angenehmer – wie bei diesem Artikel.

                Weißraum ist kein verschenkter Platz. Er gibt dem Inhalt die Bühne, die er verdient.
                """),

            new("typografie", "Typografie fürs Web: fünf Grundregeln",
                "Schrift ist der größte Teil fast jeder Website. Mit diesen fünf Regeln wirken Texte sofort professioneller.",
                "Design", "Sara Neumann", "/img/blog/3.jpg", 67, 455, false, """
                Bis zu 90 Prozent einer typischen Website bestehen aus Text. Gute Typografie ist deshalb nicht Dekoration, sondern die Grundlage für jede Benutzeroberfläche.

                ## Höchstens zwei Schriften

                Eine Schrift für Überschriften, eine für Fließtext – mehr braucht kaum ein Projekt. Zu viele Schriften wirken unruhig und erschweren das Lesen.

                ## Eine klare Größenskala

                Legen Sie wenige Schriftgrößen fest und verwenden Sie nur diese. Ein deutlicher Unterschied zwischen Überschrift und Text schafft Hierarchie, ohne dass zusätzliche Farben nötig sind.

                ## Genug Zeilenabstand

                Für Fließtext ist ein Zeilenabstand von etwa 1,5 bis 1,7 angenehm. Serifenschriften vertragen etwas mehr, kompakte Schriften für Tabellen etwas weniger.

                ## Kontrast prüfen

                Hellgrauer Text auf weißem Hintergrund sieht elegant aus, ist aber für viele Menschen schwer lesbar. Ein Kontrastverhältnis von mindestens 4,5:1 sollte Standard sein.

                ## Schriften lokal einbinden

                Werden Schriften von externen Servern geladen, wird die IP-Adresse der Besucher übertragen. Lokal eingebundene Schriften sind schneller und datenschutzfreundlicher – ein wichtiger Punkt für Websites in Deutschland.

                Gute Typografie fällt nicht auf. Man merkt nur, dass sich der Text mühelos lesen lässt.
                """),

            new("einstieg-it", "Der erste Job nach der Umschulung: so gelingt der Einstieg",
                "Mit dem Abschluss beginnt die eigentliche Lernphase. Tipps für die ersten Monate als Anwendungsentwickler im neuen Team.",
                "Karriere", "Daniel Krüger", "/img/blog/banner1.jpg", 9, 1380, false, """
                Die Umschulung ist geschafft, der Arbeitsvertrag unterschrieben – und plötzlich sitzt man vor einer Codebasis mit hunderttausend Zeilen. Das ist normal. Niemand erwartet, dass Sie am ersten Tag alles verstehen.

                ## Fragen ist ein Zeichen von Professionalität

                Gerade am Anfang ist es besser, nach 30 Minuten gezielt zu fragen, als einen ganzen Tag allein festzustecken. Formulieren Sie Ihre Frage konkret: Was haben Sie versucht, und wo genau hängen Sie?

                ## Die Codebasis Schritt für Schritt erkunden

                Folgen Sie einer einzigen Anfrage durch das System: vom Controller über den Service bis zur Datenbank. So verstehen Sie die Architektur besser als durch das Lesen aller Dateien.

                ## Kleine Aufgaben zuerst

                Ein kleiner Bugfix, der sauber getestet und dokumentiert ist, schafft Vertrauen. Größere Features folgen, sobald Sie die Abläufe im Team kennen.

                ## Lernen sichtbar machen

                Führen Sie eine Liste mit Dingen, die Sie gelernt haben. Sie hilft in Feedbackgesprächen und zeigt Ihnen selbst, wie viel Sie in wenigen Wochen geschafft haben.

                ## Erfahrung aus dem früheren Beruf nutzen

                Wer aus einem anderen Beruf kommt, bringt Disziplin, Kundenverständnis oder Teamerfahrung mit. Diese Stärken sind im Team oft genauso wertvoll wie technisches Wissen.

                Der Einstieg ist eine Phase, kein Test. Mit Geduld und Neugier wird aus dem neuen Job schnell der eigene.
                """),

            new("portfolio", "Das Portfolio, das Recruiter wirklich lesen",
                "Drei gut dokumentierte Projekte sagen mehr als zwanzig Tutorials. Worauf es bei einem Entwickler-Portfolio ankommt.",
                "Karriere", "Lena Hoffmann", "/img/blog/5.jpg", 33, 970, false, """
                Recruiter und Teamleitungen haben wenig Zeit. Ein Portfolio muss in wenigen Minuten zeigen, was Sie können und wie Sie arbeiten.

                ## Qualität vor Menge

                Drei durchdachte Projekte sind überzeugender als eine lange Liste kopierter Tutorials. Wählen Sie Projekte, die ein echtes Problem lösen und eine klare Architektur zeigen.

                ## Eine Live-Demo ohne Hürden

                Niemand registriert sich für eine Testanwendung. Bieten Sie einen Demo-Zugang mit einem Klick an – idealerweise mit Beispieldaten, damit sofort sichtbar ist, was die Anwendung kann.

                ## Die README als Visitenkarte

                Beschreiben Sie kurz das Ziel des Projekts, die verwendeten Technologien und die Architektur. Screenshots und eine Anleitung zum lokalen Start runden den Eindruck ab.

                ## Den eigenen Anteil benennen

                Wenn ein Projekt im Kurs oder im Team entstanden ist, schreiben Sie ehrlich, welche Teile von Ihnen stammen. Das schafft Vertrauen und gibt Gesprächsstoff im Interview.

                ## Regelmäßig pflegen

                Ein Projekt mit veralteten Abhängigkeiten wirkt vernachlässigt. Ein kurzes Update auf eine aktuelle Framework-Version zeigt, dass Sie dranbleiben.

                Ein gutes Portfolio erzählt eine Geschichte: Woher Sie kommen, was Sie gelernt haben und wohin Sie wollen.
                """),

            new("homeoffice", "Homeoffice ergonomisch einrichten – ohne großes Budget",
                "Rückenschmerzen und müde Augen müssen nicht sein. Mit wenigen Anpassungen wird der Küchentisch zum gesunden Arbeitsplatz.",
                "Remote Work", "Jonas Weber", "/img/blog/banner4.jpg", 48, 610, false, """
                Viele Homeoffice-Arbeitsplätze sind improvisiert: Laptop auf dem Esstisch, Stuhl aus der Küche. Für ein paar Tage geht das, auf Dauer rächt es sich. Die gute Nachricht: Die wichtigsten Verbesserungen kosten wenig.

                ## Bildschirm auf Augenhöhe

                Die Oberkante des Bildschirms sollte etwa auf Augenhöhe liegen. Ein Laptopständer oder ein Stapel Bücher plus eine externe Tastatur entlasten den Nacken sofort.

                ## Die richtige Sitzhöhe

                Die Füße stehen flach auf dem Boden, die Oberschenkel sind etwa waagerecht. Ist der Tisch zu hoch, hilft ein Sitzkissen; sind die Beine zu kurz, eine Fußstütze.

                ## Licht von der Seite

                Ein Fenster direkt hinter dem Bildschirm blendet, eines im Rücken spiegelt. Ideal ist Tageslicht von der Seite und abends eine zusätzliche, blendfreie Lampe.

                ## Bewegung einplanen

                Der beste Stuhl hilft wenig, wenn man acht Stunden reglos sitzt. Telefonieren Sie im Stehen, und stehen Sie spätestens jede Stunde einmal auf.

                Ein gesunder Arbeitsplatz ist keine Frage des Budgets, sondern der Aufmerksamkeit. Ihr Rücken wird es Ihnen danken.
                """),

            new("async", "Asynchrone Kommunikation im verteilten Team",
                "Nicht jede Frage braucht ein Meeting. Wie Teams mit klaren Texten statt ständiger Calls schneller vorankommen.",
                "Remote Work", "Daniel Krüger", "/img/blog/m1.jpg", 80, 380, false, """
                In verteilten Teams arbeiten Menschen zu unterschiedlichen Zeiten. Wer für jede Abstimmung einen Call ansetzt, blockiert den Kalender aller Beteiligten. Asynchrone Kommunikation ist die Alternative.

                ## Vollständige Nachrichten schreiben

                „Hast du kurz Zeit?“ zwingt das Gegenüber zum Warten. Besser: Kontext, Frage und gewünschte Frist in einer Nachricht. So kann die Antwort sofort kommen, wann immer die Person Zeit hat.

                ## Entscheidungen dokumentieren

                Was im Call besprochen wurde, ist nach einer Woche vergessen. Halten Sie Entscheidungen schriftlich fest – mit Begründung. Neue Kolleginnen und Kollegen verstehen so später, warum etwas so gebaut wurde.

                ## Reaktionszeiten vereinbaren

                Asynchron heißt nicht beliebig. Vereinbaren Sie, innerhalb welcher Zeit Nachrichten beantwortet werden – zum Beispiel bis zum Ende des Arbeitstages. Für echte Notfälle gibt es einen separaten Kanal.

                ## Meetings gezielt einsetzen

                Synchrone Gespräche bleiben wichtig: für schwierige Themen, Konflikte oder kreatives Brainstorming. Der Unterschied ist, dass sie bewusst gewählt werden.

                Gute asynchrone Kommunikation braucht Übung – und belohnt mit mehr Fokuszeit und weniger Unterbrechungen.
                """),
        };
    }
}
