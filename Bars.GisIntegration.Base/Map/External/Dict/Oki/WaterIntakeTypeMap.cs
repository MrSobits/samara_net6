﻿namespace Bars.GisIntegration.Base.Map.External.Dict.Oki
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GisIntegration.Base.Entities.External.Dict.Oki;

    using NHibernate.Mapping.ByCode;

    /// <summary>
    /// Маппинг для Bars.Ris.Contragent.Entities.WaterIntakeType
    /// </summary>
    public class WaterIntakeTypeMap : BaseEntityMap<WaterIntakeType>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public WaterIntakeTypeMap() :
            base("NSI_WATER_INTAKE_TYPE")
        {
            //Устанавливаем схему РИС
            this.Schema("NSI");

            this.Id(x => x.Id, m =>
            {
                m.Column("WATER_INTAKE_TYPE_ID");
                m.Generator(Generators.Native);
            });
            this.Map(x => x.Value, "WATER_INTAKE_TYPE");
            this.Map(x => x.DictCode, "DICT_CODE");
            this.Map(x => x.GisGuid, "GIS_GUID");
            this.Map(x => x.ChangedBy, "CHANGED_BY");
            this.Map(x => x.ChangedOn, "CHANGED_ON");
        }

    }
}
