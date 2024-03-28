namespace Bars.Gkh.Gis.Entities.Register.HouseRegister
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using B4.Utils;
    using Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Enums;

    /// <summary>
    /// Дом
    /// </summary>
    [Serializable]
    public class HouseRegister : BaseEntity
    {
        /// <summary>
        /// Адрес дома
        /// </summary>
        public virtual FiasAddress FiasAddress { get; set; }

        /// <summary>
        /// Регион
        /// </summary>
        public virtual string Region { get; set; }

        /// <summary>
        /// Район
        /// </summary>
        public virtual string Area { get; set; }

        /// <summary>
        /// Город
        /// </summary>
        public virtual string City { get; set; }

        /// <summary>
        /// Улица
        /// </summary>
        public virtual string Street { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        public virtual string HouseNum { get; set; }

        /// <summary>
        /// Корпус дома
        /// </summary>
        public virtual string BuildNum { get; set; }

        /// <summary>
        /// Общая площадь
        /// </summary>
        public virtual decimal TotalSquare { get; set; }

        /// <summary>
        /// Дата постройки
        /// </summary>
        public virtual DateTime BuildDate { get; set; }

        /// <summary>
        /// Тип дома
        /// </summary>
        public virtual TypeHouse TypeHouse { get; set; }

        /// <summary>
        /// Минимальная этажность
        /// </summary>
        public virtual int MinimumFloors { get; set; }

        /// <summary>
        /// Максимальная этажность
        /// </summary>
        public virtual int MaximumFloors { get; set; }

        /// <summary>
        /// Количество проживающих
        /// </summary>
        public virtual int NumberLiving { get; set; }

        /// <summary>
        /// Количество ИПУ
        /// </summary>
        public virtual int NumberIndividualCounter { get; set; }

        /// <summary>
        /// Дата приватизации первой квартиры
        /// </summary>
        public virtual DateTime PrivatizationDate { get; set; }

        /// <summary>
        /// Количество лифтов
        /// </summary>
        public virtual int NumberLifts { get; set; }

        /// <summary>
        /// Тип крыши
        /// </summary>
        public virtual TypeRoof TypeRoof { get; set; }

        /// <summary>
        /// Материал стен
        /// </summary>
        public virtual WallMaterial WallMaterial { get; set; }

        /// <summary>
        /// Физический износ
        /// </summary>
        public virtual decimal PhysicalWear { get; set; }

        /// <summary>
        /// Количество входов
        /// </summary>
        public virtual int NumberEntrances { get; set; }

        /// <summary>
        /// Материал крыши
        /// </summary>
        public virtual RoofingMaterial RoofingMaterial { get; set; }

        /// <summary>
        /// Тип проекта
        /// </summary>
        public virtual TypeProject TypeProject { get; set; }

        /// <summary>
        /// Система отопления
        /// </summary>
        public virtual HeatingSystem HeatingSystem { get; set; }

        /// <summary>
        /// Количество лицевых счетов
        /// </summary>
        public virtual int NumberAccount { get; set; }

        /// <summary>
        /// Наименование управляющих организаций
        /// </summary>
        public virtual string ManOrgs { get; set; }

        /// <summary>
        /// Поставщик
        /// </summary>
        public virtual string Supplier { get; set; }

        /// <summary>
        /// ИНН поставщика
        /// </summary>
        public virtual long SupplierInn { get; set; }

        /// <summary>
        /// Общая площадь жилых и нежилых помещений в МКД (кв.м.)
        /// </summary>
        public virtual decimal? AreaLivingNotLivingMkd { get; set; }

        /// <summary>
        /// Площадь частной собственности (кв.м.)
        /// </summary>
        public virtual decimal? AreaOwned { get; set; }

        /// <summary>
        /// Количество квартир
        /// </summary>
        public virtual int? NumberApartments { get; set; }

        /// <summary>
        /// Бульвар/Переулок/....
        /// </summary>
        public virtual string StreetAddName { get; set; }

        /// <summary>
        /// Код ЕРЦ
        /// </summary>
        public virtual string CodeErc { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }

        /// <summary>
        /// Возвращает адрес, составленный из полей объекта
        /// </summary>
        /// <returns></returns>
        public virtual string GetFullAddress()
        {
            var address = new List<string> {Region, Area, City, Street};
            if (!HouseNum.IsEmpty()) address.Add("д." + HouseNum);
            if (!BuildNum.IsEmpty()) address.Add("корп." + BuildNum);
            return string.Join(", ", address.Where(x => !x.IsEmpty()));
        }
    }
}
