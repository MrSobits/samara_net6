using Bars.B4;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.Gkh.Overhaul.Hmao.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    public class DPKRActualCriteriasViewModel : BaseViewModel<DPKRActualCriterias>
    {
        #region Public methods

        public override IDataResult List(IDomainService<DPKRActualCriterias> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Text = GetDescription(x),
                    Operator = x.Operator.User.Login,
                    x.DateStart,
                    x.DateEnd,
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        //public override IDataResult Get(IDomainService<DPKRActualCriterias> domainService, BaseParams baseParams)
        //{
        //    var value =
        //        domainService.GetAll()
        //            .Where(x => x.Id == baseParams.Params["id"].To<long>())
        //            .Select(x => new
        //            {
        //                x.Id,
        //                x.DateStart,
        //                x.DateEnd,
        //                x.Statuses,
        //                x.TypeHouse,
        //                x.ConditionHouse,
        //                x.IsNumberApartments,
        //                x.NumberApartmentsCondition,
        //                x.NumberApartments,
        //                x.IsYearRepair,
        //                x.YearRepairCondition,
        //                x.YearRepair,
        //                x.CheckRepairAdvisable,
        //                x.CheckInvolvedCr,
        //                x.IsStructuralElementCount,
        //                x.StructuralElementCountCondition,
        //                x.StructuralElementCount,
        //            })
        //            .FirstOrDefault();

        //    return new BaseDataResult(value);
        //}

        #endregion

        #region Private methods

        private object GetDescription(DPKRActualCriterias x)
        {
            var criterias = new List<string>();

            if (x.Status != null )
                criterias.Add("статус: " + x.Status.Name);
            if (x.TypeHouse != 0)
                criterias.Add("тип дома: " + EnumToTextHelper.TypeHouseToString(x.TypeHouse));
            if (x.ConditionHouse != 0)
                criterias.Add("состояние дома: " + EnumToTextHelper.ConditionHouseToString(x.ConditionHouse));
            if (x.IsNumberApartments)
                criterias.Add($"количество квартир: {EnumToTextHelper.ConditionToString(x.NumberApartmentsCondition)} {x.NumberApartments}");
            if (x.IsYearRepair)
                criterias.Add($"год последнего капитального ремонта: {EnumToTextHelper.ConditionToString(x.YearRepairCondition)} {x.YearRepair}");
            if (x.CheckRepairAdvisable)
                criterias.Add($"ремонт целесообразен");
            if (x.CheckInvolvedCr)
                criterias.Add($"дом участвует в программе КР");
            if (x.IsStructuralElementCount)
                criterias.Add($"количество КЭ: {EnumToTextHelper.ConditionToString(x.StructuralElementCountCondition)} {x.StructuralElementCount}");
            if (x.SEStatus != null)
                criterias.Add("статус КЭ: " + x.SEStatus.Name);

            return String.Join(", ", criterias.ToArray());
        }

        #endregion
    }
}
