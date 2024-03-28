namespace Bars.Gkh1468.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Domain;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Entities.Passport;
    using Bars.Gkh1468.Enums;

    using Castle.Windsor;

    using ValueType = Bars.Gkh1468.Enums.ValueType;

    public class HousePassportService : IHousePassportService
    {
        #region Properties
        public IDomainService<PassportStruct> PassportStructDomain { get; set; }

        public IDomainService<HouseProviderPassport> HouseProviderPassportDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        public IStateProvider StateProvider { get; set; }

        public IDomainService<HouseProviderPassportRow> HouseProviderPassportRowDomain { get; set; }

        public IDomainService<MetaAttribute> MetaAttributeService { get; set; }

        public IDomainService<RealityObject> RoDomain { get; set; }

        public IDomainService<HousePassport> PaspService { get; set; }

        public IWindsorContainer Container { get; set; }
        #endregion Properties

        public IDataResult GetPassport(RealityObject realityObject, int year, int month)
        {
            var pasp =
                PaspService.GetAll()
                    .FirstOrDefault(
                        x => x.RealityObject.Id == realityObject.Id && x.ReportYear == year && x.ReportMonth == month);

            if (pasp == null)
            {
                pasp = new HousePassport { ReportYear = year, ReportMonth = month, RealityObject = realityObject };
                PaspService.Save(pasp);
            }

            return new BaseDataResult(pasp);
        }

        public IDataResult GetCurrentPassport(RealityObject realityObject)
        {
            var today = DateTime.Today;
            return this.GetPassport(realityObject, today.Year, today.Month);
        }

        public Dictionary<long, decimal> GetPassportsPercentage(TypeRealObj houseType, int year, int month)
        {
            return PaspService.GetAll()
                .Where(x => x.ReportMonth == month && x.ReportYear == year && x.RealityObject.TypeHouse.To1468RealObjType() == houseType)
                .GroupBy(x => x.RealityObject.Municipality.Id)
                .Select(x => new
                    {
                        x.Key,
                        Percent = x.Average(g => g.Percent)
                    })
                .AsEnumerable()
                .ToDictionary(x => x.Key, y => y.Percent);
        }

        public Dictionary<long, decimal> GetPassportsPercentageByHouse(int year, int month)
        {
            return PaspService.GetAll()
                .Where(x => x.ReportMonth == month && x.ReportYear == year)
                .GroupBy(x => x.RealityObject.Municipality.Id)
                .Select(x => new
                {
                    x.Key,
                    Percent = x.Average(g => g.Percent)
                }).ToList().ToDictionary(x => x.Key, y => y.Percent);
        }

        private bool ValidateValue(HouseProviderPassportRow row, out string msg)
        {
            msg = string.Empty;
            if (row.MetaAttribute.Required && row.Value.IsEmpty())
            {
                msg = "Не заполнено значение обязательного аттрибута";
                return false;
            }

            var validType = true;
            switch (row.MetaAttribute.ValueType)
            {
                case ValueType.Decimal:
                    decimal d_tmp;
                    validType = decimal.TryParse(row.Value, out d_tmp);
                    break;
                    case ValueType.Int:
                    int i_tmp;
                    validType = int.TryParse(row.Value, out i_tmp);
                    break;
            }

            if (!validType)
            {
                msg = "Неверный тип импортируемых данных";
                return false;
            }

            return true;
        }
    }
}