namespace Bars.Gkh.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService.TechPassport;
    using Bars.Gkh.PassportProvider;
    using Bars.Gkh.Services.DataContracts.HousesInfo;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сервис для работы с техпаспортом
    /// </summary>
    public class TehPassportValueService : ITehPassportValueService
    {
        /// <summary>
        /// Домен-сервис значений техпаспорта
        /// </summary>
        public IDomainService<TehPassportValue> TehPassportValueDomain { get; set; }

        /// <summary>
        /// Интерфейс для работы с паспортами
        /// </summary>
        public IPassportProvider PassportProvider { get; set; }

        /// <summary>
        /// Метод возвращает информацию по лифтам
        /// </summary>
        /// <param name="houseIds">Идентификаторы домов</param>
        /// <returns>Словарь с информацией</returns>
        public IDictionary<long, List<LiftInfo>> GetLiftsByHouseIds(long[] houseIds)
        {
            return this.TehPassportValueDomain.GetAll()
                .Where(x => houseIds.Contains(x.TehPassport.RealityObject.Id) && x.FormCode == "Form_4_2_1")
                .AsEnumerable()
                .GroupBy(x => x.TehPassport.RealityObject.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.GroupBy(x => int.Parse(x.CellCode.Split(':').First())).ToDictionary(x => x.Key, x => x.ToList())
                    .Select(
                        x => new LiftInfo
                        {
                            PorchNumber = x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":1")?.Value.ToInt() ?? 0,
                            CommissioningУear = x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":9")?.Value.ToInt() ?? 0,
                            HouseLiftType = x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":19")?.Value
                        }).ToList());
        }

        /// <summary>
        /// Метод возвращает информацию по приборам учёта
        /// </summary>
        /// <param name="houseIds">Идентификаторы домов</param>
        /// <returns>Словарь с информацией</returns>
        public IDictionary<long, List<MeteringDeviceInfo>> GetMeteringDevicesByHouseIds(long[] houseIds)
        {
            return this.TehPassportValueDomain.GetAll()
                .Where(x => houseIds.Contains(x.TehPassport.RealityObject.Id) && x.FormCode == "Form_6_6_2")
                .AsEnumerable()
                .GroupBy(x => x.TehPassport.RealityObject.Id)
                .ToDictionary(
                    x => x.Key,
                    y => y.GroupBy(x => int.Parse(x.CellCode.Split(':').First()))
                        .ToDictionary(x => x.Key, x => x.ToList())
                        .Select(
                            x => new MeteringDeviceInfo
                            {
                                NumberOfMeteringDevice = x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":7")?.Value ?? "0",
                                TypePublicLife = this.PassportProvider.GetTextForCellValue("Form_6_6_2", x.Key + ":1", x.Key.ToString()),

                                MetersInstalled = this.PassportProvider.GetTextForCellValue(
                                    "Form_6_6_2",
                                    x.Key + ":2",
                                    x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":2")?.Value ?? "0"),

                                InterfaceType = this.PassportProvider.GetTextForCellValue(
                                    "Form_6_6_2",
                                    x.Key + ":3",
                                    x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":3")?.Value ?? "0"),

                                UnitAccountingDevice = this.PassportProvider.GetTextForCellValue(
                                    "Form_6_6_2",
                                    x.Key + ":4",
                                    x.Value.FirstOrDefault(v => v.CellCode == x.Key + ":4")?.Value ?? "0"),

                                DateOfCommissioningMeteringDevice = x.Value
                                    .FirstOrDefault(v => v.CellCode == x.Key + ":5")?.Value.ToDateTime() ?? default(DateTime),

                                DateReplacingMeteringDevice = x.Value
                                    .FirstOrDefault(v => v.CellCode == x.Key + ":6")?.Value.ToDateTime() ?? default(DateTime),
                            }).ToList());
        }

        /// <summary>
        /// ћетод возвращает информацию по количеству вентил¤ционных каналов
        /// </summary>
        /// <param name="houseIds">»дентификаторы домов</param>
        /// <returns>—ловарь с информацией</returns>
        public IDictionary<long, VentilationInfo> GetNumVentilationDuctByHouseIds(long[] houseIds)
        {
            var ventilationInfoDict = this.TehPassportValueDomain.GetAll()
                .Where(x => x.FormCode.StartsWith("Form_3_5"))
                .Where(x => houseIds.Contains(x.TehPassport.RealityObject.Id))
                .AsEnumerable()
                .GroupBy(x => x.TehPassport.RealityObject.Id)
                .ToDictionary(
                    x => x.Key,
                    y => new VentilationInfo
                    {
                        TypeVentilation = (y.FirstOrDefault(x => x.FormCode == "Form_3_5" && x.CellCode == "1:3")?.Value),
                        NumVentilationDuct = (y.FirstOrDefault(z => z.FormCode == "Form_3_5_2" && z.CellCode == "1:3")?.Value),
                    });

            return ventilationInfoDict;
        }

        /// <summary>
        /// Метод возвращает информацию по мусоропроводу
        /// </summary>
        /// <param name="houseIds">Идентификаторы домов</param>
        /// <returns>Словарь с информацией</returns>
        public IDictionary<long, GarbageInfo> GetGarbageInfoByHouseIds(long[] houseIds)
        {
            return this.TehPassportValueDomain.GetAll()
                .Where(x => houseIds.Contains(x.TehPassport.RealityObject.Id) && (x.FormCode == "Form_3_7_3" || x.FormCode == "Form_3_7_2"))
                .AsEnumerable()
                .GroupBy(x => x.TehPassport.RealityObject.Id)
                .ToDictionary(x => x.Key, y => new GarbageInfo
                {
                    ConstructionGarbage = this.PassportProvider.GetTextForCellValue(
                        "Form_3_7_2",
                        "1:3",
                        y.FirstOrDefault(z => z.FormCode == "Form_3_7_2" && z.CellCode == "1:3")?.Value ?? "0"),

                    NumberOfRefuseChutes = (y.FirstOrDefault(z => z.FormCode == "Form_3_7_3" && z.CellCode == "5:1")?.Value ?? "0").ToInt(),
                });
        }

        /// <summary>
        /// Метод возвращает информацию по облицовачным материалам
        /// </summary>
        /// <param name="houseIds">Идентификаторы домов</param>
        /// <returns>Словарь с информацией</returns>
        public IDictionary<long, string> GetFacadesByHouseIds(long[] houseIds)
        {
            const string facadesForm = "Form_5_8";

            var tpValueDataByRo = this.TehPassportValueDomain.GetAll()
                .Where(x => houseIds.Contains(x.TehPassport.RealityObject.Id) && x.FormCode == facadesForm)
                .AsEnumerable()
                .GroupBy(x => x.TehPassport.RealityObject.Id)
                .ToDictionary(x => x.Key, x => x.ToList());

            var resultDict = new Dictionary<long, string>();


            foreach (var houseId in houseIds)
            {
                var roValues = tpValueDataByRo.Get(houseId);
                if (roValues.IsEmpty())
                {
                    continue;
                }

                //Facades
                var facades = new List<Facade>();
                //Оштукатуренный
                var facadesPlastered = roValues.FirstOrDefault(x => x.CellCode == "23:1");
                if (facadesPlastered?.Value.ToDecimal() > 0)
                {
                    facades.Add(new Facade { FacadeType = "Оштукатуренный", EditDate = facadesPlastered.ObjectEditDate });
                }
                //Облицованный плиткой
                var facadesTiled = roValues.FirstOrDefault(x => x.CellCode == "26:1");
                if (facadesTiled?.Value.ToDecimal() > 0)
                {
                    facades.Add(new Facade { FacadeType = "Облицованный плиткой", EditDate = facadesTiled.ObjectEditDate });
                }
                //Окрашенный 
                var facadesPrinted = roValues.FirstOrDefault(x => x.CellCode == "9:1");
                if (facadesPrinted?.Value.ToDecimal() > 0)
                {
                    facades.Add(new Facade { FacadeType = "Окрашенный", EditDate = facadesPrinted.ObjectEditDate });
                }
                //Сайдинг
                var facadesSiding = roValues.FirstOrDefault(x => x.CellCode == "27:1");
                if (facadesSiding?.Value.ToDecimal() > 0)
                {
                    facades.Add(new Facade { FacadeType = "Сайдинг", EditDate = facadesSiding.ObjectEditDate });
                }
                //Соответствует материалу стен 
                var facadesTotalAreaFacade = roValues.FirstOrDefault(x => x.CellCode == "22:1");

                //Не заполнено или Соответствует материалу стен(если указана общая площадь)
                if (!facades.Any())
                {
                    facades.Add(
                        facadesTotalAreaFacade?.Value.ToDecimal() > 0
                            ? new Facade { FacadeType = "Соответствует материалу стен" }
                            : new Facade { FacadeType = "Не заполнено" });
                }

                resultDict[houseId] = facades.OrderByDescending(x => x.EditDate).Select(x => x.FacadeType).FirstOrDefault();
            }

            return resultDict;
        }

        /// <summary>
        /// Метод возвращает информацию по сведениям об инженерных системах
        /// </summary>
        /// <param name="houseIds">Идентификаторы домов</param>
        /// <returns>Словарь с информацией</returns>
        public IDictionary<long, EngineeringSystemProxy> GetEngineeringSystemByHouseIds(long[] houseIds)
        {
            Tuple<string, string> typeHeatingForm = new Tuple<string, string>("Form_3_1", "1:3");
            Tuple<string, string> typeHotWaterForm = new Tuple<string, string>("Form_3_2", "1:3");
            Tuple<string, string> typeColdWaterForm = new Tuple<string, string>("Form_3_2_CW", "1:3");
            Tuple<string, string> typePowerForm = new Tuple<string, string>("Form_3_3", "1:3");
            Tuple<string, string> typeSewageForm = new Tuple<string, string>("Form_3_3_Water", "1:3");

            var formCodes = new[]
            {
                typeHeatingForm.Item1,
                typeHotWaterForm.Item1,
                typeColdWaterForm.Item1,
                typePowerForm.Item1,
                typeSewageForm.Item1
            };

            return this.TehPassportValueDomain.GetAll()
                .Where(x => houseIds.Contains(x.TehPassport.RealityObject.Id) && formCodes.Contains(x.FormCode) && x.CellCode == "1:3")
                .AsEnumerable()
                .GroupBy(x => x.TehPassport.RealityObject.Id)
                .ToDictionary(x => x.Key, y => new EngineeringSystemProxy
                {
                    HeatingType = this.PassportProvider.GetTextForCellValue(
                        typeHeatingForm.Item1,
                        typeHeatingForm.Item2,
                        y.FirstOrDefault(z => z.FormCode == typeHeatingForm.Item1)?.Value ?? "0"),

                    HotWaterType = this.PassportProvider.GetTextForCellValue(
                        typeHotWaterForm.Item1,
                        typeHotWaterForm.Item2,
                        y.FirstOrDefault(z => z.FormCode == typeHotWaterForm.Item1)?.Value ?? "0"),

                    ColdWaterType = this.PassportProvider.GetTextForCellValue(
                        typeColdWaterForm.Item1,
                        typeColdWaterForm.Item2,
                        y.FirstOrDefault(z => z.FormCode == typeColdWaterForm.Item1)?.Value ?? "0"),

                    ElectricalType = this.PassportProvider.GetTextForCellValue(
                        typePowerForm.Item1,
                        typePowerForm.Item2,
                        y.FirstOrDefault(z => z.FormCode == typePowerForm.Item1)?.Value ?? "0"),

                    SewerageType = this.PassportProvider.GetTextForCellValue(
                        typeSewageForm.Item1,
                        typeSewageForm.Item2,
                        y.FirstOrDefault(z => z.FormCode == typeSewageForm.Item1)?.Value ?? "0"),
                });
        }

        private class Facade
        {
            public string FacadeType { get; set; }

            public DateTime EditDate { get; set; }
        }
    }
}