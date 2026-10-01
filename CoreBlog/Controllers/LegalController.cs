using CoreBlog.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CoreBlog.Controllers
{
    /// <summary>
    /// Impressum und Datenschutzerklärung liegen zentral auf der Portfolio-Website.
    /// /impressum und /datenschutz leiten dorthin weiter, damit die Links in der Demo stabil bleiben.
    /// </summary>
    public class LegalController : Controller
    {
        private readonly DemoOptions _demo;
        public LegalController(IOptions<DemoOptions> demo) => _demo = demo.Value;

        [Route("impressum")]
        public IActionResult Impressum() => Redirect(_demo.ImpressumUrl);

        [Route("datenschutz")]
        public IActionResult Datenschutz() => Redirect(_demo.DatenschutzUrl);
    }
}
