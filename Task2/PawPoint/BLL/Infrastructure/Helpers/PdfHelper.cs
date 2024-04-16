using Common.Resources;
using PdfSharp.Pdf.IO;

namespace BLL.Infrastructure.Helpers;
public static class PdfHelper
{
    public static MemoryStream GetPdfMemoryStreamFromBytes(byte[] pdfBytes)
    {
        if (pdfBytes == null || pdfBytes.Length == 0)
            throw new ArgumentException(Resources.Get("FILE_NOT_FOUND"));

        var stream = new MemoryStream();
        stream.Write(pdfBytes, 0, pdfBytes.Length);
        stream.Position = 0;

        var document = PdfReader.Open(stream, PdfDocumentOpenMode.Import);
        _ = document.PageCount;

        document.Save(stream, false);
        return stream;
    }
}
