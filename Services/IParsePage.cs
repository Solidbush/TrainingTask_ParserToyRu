using AngleSharp.Dom;


namespace ParserToyRu.Services
{
    internal interface IParsePage
    {
        Task<string> GetRegionName(IDocument document);

        Task<string> GetBreadCrumb(IDocument document, string productName);

        Task<string> GetProductName(IDocument document);

        Task<string> GetOldPrice(IDocument document);

        Task<string> GetNewPrice(IDocument document);

        Task<string> GetAvailability(IDocument document);

        Task<string> GetImgLinks(IDocument document);

    }
}
