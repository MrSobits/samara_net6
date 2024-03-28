namespace Bars.Gkh.RegOperator.Imports.Account
{
    using Bars.Gkh.RegOperator.Enums;

    public sealed class Record
    {
        public bool isValidRecord { get; set; }

        public string LocalityName { get; set; }

        public string StreetName { get; set; }

        public string House { get; set; }

        public string Housing { get; set; }

        public string Apartment { get; set; }

        public string Owner { get; set; }

        public decimal Area { get; set; }

        public decimal AreaShare { get; set; }

        public PersonalAccountOwnerType OwnerType { get; set; }

        public int RowNumber { get; set; }

        public long RealtyObjectId { get; set; }
    }
}