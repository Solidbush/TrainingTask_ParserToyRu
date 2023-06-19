using CsvHelper.Configuration.Attributes;

namespace ParserToyRu.Models
{
    public class ProductModel
    {
        [Name("Регион")]
        [Index(0)]
        public string RegionName { get; set; }

        [Name("Хлебные крошки")]
        [Index(1)]
        public string BreadCrumb { get; set; }

        [Name("Название товара")]
        [Index(2)]
        public string ProductName { get; set; }

        [Name("Старая цена")]
        [Index(3)]
        public string OldPrice { get; set; }

        [Name("Цена со скидкой")]
        [Index(4)]
        public string NewPrice { get; set; }

        [Name("Наличие товара")]
        [Index(5)]
        public string Availability { get; set; }

        [Name("Ссылки на картинки")]
        [Index(6)]
        public string ImgLinks {get; set; }

        [Name("Ссылка на товар")]
        [Index(7)]
        public string ProductLink { get; set; }

    }
}
