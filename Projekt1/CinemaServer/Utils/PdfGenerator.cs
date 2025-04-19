using CinemaServer.Models;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;

namespace CinemaServer.Utils
{
    public static class PdfGenerator
    {
        public static byte[] GenerujPotwierdzenie(Rezerwacja rezerwacja)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document();
                PdfWriter.GetInstance(document, ms);
                document.Open();

                // Tytuł
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                document.Add(new Paragraph("Potwierdzenie Rezerwacji", titleFont));
                document.Add(new Paragraph($"Data rezerwacji: {rezerwacja.DataRezerwacji}"));
                document.Add(new Paragraph($"Imię i nazwisko: {rezerwacja.ImieNazwisko}"));
                document.Add(new Paragraph(" "));

                // Informacje o filmie
                var sectionFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14);
                document.Add(new Paragraph("Informacje o filmie", sectionFont));
                document.Add(new Paragraph($"Tytuł: {rezerwacja.TytulFilmu}"));
                document.Add(new Paragraph($"Reżyser: {rezerwacja.RezyserFilmu}"));
                document.Add(new Paragraph($"Opis: {rezerwacja.OpisFilmu}"));
                document.Add(new Paragraph($"Aktorzy: {string.Join(", ", rezerwacja.AktorzyFilmu)}"));
                document.Add(new Paragraph(" "));

                Console.WriteLine("Rozmiar zdjęcia: " + (rezerwacja.ZdjecieFilmu?.Length ?? 0));

                // Zdjęcie (jeśli jest)
                if (rezerwacja.ZdjecieFilmu != null && rezerwacja.ZdjecieFilmu.Length > 0)
                {
                    try
                    {
                        Image img = iTextSharp.text.Image.GetInstance(rezerwacja.ZdjecieFilmu);
                        img.ScaleToFit(300f, 300f);
                        img.Alignment = Element.ALIGN_CENTER;
                        document.Add(img);
                        document.Add(new Paragraph(" "));
                    }
                    catch (Exception ex)
                    {
                        document.Add(new Paragraph("❌ Nie udało się dodać zdjęcia: " + ex.Message));
                    }
                }
                else
                {
                    document.Add(new Paragraph("Nie udało sie dodać zdjecia: "));
                }

                // Informacje o miejscach
                document.Add(new Paragraph("Zarezerwowane miejsca:", sectionFont));
                document.Add(new Paragraph($"Sala: {rezerwacja.SalaId}"));
                document.Add(new Paragraph($"Miejsca: {string.Join(", ", rezerwacja.NumeryMiejsc)}"));

                document.Close();
                return ms.ToArray();
            }
        }
    }
}


