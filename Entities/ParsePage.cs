using AngleSharp.Dom;
using ParserToyRu.Services;
using System.Text;


namespace ParserToyRu.Entities
{
    public class ParsePage : IParsePage
    {
        public const string NetAns = "Нет в наличии";
        public const string NewStr = ";\n";
        public const string Delimiter = " > ";
        public const string BasePrice = "0 руб.";
        private List<string> linksList = new List<string>();
        public static string links;
        public static string newsPrice;
        public static string oldPrice;
        public static string productName;
        public static string availability;
        public static string breadCrumb;
        public static string productRegion;

        public async Task<string> GetAvailability(IDocument document)
        {
            await Task.Run(() => 
            {
                new Thread(() => 
                {
                    try
                    {
                        var ok = document.QuerySelectorAll("span")
                        .Where(x => x.ClassName != null && x.ClassName == "ok")
                        .First().TextContent
                        .Trim();
                        availability = ok;
                    }
                    catch
                    {
                        try
                        {
                            var net = document.QuerySelectorAll("div")
                            .Where(x => x.ClassName != null && x.ClassName == "net-v-nalichii")
                            .First().TextContent
                            .Trim();
                            availability = net;
                        }
                        catch
                        {
                            availability = NetAns;
                        };
                    }
                }).Start();
                
            });
            
            Console.WriteLine(availability);
            return availability + NewStr;
        }

        public async Task<string> GetBreadCrumb(IDocument document, string productName)
        {
            string tempBreadCrumb = null;
            await Task.Run(() => 
            {
                new Thread(() => 
                {
                    foreach (var child in document.QuerySelectorAll("nav.breadcrumb>a>span"))
                    {
                        tempBreadCrumb += child.TextContent.Trim() + Delimiter;
                    }
                    breadCrumb = tempBreadCrumb;

                }).Start();
                
            });
            
            Console.WriteLine(breadCrumb + productName);
            return breadCrumb + productName;
        }

        public async Task<string> GetImgLinks(IDocument document)
        {
            string delimetr = " ";
            await Task.Run(() => 
            {
                new Thread(() => 
                {
                    foreach (var img in document.QuerySelectorAll("img")
                    .Where(x => x.ClassName != null && x.ClassName == "img-fluid"))
                    {
                        linksList.Add(img.Attributes["src"].Value);
                    }

                    StringBuilder sb = new StringBuilder();

                    for (int i = 0; i < linksList.Count; i++)
                    {
                        sb.Append(linksList[i]);
                        if (i < linksList.Count - 1)
                        {
                            sb.Append(delimetr);
                        }
                    }
                    links = sb.ToString();
                }).Start();
            });

            Console.WriteLine(links);
            return links + NewStr;
        }

        public async Task<string> GetNewPrice(IDocument document)
        {
            await Task.Run(() =>
            {
                new Thread(() => 
                {
                    try
                    {
                        var newPrice = document.QuerySelectorAll("span")
                        .Where(x => x.ClassName != null && x.ClassName == "price")
                        .First().TextContent
                        .Trim();
                        newsPrice = newPrice;
                    }
                    catch
                    {
                        newsPrice = BasePrice;
                    }
                }).Start();
            });

            Console.WriteLine(newsPrice);
            return newsPrice + NewStr;
        }

        public async Task<string> GetOldPrice(IDocument document)
        {
            await Task.Run(() =>
            {
                new Thread(() => {
                    try
                    {
                        string oldPriceTemp = document.QuerySelectorAll("span")
                        .Where(x => x.ClassName != null && x.ClassName == "old-price")
                        .First().TextContent
                        .Trim();
                        oldPrice = oldPriceTemp;
                    }
                    catch
                    {
                        oldPrice = BasePrice;
                    }
                }).Start();

            });

            Console.WriteLine(oldPrice);
            return oldPrice + NewStr;
        }

        public async Task<string> GetProductName(IDocument document)
        {
            await Task.Run(() => 
            {
                new Thread(() =>
                {
                    try
                    {
                        productName = document.QuerySelectorAll("h1")
                        .Where(x => x.ClassName != null && x.ClassName == "detail-name")
                        .First()
                        .TextContent
                        .Trim();
                    }
                    catch
                    {
                        productName = "Non product name";
                    }
                }).Start();
            });
            
            Console.WriteLine(productName);
            return productName + NewStr;
        }

        public async Task<string> GetRegionName(IDocument document)
        {
            await Task.Run(() =>
            {
                new Thread(() => 
                {
                    try
                    {
                        productRegion = document.QuerySelector("div.col-12.select-city-link>a").TextContent.Trim();
                    }
                    catch
                    {
                        productRegion = "Non region";
                    }
                }).Start();

            });

            Console.WriteLine(productRegion);
            return productRegion + NewStr;
        }
    }
}
