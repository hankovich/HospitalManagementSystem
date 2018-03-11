namespace Hms.Common.Interface.Domain
{
    public class BuildingAddress
    {
        public int Id { get; set; }

        public string City { get; set; }

        public string Street { get; set; }

        public string Building { get; set; }

        public PolyclinicRegion PolyclinicRegion { get; set; }
    }
}