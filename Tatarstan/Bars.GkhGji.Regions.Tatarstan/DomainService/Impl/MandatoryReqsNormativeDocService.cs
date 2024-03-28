namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Castle.Windsor;

    /// <summary>
    /// Сервис работы с MandatoryReqsNormativeDoc
    /// </summary>
    public class MandatoryReqsNormativeDocService : IMandatoryReqsNormativeDocService
    {
        #region Поля
        private readonly IDomainService<MandatoryReqsNormativeDoc> domainService;
        private readonly IWindsorContainer container;
        private List<string> messages;
        #endregion

        public MandatoryReqsNormativeDocService(IDomainService<MandatoryReqsNormativeDoc> domainService, IWindsorContainer container)
        {
            this.domainService = domainService;
            this.container = container;
        }

        public IDataResult AddUpdateDeleteNpa(BaseParams baseParams)
        {
            var addIds = baseParams.Params.GetAs<long[]>("addIds") ?? new long[0];
            var updateData = baseParams.Params.GetAs<MandatoryReqsNormativeDoc[]>("updateData") ?? new MandatoryReqsNormativeDoc[0];
            var deleteIds = baseParams.Params.GetAs<long[]>("deleteIds") ?? new long[0];
            var mandatoryReqId = baseParams.Params.GetAsId("mandatoryReqId");
            var successes = new List<bool>(3);

            messages = new List<string>(3);

            using (container.Using(domainService))
            {                
                successes.Add(AddNpa(addIds, mandatoryReqId));
                successes.Add(UpdateNpa(updateData, mandatoryReqId));
                successes.Add(DeleteNpa(deleteIds));
            };

            if (successes.Contains(true))
                return new BaseDataResult { Success = true, Message = messages.Any() ? string.Join("\r\n", messages) : "" };
            else
                return new BaseDataResult { Success = false, Message = string.Join("\r\n", messages) };
            
        }
        #region Add, Updata, Delete methods
        private bool AddNpa(long[] addIds, long mandatoryReqId)
        {
            if (addIds.Any())
            {
                try
                {
                    var npaIds = this.domainService.GetAll()
                            .Where(x => x.MandatoryReqs.Id == mandatoryReqId)
                            .Select(x => x.Npa.Id)
                            .ToHashSet();

                    foreach (var id in addIds.Distinct())
                    {
                        if (!npaIds.Contains(id))
                        {
                            var newObj = new MandatoryReqsNormativeDoc
                            {
                                MandatoryReqs = new MandatoryReqs { Id = mandatoryReqId },
                                Npa = new NormativeDoc { Id = id }
                            };

                            this.domainService.Save(newObj);
                        }
                    }                    

                    return true;
                }
                catch (ValidationException e)
                {
                    messages.Add(e.Message);

                    return false;
                }
            }

            return true;
        }

        private bool UpdateNpa(MandatoryReqsNormativeDoc[] updateData, long mandatoryReqId)
        {
            if (updateData.Any())
            {
                try
                {
                    var normativeDocs = this.domainService.GetAll()
                        .Where(w => w.MandatoryReqs.Id == mandatoryReqId)
                        .ToDictionary(d => d.Id);

                    foreach (var updateItem in updateData)
                    {
                        var normativeDoc = normativeDocs[updateItem.Id];
                        normativeDoc.Npa = updateItem.Npa;
                        this.domainService.Update(normativeDoc);
                    }

                    return true;
                }
                catch (ValidationException e)
                {
                    messages.Add(e.Message);

                    return false;
                }
            }

            return true;
        }

        private bool DeleteNpa(long[] deleteIds)
        {
            if (deleteIds.Any())
            {
                try
                {
                    foreach (var id in deleteIds)
                    {
                        this.domainService.Delete(id);
                    }

                    return true;
                }
                catch (ValidationException e)
                {
                    messages.Add(e.Message);

                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}
