namespace Bars.Gkh1468.Domain.ProviderPassport.Serialize
{
    using Bars.Gkh.Enums;

    public class PassportContragent
    {
        public virtual ContragentType ContragentType { get; set; }

        /// <summary>Наименование</summary>
        public virtual string Name { get; set; }

        /// <summary>ИНН</summary>
        public virtual string Inn { get; set; }

        /// <summary>КПП</summary>
        public virtual string Kpp { get; set; }

        /// <summary>ОГРН</summary>
        public virtual string Ogrn { get; set; }
    }
}