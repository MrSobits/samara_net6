namespace Bars.Gkh.RegOperator.Dto
{
    using B4.Modules.FIAS;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Enums;
    using System.Text.RegularExpressions;

    /// <summary>
    /// dto объект лс (для квитанций)
    /// </summary>
    public class BillingPersonalAccountDto : PersonalAccountDto
    {
        private static readonly Regex FiasRaionRegex = new Regex($@"{(byte)FiasLevelEnum.Raion}_(?<RaionGuid>[\w-]+)#?", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>
        /// Начисление проводится в соответствии с решением собственников
        /// </summary>
        public bool AccuralByOwnersDecision { get; set; }

        /// <summary>
        /// Электронная почта
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Адрес за пределами субъекта
        /// </summary>
        public string AddressOutsideSubject { get; set; }

        /// <summary>
        /// Нас. пункт
        /// </summary>
        public string PlaceName { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        public string StreetName { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        public string HouseNum { get; set; }

        /// <summary>
        /// Литер
        /// </summary>
        public string Letter { get; set; }

        /// <summary>
        /// Корпус
        /// </summary>
        public string Housing { get; set; }

        /// <summary>
        /// Секция
        /// </summary>
        public string Building { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// Способ формирования фонда кр на текущий момент
        /// </summary>
        public string CrFundFormationType
        {
            get
            {
                switch (base.AccountFormationVariant)
                {
                    case Gkh.Enums.CrFundFormationType.RegOpAccount:
                        return "Счет регионального оператора";
                    case Gkh.Enums.CrFundFormationType.SpecialAccount:
                        return "Специальный счет";
                    case Gkh.Enums.CrFundFormationType.SpecialRegOpAccount:
                        return "Специальный счет регионального оператора";
                    default:
                        return "Не определено";
                }
            }
        }

        /// <summary>
        /// Какой адрес использовать для отправки корреспонденции
        /// </summary>
        public virtual AddressType BillingAddressType { get; set; }

        /// <summary>
        /// Фиас адрес жилого дома
        /// </summary>
        public FiasAddress RobjectAddress { get; set; }

        /// <summary>
        /// Фиас адрес юр.лица
        /// </summary>
        public FiasAddress LegalFactAddress { get; set; }

        /// <summary>
        /// Фиас адрес физ. лица
        /// </summary>
        public FiasAddress IndividualFactAddress { get; set; }

        /// <summary>
        /// Идентификатор муниципального района из ФИАС
        /// </summary>
        public string IndividualFactAddressRaionGuid { get; set; }

        /// <summary>
        /// Идентификатор жилого дома фактического адреса физ. лица
        /// </summary>
        public long IndividualFactAddressRoId { get; set; }

        /// <summary>
        /// Жилой дом фактического адреса физ. лица
        /// </summary>
        public RealityObject IndividualFactAddressRo { get; set; }

        /// <summary>
        /// Муниципальный район фактического адреса физ. лица
        /// </summary>
        public string IndividualFactAddressRoMunicipality { get; set; }

        /// <summary>
        /// Поселение фактического адреса физ. лица
        /// </summary>
        public string IndividualFactAddressRoSettlement { get; set; }

        /// <summary>
        /// Почтовый индекс фактического адреса физ. лица
        /// </summary>
        public string IndividualFactAddressPostCode { get; set; }

        /// <summary>
        /// Секция фактического адреса физ. лица
        /// </summary>
        public string IndividualFactAddressBuilding { get; set; }

        /// <summary>
        /// Корпус фактического адреса физ. лица
        /// </summary>
        public string IndividualFactAddressHousing { get; set; }

        /// <summary>
        /// Литер фактического адреса физ. лица
        /// </summary>
        public string IndividualFactAddressLetter { get; set; }

        /// <summary>
        /// Номер дома фактического адреса физ. лица
        /// </summary>
        public string IndividualFactAddressHouseNum { get; set; }

        /// <summary>
        /// Населенный пункт фактического адреса физ. лица
        /// </summary>
        public string IndividualFactAddressPlaceName { get; set; }

        /// <summary>
        /// Улица фактического адреса физ. лица
        /// </summary>
        public string IndividualFactAddressStreetName { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        public virtual decimal SaldoOut { get; set; }

        /// <summary>
        /// Есть ненулевые начисления
        /// </summary>
        public virtual bool HasNonZeroCharges { get; set; }

        /// <summary>
        /// Установить адрес из ФИАС
        /// </summary>
        public void ApplyAddressParts()
        {
            if (this.BillingAddressType != AddressType.FactAddress)
            {
                this.ApplyFias(this.RobjectAddress);
            }
            else
            {
                this.ApplyFias(this.OwnerType == PersonalAccountOwnerType.Legal ? this.LegalFactAddress : this.IndividualFactAddress);
            }
        }

        /// <summary>
        /// Установить фактический адрес на основании жилого дома
        /// </summary>
        public void ApplyIndividialFactRoParts()
        {
            if (this.IndividualFactAddressRo == null)
            {
                return;
            }

            this.IndividualFactAddressRoId = this.IndividualFactAddressRo.Id;
            this.IndividualFactAddressRoMunicipality = this.IndividualFactAddressRo.Municipality.Name;
            this.IndividualFactAddressRoSettlement = this.IndividualFactAddressRo.MoSettlement?.Name;
        }

        /// <summary>
        /// Установить фактический адрес из ФИАС
        /// </summary>
        public void ApplyIndividialFactFiasParts()
        {
            if (this.IndividualFactAddress == null)
            {
                return;
            }

            this.IndividualFactAddressPlaceName = this.IndividualFactAddress.PlaceName;
            this.IndividualFactAddressStreetName = this.IndividualFactAddress.StreetName;
            this.IndividualFactAddressHouseNum = this.IndividualFactAddress.House;
            this.IndividualFactAddressLetter = this.IndividualFactAddress.Letter;
            this.IndividualFactAddressHousing = this.IndividualFactAddress.Housing;
            this.IndividualFactAddressBuilding = this.IndividualFactAddress.Building;
            this.IndividualFactAddressPostCode = this.IndividualFactAddress.PostCode;
            this.IndividualFactAddressRaionGuid = BillingPersonalAccountDto.FiasRaionRegex.Match(this.IndividualFactAddress.AddressGuid ?? string.Empty)
                .Groups["RaionGuid"]?.Value;
        }

        private void ApplyFias(FiasAddress fias)
        {
            if (fias == null)
            {
                return;
            }

            this.PlaceName = fias.PlaceName;
            this.StreetName = fias.StreetName;
            this.HouseNum = fias.House;
            this.Letter = fias.Letter;
            this.Housing = fias.Housing;
            this.Building = fias.Building;
            this.PostCode = fias.PostCode;
        }
    }
}
