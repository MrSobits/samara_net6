namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Enum;
    using Bars.Gkh.Overhaul.Hmao.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SubProgramCriteriasViewModel : BaseViewModel<SubProgramCriterias>
    {
        public override IDataResult List(IDomainService<SubProgramCriterias> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    Description = GetDescription(x),
                    Operator = x.Operator.User.Login
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        //public override IDataResult Get(IDomainService<SubProgramCriterias> domainService, BaseParams baseParams)
        //{
        //    return new BaseDataResult(
        //        domainService.GetAll()
        //            .Where(x => x.Id == baseParams.Params["id"].To<long>())                    
        //            .Select(x => new
        //            {
        //                x.Id,
        //            })
        //            .FirstOrDefault());
        //}

        #region Private methods

        private object GetDescription(SubProgramCriterias x)
        {
            var criterias = new List<string>();

            if (x.IsStatusUsed)
                criterias.Add("статус: " + x.Status.Name);
            if (x.IsTypeHouseUsed)
                criterias.Add("тип дома: " + EnumToTextHelper.TypeHouseToString(x.TypeHouse));
            if (x.IsConditionHouseUsed)
                criterias.Add("состояние дома: " + EnumToTextHelper.ConditionHouseToString(x.ConditionHouse));
            if (x.IsNumberApartmentsUsed)
                criterias.Add($"количество квартир: {EnumToTextHelper.ConditionToString(x.NumberApartmentsCondition)} {x.NumberApartments}");
            if (x.IsYearRepairUsed)
                criterias.Add($"год последнего капитального ремонта: {EnumToTextHelper.ConditionToString(x.YearRepairCondition)} {x.YearRepair}");
            if (x.IsRepairNotAdvisableUsed)
                criterias.Add($"ремонт нецелесообразен: {EnumToTextHelper.BoolToString(x.RepairNotAdvisable)}");
            if (x.IsNotInvolvedCrUsed)
                criterias.Add($"дом не участвует в программе КР: {EnumToTextHelper.BoolToString(x.NotInvolvedCr)}");
            if (x.IsStructuralElementCountUsed)
                criterias.Add($"количество КЭ: {EnumToTextHelper.ConditionToString(x.StructuralElementCountCondition)} {x.StructuralElementCount}");
            if (x.IsFloorCountUsed)
                criterias.Add($"количество этажей: {EnumToTextHelper.ConditionToString(x.FloorCountCondition)} {x.FloorCount}");
            if (x.IsLifetimeUsed)
                criterias.Add($"Срок службы: {EnumToTextHelper.ConditionToString(x.LifetimeCondition)} {x.Lifetime}");

            return String.Join(", ", criterias.ToArray());
        }        

        #endregion
    }

}
