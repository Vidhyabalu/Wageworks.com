namespace Wageworks.Feature.Navigation
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct NavigationRoot
        {
            public static readonly ID ID = new ID("{F9F4FC05-98D0-4C62-860F-F08AE7F0EE25}");
        }

        public struct Navigable
        {
            public static readonly ID ID = new ID("{A1CBA309-D22B-46D5-80F8-2972C185363F}");

            public struct Fields
            {
                public static readonly ID ShowInNavigation = new ID("{5585A30D-B115-4753-93CE-422C3455DEB2}");
                public static readonly ID NavigationTitle = new ID("{1B483E91-D8C4-4D19-BA03-462074B55936}");
                public static readonly ID ShowChildren = new ID("{68016087-AA00-45D6-922A-678475C50D4A}");
                public static readonly ID ShowInBreadcrumb = new ID("{92EA9A0E-88A0-497A-A41D-F2A6FF393C14}");
            }
        }

        public struct Link
        {
            public static readonly ID ID = new ID("{A16B74E9-01B8-439C-B44E-42B3FB2EE14B}");

            public struct Fields
            {
                public static readonly ID Link = new ID("{FE71C30E-F07D-4052-8594-C3028CD76E1F}");
                //public static readonly string CssClass = new ID("{}");
            }
        }
        public struct LinkDescription
        {
            public static readonly ID ID = new ID("{AF39EA75-D5F8-4290-A1F4-0A55D3CF258E}");

            public struct Fields
            {
                public static readonly ID Description = new ID("{448F2DB8-C477-4D21-835E-CE7583491B24}");
            }
        }

        public struct LinkMenuItem
        {
            public static readonly ID ID = new ID("{18BAF6B0-E0D6-4CCE-9184-A4849343E7E4}");

            public struct Fields
            {
                public static readonly ID Icon = new ID("{2C24649E-4460-4114-B026-886CFBE1A96D}");
                public static readonly ID DividerBefore = new ID("{4231CD60-47C1-42AD-B838-0A6F8F1C4CFB}");
            }
        }

        public struct PromotionLinkMenuItem
        {
            public static readonly ID ID = new ID("{9F93FEC6-FD95-47D9-AE56-CEC1225C044F}");

            public struct Fields
            {
                public static readonly ID PromoLink = new ID("{437C8DDE-ECD5-4855-8DBC-7F7E59DDB42C}");
                public static readonly ID BackgroundImage = new ID("{0162689C-B666-42D7-A55C-DF0F41DE8A14}");
                public static readonly ID PromoContent = new ID("{F8B446E2-911A-4151-8AB9-F429E19E7048}");
            }
        }
    }
}