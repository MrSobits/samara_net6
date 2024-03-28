namespace Bars.GisIntegration.Base.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Package;

    /// <summary>
    /// Пакет данных
    /// </summary>
    public class RisPackage : BaseEntity, IUserEntity, IStorablePackageInfo
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual RisContragent RisContragent { get; set; }

        /// <summary>
        /// Признак делегирования
        /// </summary>
        public virtual bool IsDelegacy { get; set; }

        /// <summary>
        /// Наименование пакета
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Пакет подписан
        /// </summary>
        public virtual bool Signed { get; set; }

        /// <summary>
        /// Идентификатор данных пакета
        /// </summary>
        public virtual long PackageDataId { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        public virtual string UserName { get; set; }
    }
}