using Microsoft.AspNetCore.Identity;

namespace CoreBlog.Infrastructure
{
    /// <summary>Deutsche Fehlermeldungen für ASP.NET Core Identity.</summary>
    public class GermanIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError DuplicateEmail(string email) => new() { Code = nameof(DuplicateEmail), Description = $"Die E-Mail-Adresse „{email}“ wird bereits verwendet." };
        public override IdentityError DuplicateUserName(string userName) => new() { Code = nameof(DuplicateUserName), Description = $"Der Benutzername „{userName}“ ist bereits vergeben." };
        public override IdentityError InvalidEmail(string email) => new() { Code = nameof(InvalidEmail), Description = "Die E-Mail-Adresse ist ungültig." };
        public override IdentityError PasswordTooShort(int length) => new() { Code = nameof(PasswordTooShort), Description = $"Das Passwort muss mindestens {length} Zeichen lang sein." };
        public override IdentityError PasswordRequiresDigit() => new() { Code = nameof(PasswordRequiresDigit), Description = "Das Passwort muss mindestens eine Ziffer enthalten." };
        public override IdentityError PasswordRequiresLower() => new() { Code = nameof(PasswordRequiresLower), Description = "Das Passwort muss mindestens einen Kleinbuchstaben enthalten." };
        public override IdentityError PasswordRequiresUpper() => new() { Code = nameof(PasswordRequiresUpper), Description = "Das Passwort muss mindestens einen Großbuchstaben enthalten." };
        public override IdentityError PasswordMismatch() => new() { Code = nameof(PasswordMismatch), Description = "Das aktuelle Passwort ist nicht korrekt." };
        public override IdentityError DefaultError() => new() { Code = nameof(DefaultError), Description = "Es ist ein unbekannter Fehler aufgetreten." };
    }
}
