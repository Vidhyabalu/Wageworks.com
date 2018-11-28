namespace Wageworks.Feature.News
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct NewsArticle
        {
            public static readonly ID ID = new ID("{2F75C8AF-35FC-4A88-B585-7595203F442C}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{BD9ECD4A-C0B0-4233-A3CD-D995519AC87B}");
                public const string Title_FieldName = "newstitle_t";

                public static readonly ID Image = new ID("{3437EAAC-6EE8-460B-A33D-DA1F714B5A93}");
                public static readonly ID ThumbnailImage = new ID("{5BC3FFED-82F9-4A85-AA43-6C470D24CF09}");

                public static readonly ID Date = new ID("{C464D2D7-3382-428A-BCDF-0963C60BA0E3}");

                public static readonly ID Summary = new ID("{9D08271A-1672-44DD-B7EF-0A6EC34FCBA7}");
                public const string Summary_FieldName = "newssummary_t";

                public static readonly ID Body = new ID("{801612C7-5E98-4E3C-80D2-A34D0EEBCBDA}");
                public const string Body_FieldName = "newsbody_t";

                public static readonly ID SeeMoreStories = new ID("{B84938EC-AC9C-4078-A795-86AF7FED5A2A}");
                public const string SeeMoreStories_FieldName = "See More Stories";

                public static readonly ID RelatedArticles = new ID("{560A0ADF-32E0-41BD-8F7C-676E130413BB}");
                public const string RelatedArticles_FieldName = "Related Articles";

                public static readonly ID Category = new ID("{A7CF42CC-1BF2-489A-9ABC-831BFE434AB0}");

                public static readonly ID MediaVideoLink = new ID("{2628705D-9434-4448-978C-C3BF166FA1EB}");

                public static readonly ID AlertText = new ID("{6B04809D-3A01-4FE6-8BBF-3759211B0B10}");

                public static readonly ID AllowClose = new ID("{08B670BB-9195-4492-A086-F265AA6D8D99}");

                public static readonly ID EndDate = new ID("{01A78FCB-BF94-433F-8799-829C214532AE}");

                public static readonly ID NewsListPage = new ID("{F8362B6B-2521-44B7-AB8F-CDE2556BE916}");
            }
        }

        public struct NewsTaxonomy
        {
            public static readonly ID ID = new ID("{2364F78B-1357-4BFC-B048-4E69DECC9039}");

            public struct Fields
            {
                public static readonly ID Category = new ID("{A7CF42CC-1BF2-489A-9ABC-831BFE434AB0}");
            }
        }

        public struct Taxonomy
        {
            public static readonly ID ID = new ID("{A12D7BA3-0C9F-4AB3-90E2-5D71C8A9D700}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{EABC70CA-F791-4CAF-978C-1F0E8E95371F}");
            }
        }

        public struct NewsFolder
        {
            public static readonly ID ID = new ID("{74889B26-061C-4D6A-8CDB-422665FC34EC}");
            public struct Fields
            {
                public static readonly ID PageSize = new ID("{66972584-A157-4059-B28A-60FC159C1805}");
            }
        }

        public struct ExpertAdviceGroup
        {
            public static readonly ID ID = new ID("{7AF5243A-3791-4BA3-88B4-B71285541EC5}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{2372B9DF-C5AB-48AA-8C10-C68271261880}");
                public const string Title_FieldName = "Title";

                public static readonly ID Articles = new ID("{BD3DBC32-B6A8-4103-8C2F-CFD86DF2CBB5}");
                public const string Articles_FieldName = "Articles";

                public static readonly ID ListPage = new ID("{2E3B7E91-0004-4FD5-AD08-9B1B6E5AF47E}");
                public const string LisgtPage_FieldName = "List Page";
            }
        }

        public struct NewsGrouping
        {
            public static readonly ID ID = new ID("{51B31FEB-54CB-4320-A9F5-080D6BDB106D}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{5EF06621-830B-4C14-B3FA-711BB9183FE3}");
                public static readonly ID Members = new ID("{404230F8-5075-40E5-A3CD-AEE0198AA1DF}");
            }
        }
    }
}