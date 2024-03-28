namespace Bars.Gkh.Ris.Extractors.HouseManagement.HouseData
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Экстрактор данных по подъездам
    /// </summary>
    public class LiftDataExtractor : BaseSlimDataExtractor<RisLift>
    {

        public IDictionaryManager DictionaryManager { get; set; }

        /// <summary>
        /// Получить сущности сторонней системы - лифт
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы - подъезды</returns>
        public override List<RisLift> Extract(DynamicDictionary parameters)
        {
            var result = new List<RisLift>();

            var houses = parameters.GetAs<List<RisHouse>>("houses");
            var houseIds = houses?.Select(x => x.ExternalSystemEntityId).ToArray() ?? new long[0];

            var tehPassportValueDomain = this.Container.ResolveDomain<TehPassportValue>();
            var liftTypeDictionary = this.DictionaryManager.GetDictionary("LiftTypeDictionary");


            try
            {
                var needTehPassportValues = tehPassportValueDomain.GetAll()
                    .WhereIf(houses != null, x => houseIds.Contains(x.TehPassport.RealityObject.Id))
                    .Where(x => x.FormCode == "Form_4_2_1")
                    .GroupBy(x => x.TehPassport.RealityObject.Id)
                    .ToArray();

                foreach (var groupTehPassport in needTehPassportValues)
                {
                    var i = 0;

                    do
                    {
                        var lift = new RisLift();

                        //Номер подъезда
                        lift.EntranceNum = groupTehPassport?.Where(x => x.FormCode == "Form_4_2_1" && x.CellCode == i + ":1")
                            .Select(x => x.Value).FirstOrDefault();

                        //Номер лифта (заводской)
                        lift.FactoryNum = groupTehPassport?.Where(x => x.FormCode == "Form_4_2_1" && x.CellCode == i + ":2")
                            .Select(x => x.Value).FirstOrDefault();

                        //Тип лифта
                        var typeCode = groupTehPassport?.Where(x => x.FormCode == "Form_4_2_1" && x.CellCode == i + ":19")
                            .Select(x => x.Value).FirstOrDefault().ToInt()??0;

                        //Код лифта в системе ГИС
                        lift.TypeCode = liftTypeDictionary.GetDictionaryRecord(typeCode)?.GisCode;

                        //GUID лифта в системе ГИС
                        lift.TypeGuid = liftTypeDictionary.GetDictionaryRecord(typeCode)?.GisGuid;

                        //Предельный срок эксплуатации(год)
                        lift.OperatingLimit = groupTehPassport?.Where(x => x.FormCode == "Form_4_2_1" && x.CellCode == i + ":12")
                            .Select(x => x.Value).FirstOrDefault();

                        lift.ApartmentHouse = houses?
                            .FirstOrDefault(x => x.ExternalSystemEntityId == groupTehPassport?.FirstOrDefault()?.TehPassport.RealityObject.Id);

                        lift.ExternalSystemEntityId = groupTehPassport?.FirstOrDefault()?.TehPassport.RealityObject.Id ?? 0;

                        //дата сноса
                        lift.TerminationDate = groupTehPassport?.FirstOrDefault()?.TehPassport.RealityObject.DateDemolition;
                        lift.ExternalSystemEntityId = groupTehPassport?.FirstOrDefault()?.TehPassport?.RealityObject?.Id ?? 0;
                        lift.ExternalSystemName = "Lift " + lift.EntranceNum;
                        i++;
                        result.Add(lift);
                    }
                    while (groupTehPassport.Any(x => (x.CellCode.Remove(2) == i + ":") && (x.CellCode != "1:1") && (x.CellCode != "2:1")));
                }
            }
            finally
            {
                this.Container.Release(tehPassportValueDomain);

            }

            return this.PrepareRisEntities(result);
        }

        /// <summary>
        /// Подготовить рис-объекты 
        /// </summary>
        /// <param name="lifts">Собранный список лифтов</param>
        /// <returns>Список лифтов</returns>
        private List<RisLift> PrepareRisEntities(List<RisLift> lifts)
        {
            var liftDomain = this.Container.ResolveDomain<RisLift>();
            var result = new List<RisLift>();

            try
            {
                foreach (var lift in lifts)
                {
                    var gisLift = liftDomain.GetAll()
                        .FirstOrDefault(x => x.ApartmentHouse.Id == lift.ApartmentHouse.Id && x.EntranceNum == lift.EntranceNum && x.Guid != null);

                    if (gisLift != null)
                    {
                        this.UpdateLiftRow(lift, ref gisLift);
                        gisLift.Operation = RisEntityOperation.Update;

                        result.Add(gisLift);
                    }
                    else
                    {
                        lift.Operation = RisEntityOperation.Create;
                        result.Add(lift);
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, result);

                return result;
            }
            finally
            {
                this.Container.Release(liftDomain);
            }
        }

        /// <summary>
        /// Обновить значения в существующей в таблице сущности "Лифт"
        /// </summary>
        /// <param name="newLift">Новые значения лифта</param>
        /// <param name="oldLift">Старые значения лифта</param>
        public void UpdateLiftRow(RisLift newLift, ref RisLift oldLift)
        {
            oldLift.FactoryNum = newLift.FactoryNum;
            oldLift.OperatingLimit = newLift.OperatingLimit;
            oldLift.TerminationDate = newLift.TerminationDate;
            oldLift.OgfDataValue = newLift.OgfDataValue;
            oldLift.OgfDataCode = newLift.OgfDataCode;
            oldLift.TypeCode = newLift.TypeCode;
            oldLift.TypeGuid = newLift.TypeGuid;
        }
    }
}
