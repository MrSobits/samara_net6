namespace Bars.GisIntegration.Base.Entities.External.Dict.PersonalAccount
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Тип лицевого счета
    /// </summary>
    public class LsType : BaseEntity
    {
        /// <summary>
        /// Тип лицевого счета
        /// </summary>
        public virtual string LsTypeName { get; set; }
    }
}
