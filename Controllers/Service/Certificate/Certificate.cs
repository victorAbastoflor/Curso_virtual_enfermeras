using NurseCourse.Models;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace NurseCourse.Controllers.Service.Certificate;

public class Certificate : BasicDocument
{
    private string _cursoNombre;
    private string  _usuarioNombre;
    public Certificate(string cursoNombre, string usuarioNombre)
    {
        _cursoNombre = cursoNombre;
        _usuarioNombre = usuarioNombre;
    }

    public override void ComposeBodyDocument(IContainer body)
    {
        body
        .Padding(2, Unit.Centimetre)
        .Column(column =>
        {
            column.Item()
                .AlignCenter()
                .Text(text =>
                {
                    text.Span("CERTIFICADO A:").FontColor(Colors.Blue.Medium).ExtraBlack().FontSize(32);
                });

            column.Item()
                .AlignCenter()
                .Text(_usuarioNombre.ToUpper())
                .FontSize(45)
                .Bold()
                .FontColor(Colors.Blue.Medium);

            column.Item()
                .AlignCenter()
                .Text(text =>
                {
                    text.Line("Recibe este certificado por haber")
                        .FontSize(15);
                    text.Line("completado exitosamente el curso en línea")
                        .FontSize(15);
                    text.Line(_cursoNombre)
                        .FontSize(16).ExtraBlack();
                    text.Line(DateTime.Now.ToShortDateString())
                        .ExtraBlack()
                        .FontSize(16);
                });

            column.Item()
                .Row(row =>
                {
                    row.RelativeItem().Height(120)
                        .AlignLeft()
                        .AlignCenter()
                        .PaddingBottom(-15)
                        .PaddingLeft(40)
                        .Image("Images/firmaDigitalCR.png")
                        .FitWidth()
                        .FitHeight();
                    row.RelativeItem().Height(120)
                        .AlignRight()
                        .AlignCenter()
                        .PaddingBottom(-15)
                        .PaddingRight(40)
                        .Image("Images/firmaDigital1.png")
                        .FitWidth()
                        .FitHeight();
                });

            column.Item()
            .Height(50)
                .Row(row =>
                {
                    row.RelativeItem().PaddingLeft(90)
                        .AlignLeft().AlignCenter()
                        .Text(text =>
                        {
                            text.Line("Cristiano Ronaldo").FontSize(15).Black();
                            text.Span("Firma").FontSize(9).Black();
                        });

                    row.RelativeItem().PaddingRight(90)
                        .AlignRight().AlignCenter()
                        .Text(text =>
                        {
                            text.Line("Georgina Rodriguez").FontSize(15).Black();
                            text.Span("Firma").FontSize(9).Black();
                        });
                });
        });
    }
}