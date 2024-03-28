namespace Bars.GkhGji.Regions.Chelyabinsk.DomainService.MKDLicRequest
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class MKDLicRequestExecutantService : IMKDLicRequestExecutantService
    {
        public IWindsorContainer Container { get; set; }


        public IDataResult ListAppealOrderExecutant(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var requestId = baseParams.Params.GetAs("requestId", 0L);
            var executantDomain = this.Container.Resolve<IDomainService<MKDLicRequestExecutant>>();

            var data = executantDomain.GetAll()
                .WhereIf(requestId > 0, x => x.MKDLicRequest.Id == requestId)
                .Select(x => new
                {
                    x.Id,
                    x.Executant.Fio,
                    x.Executant.Position,
                    x.Executant.Phone,
                    x.Executant.Email

                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public IDataResult AddExecutants(BaseParams baseParams)
        {
            try
            {
                var requestId = baseParams.Params.GetAs<long>("requestId");
                var inspectorIds = baseParams.Params.GetAs<List<long>>("inspectorIds");
                var performanceDate = baseParams.Params.GetAs<DateTime?>("performanceDate");
                var authorId = baseParams.Params.GetAs<long>("authorId");
                var dfOrderDate = baseParams.Params.GetAs<DateTime?>("dfOrderDate");
                var taDescription = baseParams.Params.GetAs<string>("taDescription");
                var cbIsResponsible = baseParams.Params.GetAs<bool>("cbIsResponsible", false);

                var executantDomain = this.Container.Resolve<IDomainService<MKDLicRequestExecutant>>();

                try
                {
                    foreach (var inspectorId in inspectorIds)
                    {
                        var newObj = new MKDLicRequestExecutant
                        {
                            MKDLicRequest = new MKDLicRequest { Id = requestId },
                            Executant = new Inspector { Id = inspectorId },
                            OrderDate = dfOrderDate ?? DateTime.Now,
                            PerformanceDate = performanceDate,
                            Description = taDescription,
                            IsResponsible = cbIsResponsible,
                            Author = new Inspector { Id = authorId }
                        };

                        executantDomain.Save(newObj);
                    }

                    if (!inspectorIds.Any())
                    {
                        var newObj = new AppealCitsExecutant
                        {
                            AppealCits = new AppealCits { Id = requestId },
                            OrderDate = DateTime.Now,
                            PerformanceDate = performanceDate,
                            IsResponsible = false,
                            Author = new Inspector { Id = authorId }
                        };

                        executantDomain.Save(newObj);
                    }

                    return new BaseDataResult();
                }
                finally
                {
                    this.Container.Release(executantDomain);
                }
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        public IDataResult RedirectExecutant(BaseParams baseParams)
        {
            try
            {
                var executantId = baseParams.Params.GetAs<long>("executantId");
                var newRecordIds = baseParams.Params.GetAs<long[]>("objectIds");
                var performanceDate = baseParams.Params.GetAs<DateTime?>("performanceDate");

                if (newRecordIds == null)
                {
                    return new BaseDataResult { Success = false, Message = "Необходимо выбрать исполнителей!" };
                }

                var executantDomain = this.Container.Resolve<IDomainService<MKDLicRequestExecutant>>();
                var stateDomain = this.Container.Resolve<IDomainService<State>>();

                using (this.Container.Using(executantDomain, stateDomain))
                {
                    var executant = executantDomain.Get(executantId);
                    if (executant.State.FinalState || executant.State.Name == "Перенаправлено")
                    {
                        return new BaseDataResult { Success = false, Message = "Перенаправление не возможно. Исполнитель имеет конечный статус!" };
                    }

                    var finalState = stateDomain.GetAll()
                        .Where(x => x.Name == "Перенаправлено")
                        .FirstOrDefault(x => x.TypeId == "gji_appcits_executant");

                    if (finalState == null)
                    {
                        return new BaseDataResult { Success = false, Message = "Для исполнителя обращения отстутствует статус Перенаправлено" };
                    }

                    var listToSave = new List<MKDLicRequestExecutant>();

                    // Прсотавляем новый статус
                    executant.State = finalState;

                    // проходим по id вбранных исполнителей и сохздаем новые записи копируя поля
                    foreach (var id in newRecordIds)
                    {
                        listToSave.Add(new MKDLicRequestExecutant
                        {
                            Executant = new Inspector { Id = id },
                            MKDLicRequest = executant.MKDLicRequest,
                            Author = executant.Author,
                            OrderDate = executant.OrderDate,
                            PerformanceDate = performanceDate,
                            Description = executant.Description
                        });
                    }

                    using (var tr = this.Container.Resolve<IDataTransaction>())
                    {

                        try
                        {
                            executantDomain.Update(executant);

                            listToSave.ForEach(executantDomain.Save);

                            tr.Commit();
                        }
                        catch (Exception)
                        {
                            tr.Rollback();
                            throw;
                        }
                    }

                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }
    }
}