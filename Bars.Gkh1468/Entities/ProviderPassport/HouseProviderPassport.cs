namespace Bars.Gkh1468.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Entities.Passport;
    using Bars.Gkh1468.Enums;

    /// <summary>
    /// Паспорт поставщика дома
    /// </summary>
    public class HouseProviderPassport : BaseProviderPassport
    {
        /// <summary>
        /// Паспорт дома
        /// </summary>
        public virtual HousePassport HousePassport { get; set; }
        
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
        
        /// <summary>
        /// Тип дома
        /// </summary>
        public virtual HouseType HouseType { get; set; }

        /// <summary>
        /// Последний изменивший статус пользователь
        /// </summary>
        public virtual string UserName { get; set; }
    }
}