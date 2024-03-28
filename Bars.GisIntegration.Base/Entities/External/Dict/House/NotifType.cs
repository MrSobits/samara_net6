namespace Bars.GisIntegration.Base.Entities.External.Dict.House
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Тип новости
    /// </summary>
    public class NotifType : BaseEntity
    {
        /// <summary>
        /// Тип новости
        /// </summary>
        public virtual string NotifTypeName { get; set; }
    }
}
