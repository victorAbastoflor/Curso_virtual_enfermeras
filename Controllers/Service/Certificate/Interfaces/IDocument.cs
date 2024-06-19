using QuestPDF.Fluent;
using QuestPDF.Infrastructure;

namespace NurseCourse.Controllers.Service.Certificate.Interfaces;

public interface IDocument
{
    Document CreateDocument();
    void Compose(IDocumentContainer pdf);
    void ComposeHeaderDocument(IContainer header);
    void ComposeBodyDocument(IContainer body);
}