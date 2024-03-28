using Bars.Gkh.Authentification;

namespace Bars.GkhCr.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    public class ProgramCrDomainService : FileStorageDomainService<ProgramCr>
    {
        public IGkhUserManager UserManager { get; set; }

        public IDomainService<ProgramCrChangeJournal> ProgChangeJournalDomain { get; set; }

        public override IDataResult Save(BaseParams baseParams)
        {
            var res = base.Save(baseParams);

            // Если программа успешно создана, и она формируется не на основе ДПКР, то создаем запись в журнале изменений, 
            // если программа формируется на основе ДПКР, то запись журнал заносится после формировнаия ДПКР, 
            // с количеством муниципальных образований, в методе  "CreateProgramCrByDpkr" реализации интерфейса "IDpkrService"
            // Признак "IsCreateByDpkr" беру у первой записи, т.к у нас сохраняется только одна запись

            if (res.Success)
            {
                var firstRec = baseParams.Params["records"].As<List<object>>()
                    .Select(x => x.As<DynamicDictionary>()).First();

                var isCreateByDpkr = firstRec.ContainsKey("IsCreateByDpkr") && firstRec.GetAs<bool>("IsCreateByDpkr");

                var user = UserManager.GetActiveOperator();

                InTransaction(() =>
                {
                    foreach (var record in res.Data.As<List<ProgramCr>>())
                    {
                        if (!isCreateByDpkr)
                        {
                            ProgChangeJournalDomain.Save(new ProgramCrChangeJournal
                            {
                                ProgramCr = record,
                                ChangeDate = DateTime.Now,
                                TypeChange = TypeChangeProgramCr.Manually,
                                UserName = user != null ? user.Name : "Администратор"
                            });
                        }
                    }
                });
            }

            return res;
        }
    }
}
