namespace Bars.Gkh1468.Domain.PassportImport.ProxyEntity
{
    using System.Collections.Generic;
    using Entities;

    /// <summary>
    /// Паспорт вместе с аттрибутами 
    /// </summary>
    public class PassportWithAttributes
    {
        public HouseProviderPassport Passport { get; set; }

        public List<HouseProviderPassportRow> Rows { get; set; }

        public PassportWithAttributes()
        {
            Rows = new List<HouseProviderPassportRow>();
        }
    }
}