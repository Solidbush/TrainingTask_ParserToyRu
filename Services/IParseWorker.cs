using AngleSharp.Dom;
using ParserToyRu.Models;

namespace ParserToyRu.Services
{
    internal interface IParseWorker
    {
        Task<IDocument> OpenDocument(string adress);

        IEnumerable<string> GetAllProductCards(int pages);

        Task<ProductModel> ParsePage(string pageLink);

        void WritePage(ProductModel page);

    }
}
