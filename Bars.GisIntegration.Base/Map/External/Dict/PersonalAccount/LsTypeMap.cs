namespace Bars.GisIntegration.Base.Map.External.Dict.PersonalAccount
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.PersonalAccount;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class LsTypeMap : BaseEntityMap<LsType>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public LsTypeMap() :
            base("D_LS_TYPE")
        {
            //Устанавливаем схему
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("LS_TYPE_ID");
                m.Generator(Generators.Native);
            });

            this.Map(x => x.LsTypeName, "LS_TYPE");
        }
    }
}
