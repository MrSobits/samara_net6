namespace Bars.GisIntegration.Base.Map.External.Dict.House
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.House;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class NotifTypeMap : BaseEntityMap<NotifType>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public NotifTypeMap() :
            base("D_NOTIF_TYPE")
        {
            //Устанавливаем схему
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("NOTIF_TYPE_ID");
                m.Generator(Generators.Native);
            });

            this.Map(x => x.NotifTypeName, "NOTIF_TYPE");
        }
    }
}
