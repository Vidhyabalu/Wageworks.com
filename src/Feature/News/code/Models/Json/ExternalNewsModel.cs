namespace Wageworks.Feature.News.Models.Json
{
    public class ExternalNewsModel
    {
        public int ID { get; set; }
        public string BRAND_ID { get; set; }
        public string YEAR1 { get; set; }
        public string TITLE { get; set; }
        public string BODY_IMG { get; set; }
        public string FRONT_IMG { get; set; }
        public string BODY_TEXT { get; set; }
        public string HI_RES_IMG { get; set; }
        public bool ARCHIVE { get; set; }
        public string DATE_ENTER { get; set; }
        public string DATE_DISPLAYED { get; set; }
        public string KEYWORDS { get; set; }
        public string IMG_DESC { get; set; }
        public string WORD_DOC { get; set; }
        public string PDF_DOC { get; set; }
    }
}