using System.Web;
using System.Web.Optimization;

namespace GH
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de compilación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/Script/ngGHAppScripts")
                .IncludeDirectory("~/ngGHApp/dist/ngGHApp", "*.js", true));

            bundles.Add(new StyleBundle("~/Content/ngGHAppStyles")
                .Include("~/ngGHApp/dist/ngGHApp/styles.*"));
        }
    }
}
