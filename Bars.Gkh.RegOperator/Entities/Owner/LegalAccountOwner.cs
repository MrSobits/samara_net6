namespace Bars.Gkh.RegOperator.Entities
{
    using B4.Utils;
    using Bars.Gkh.Entities;
    using Enums;

    /// <summary>
    /// Абонент - юр.лицо
    /// </summary>
    public class LegalAccountOwner : PersonalAccountOwner
    {
        public virtual Contragent Contragent { get; set; }

        public override string Name => this.Contragent != null ? this.Contragent.Name : string.Empty;

        public override string Inn => this.Contragent != null ? this.Contragent.Inn : string.Empty;

        public override string Kpp => this.Contragent != null ? this.Contragent.Kpp : string.Empty;

        public virtual bool PrintAct { get; set; }

        public virtual BillingAddress Address
        {
            get
            {
                string result;

                switch (this.BillingAddressType)
                {
                    case AddressType.FactAddress:
                        result = this.Contragent.FactAddress;
                        break;
                    case AddressType.AddressOutsideSubject:
                        result = this.Contragent.AddressOutsideSubject;
                        break;
                    case AddressType.Email:
                        result = this.Contragent.Email;
                        break;
                    case AddressType.JuridicalAddress:
                        result = this.Contragent.JuridicalAddress;
                        break;
                    case AddressType.MailingAddress:
                        result = this.Contragent.MailingAddress;
                        break;
                    default:
                        result = null;
                        break;
                }

                return new BillingAddress { BillingAddressType = this.BillingAddressType, Address = result };
            }
            set
            {
                this.BillingAddressType = value.Return(v => v.BillingAddressType);
            }
        }
    }

    public class BillingAddress
    {
        public AddressType BillingAddressType { get; set; }
        public string Address { get; set; }
    }
}