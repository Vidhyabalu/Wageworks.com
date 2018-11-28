using System;

namespace Wageworks.Foundation.Solr.SpatialSearch
{
    [Serializable]
    public class SpatialPoint
    {
        public SpatialPoint()
        {
            
        }

        public SpatialPoint(string value)
        {
            if (value == null) throw new ArgumentNullException("value");
            RawValue = value;
            var tokens = value.Split(',');
            if (tokens.Length != 2) throw  new ArgumentException("incorrect spatial point format. Value must be supplied in the following format lat,lon");
            var strLat = tokens[0].Trim();
            var strLon = tokens[1].Trim();
            Lat = double.Parse(strLat);
            Lon = double.Parse(strLon);
        }

        protected string RawValue { get; set; }
        public double Lat { get; set; }
        public double Lon { get; set; }


        public override string ToString()
        {
            return RawValue;
        }
    }
}
