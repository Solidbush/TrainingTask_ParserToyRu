using AngleSharp;
using AngleSharp.Dom;
using CsvHelper;
using CsvHelper.Configuration;
using ParserToyRu.Exceptions;
using ParserToyRu.Models;
using ParserToyRu.Services;
using System.Globalization;
using System.Text;

namespace ParserToyRu.Entities
{
    internal class ParseWorker : IParseWorker
    {
        private const string Domen = "https://www.toy.ru";
        private const int StartPage = 1;
        public string adress;
        public IBrowsingContext context;
        public IDocument document;
        public IDocument tempPage;
        public List<string> cardLinks;
        
        public ParseWorker(string adress) 
        {
            this.adress = adress;
            IConfiguration config = Configuration.Default.WithDefaultLoader();
            context = BrowsingContext.New(config);
            tempPage = OpenDocument($"{adress}{StartPage}").Result;
            cardLinks = new List<string>();
        }

        public IEnumerable<string> GetCardLinks()
        {
            return cardLinks;
        }

        async public Task<IDocument> OpenDocument(string adress)
        {
            try
            {
                IDocument page = await context.OpenAsync(adress);
                return page;
            }
            catch
            {
                throw new BadRequestException($"Bad request with address: {adress}");
            }
        }

        public IEnumerable<string> GetAllProductCards(int countPages)
        {
            int count = 1;
            Console.WriteLine(countPages);
            while (count <= countPages)
            {
                Console.WriteLine($"{adress}{count}");
                document = OpenDocument($"{adress}{count}").Result;
                var cells = document.QuerySelectorAll("a")
                    .Where(item => item.ClassName != null && item.ClassName == "d-block img-link text-center gtm-click");
                Console.WriteLine("Count block a: " + cells.Count());

                foreach (var cell in cells)
                {
                    cardLinks.Add((Domen + cell.Attributes["href"].Value).ToString());
                }

                Console.WriteLine($"Page number: {count} from {countPages}");
                count++;
            };

            cardLinks = cardLinks.GroupBy(x => x).Select(x => x.First()).ToList();
            Console.WriteLine($"TotalLinks: {cardLinks.Count}");
            foreach (var link in cardLinks)
            {
                Console.WriteLine(link);
            }

            return cardLinks;
        }

        public async Task<ProductModel> ParsePage(string pageLink)
        {
            ProductModel tempModel = new ProductModel();
            IParsePage tempParser = new ParsePage();
            await Task.Run(() =>
            {
                IDocument document = OpenDocument(pageLink).Result;
                tempModel.RegionName = tempParser.GetRegionName(document).Result;
                tempModel.ProductName = tempParser.GetProductName(document).Result;
                tempModel.BreadCrumb = tempParser.GetBreadCrumb(document, tempModel.ProductName).Result;
                tempModel.NewPrice = tempParser.GetNewPrice(document).Result;
                tempModel.OldPrice = tempParser.GetOldPrice(document).Result;
                tempModel.Availability = tempParser.GetAvailability(document).Result;
                tempModel.ImgLinks = tempParser.GetImgLinks(document).Result;
                tempModel.ProductLink = pageLink + ";\n";

                return tempModel;
            });

            return tempModel;
        } 
            


        public void WritePage(ProductModel pageModel)
        {
            string path = "D:\\TaskParser\\answer1.csv";
            using (var writer = new StreamWriter(path, true, Encoding.UTF8))
            {
                var csvConfig = new CsvConfiguration(CultureInfo.GetCultureInfo("en-EN"))
                {
                    Delimiter = ""
                };

                using (var csv = new CsvWriter(writer, csvConfig))
                {
                    csv.WriteRecord(pageModel);
                }
            }
        }
    }
}
