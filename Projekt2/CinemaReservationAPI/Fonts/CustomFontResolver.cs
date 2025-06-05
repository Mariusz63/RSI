using PdfSharp.Fonts;
using System.Reflection;

namespace CinemaReservationAPI.Fonts
{
    public class CustomFontResolver : IFontResolver
    {
        public byte[] GetFont(string faceName)
        {
            var resource = "CinemaReservationAPI.Fonts.Verdana.ttf"; 
            using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource);
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }

        public FontResolverInfo ResolveTypeface(string familyName, bool isBold, bool isItalic)
        {
            if (familyName.Equals("Verdana", StringComparison.OrdinalIgnoreCase))
            {
                return new FontResolverInfo("Verdana#");
            }

            return null;
        }
    }

}
