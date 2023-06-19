using ParserToyRu.Entities;
using ParserToyRu.Services;

string address = "https://www.toy.ru/catalog/boy_transport/?count=18&filterseccode%5B0%5D=transport&PAGEN_5=";
int countPages = 2;

IParseWorker parseWorker = new ParseWorker(address);
foreach (var card in parseWorker.GetAllProductCards(countPages))
{
    await Task.Run(() =>
    {
        parseWorker.WritePage(parseWorker.ParsePage(card).Result);
    });
}

