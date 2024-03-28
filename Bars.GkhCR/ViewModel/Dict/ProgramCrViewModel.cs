namespace Bars.GkhCr.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    public class ProgramCrViewModel : BaseViewModel<ProgramCr>
    {
        public IDomainService<ProgramCrChangeJournal> changeJournalDomain { get; set; } 

        public override IDataResult List(IDomainService<ProgramCr> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var ids = baseParams.Params.GetAs("Id", string.Empty);

            var state = baseParams.Params.GetAs("state", false);

            var dateStartPeriodDi = baseParams.Params.GetAs("dateStartPeriodDi", DateTime.MinValue);
            var dateEndPeriodDi = baseParams.Params.GetAs("dateEndPeriodDi", DateTime.MaxValue);

            var forObjCr = baseParams.Params.GetAs("forObjCr", false);
            var onlyFull = baseParams.Params.GetAs("onlyFull", false);
            var notOnlyHidden = baseParams.Params.GetAs("notOnlyHidden", false);
            var activePrograms = baseParams.Params.GetAs("activePrograms", false);
            var onPrintorFull = baseParams.Params.GetAs("onPrintorFull", false);
            var onlyDpkrCreation = baseParams.Params.GetAs("onlyDpkrCreation", false);
            var forSpecialAccount = baseParams.Params.GetAs("forSpecialAccount", false);

            var listIds = new List<long>();
            if (!string.IsNullOrEmpty(ids))
                {
                    if (ids.Contains(','))
                    {
                        listIds.AddRange(ids.Split(',').Select(id => id.ToLong()));
                    }
                    else
                    {
                        listIds.Add(ids.ToInt());
                    }
                }

            var data = domainService.GetAll()
                .WhereIf(onlyDpkrCreation, x => x.AddWorkFromLongProgram == AddWorkFromLongProgram.Use //программа сформирована из ДПКР или разрешено добавление работ из ДПКР
                                                && x.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Hidden // ТипВидимости Не   Скрытая
                                                && x.TypeProgramStateCr != TypeProgramStateCr.Close  // Состояние Не Закрая
                                                )
                .WhereIf(notOnlyHidden, x => x.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Hidden)
                .WhereIf(onlyFull, x => x.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full)
                .WhereIf(onPrintorFull, x => x.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Print || x.TypeVisibilityProgramCr == TypeVisibilityProgramCr.Full)
                .WhereIf(listIds.Count > 0, x => listIds.Contains(x.Id))
                //Для фильтра в таблице реестра(подгружать в сторе только те id которые выбраны)
                .WhereIf(
                    state,
                    x => x.TypeProgramStateCr != TypeProgramStateCr.New
                    && x.TypeProgramStateCr != TypeProgramStateCr.Open
                    && x.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Hidden
                         && x.Period.DateStart >= dateStartPeriodDi && x.Period.DateStart <= dateEndPeriodDi)
                .WhereIf(
                    forObjCr,
                    x => x.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Hidden
                    && x.TypeVisibilityProgramCr != TypeVisibilityProgramCr.Print)
                .WhereIf(activePrograms, x => x.TypeProgramStateCr == TypeProgramStateCr.Active)
                .WhereIf(forSpecialAccount, x => x.ForSpecialAccount)
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    PeriodName = x.Period.Name,
                    x.UsedInExport,
                    x.NotAddHome,
                    x.Description,
                    x.MatchFl,
                    x.ForSpecialAccount,
                    x.TypeProgramCr,
                    x.TypeProgramStateCr,
                    x.TypeVisibilityProgramCr,
                    x.AddWorkFromLongProgram,
                    x.ImportContract
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}
