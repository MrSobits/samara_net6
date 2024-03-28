namespace Bars.Gkh.Ris.Extractors.HouseManagement.HouseData
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Экстрактор данных по подъездам
    /// </summary>
    public class EntranceDataExtractor : BaseDataExtractor<RisEntrance, Entrance>
    {
        private List<RisHouse> houses;
        private Dictionary<long, RisHouse> housesById;

        /// <summary>
        /// Получить сущности сторонней системы - подъезды
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы - подъезды</returns>
        public override List<Entrance> GetExternalEntities(DynamicDictionary parameters)
        {
            var houseIds = this.houses?.Select(x => x.ExternalSystemEntityId).ToArray() ?? new long[0];
            var entranceDomain = this.Container.ResolveDomain<Entrance>();

            try
            {
                return entranceDomain.GetAll()
                    .WhereIf(this.houses != null, x => houseIds.Contains(x.RealityObject.Id))
                    .ToList();
            }
            finally
            {
                this.Container.Release(entranceDomain);
            }
        }

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Заполнить словари
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.houses = parameters.GetAs<List<RisHouse>>("apartmentHouses");

            this.housesById = this.houses?
               .GroupBy(x => x.ExternalSystemEntityId)
               .ToDictionary(x => x.Key, x => x.First());
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="entrance">Сущность внешней системы</param>
        /// <param name="risEntrance">Ris сущность</param>
        protected override void UpdateRisEntity(Entrance entrance, RisEntrance risEntrance)
        {
            risEntrance.ExternalSystemEntityId = entrance.Id;
            risEntrance.ExternalSystemName = "gkh";
            risEntrance.EntranceNum = (short)entrance.Number;
            risEntrance.StoreysCount = entrance.RealityObject.MaximumFloors.HasValue ? (short)entrance.RealityObject.MaximumFloors.Value : (short)1;
            risEntrance.CreationDate = entrance.RealityObject.BuildYear != null
                ? new DateTime(entrance.RealityObject.BuildYear.Value, 1, 1)
                : DateTime.MinValue;
            risEntrance.TerminationDate = entrance.RealityObject.DateDemolition;
            risEntrance.ApartmentHouse = this.housesById?.Get(entrance.RealityObject.Id);
        }
    }
}
