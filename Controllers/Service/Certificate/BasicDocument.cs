using NurseCourse.Controllers.Service.Certificate.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace NurseCourse.Controllers.Service.Certificate;

public class BasicDocument : Interfaces.IDocument
{
    public void Compose(IDocumentContainer pdf)
    {
        pdf.Page(page => {
            page.Size(PageSizes.Legal.Landscape());
            page.Margin(2, Unit.Centimetre);
            page.Header().Element(ComposeHeaderDocument);
            page.Content().Element(ComposeBodyDocument);
        });
    }

    public virtual void ComposeBodyDocument(IContainer body) => throw new NotImplementedException();

    public void ComposeHeaderDocument(IContainer header)
    {
        
    }

    public Document CreateDocument()
    {
        return Document.Create(Compose);
    }
}