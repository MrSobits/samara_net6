namespace Bars.Gkh.RegOperator.Dto
{
    using Bars.Gkh.RegOperator.Entities;

    public class BasePersonalAccountDto : PersonalAccountDto
    {
        public string IntNumber { get; set; }

        public string PlaceName { get; set; }

        public string StreetName { get; set; }

        public decimal RoomArea { get; set; }

        public decimal Tariff { get; set; }

        public bool AccuralByOwnersDecision { get; set; }

        public string HouseNum { get; set; }

        public string Letter { get; set; }

        public string Housing { get; set; }

        public string Building { get; set; }

        public string PostalCode { get; set; }
    }
}