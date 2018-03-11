namespace Hms.Common.Interface.Domain
{
    public class PolyclinicRegion
    {
        public int Id { get; set; }

        public int RegionNumber { get; set; }

        public Doctor RegionHead { get; set; }
    }
}