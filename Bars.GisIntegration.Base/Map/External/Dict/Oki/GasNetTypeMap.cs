﻿namespace Bars.GisIntegration.Base.Map.External.Dict.Oki
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.Oki;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.GasNetType
    /// </summary>
    public class GasNetTypeMap : BaseEntityMap<GasNetType>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public GasNetTypeMap() :
            base("NSI_GAS_NET_TYPE")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("GAS_GAS_NET_TYPE_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.Value, "GAS_NET_TYPE");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
