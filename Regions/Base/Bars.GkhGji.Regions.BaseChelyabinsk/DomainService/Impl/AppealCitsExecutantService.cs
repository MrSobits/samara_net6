namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    using Castle.Windsor;

    public class AppealCitsExecutantService : IAppealCitsExecutantService
    {
        public IWindsorContainer Container { get; set; }


        public IDataResult ListAppealOrderExecutant(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var appealId = baseParams.Params.GetAs("appealId", 0L);
            var giserpdocsServ = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();
          

            var data = giserpdocsServ.GetAll()
                .WhereIf(appealId>0, x=> x.AppealCits.Id == appealId)             
                .Select(x => new
                {
                    x.Id,
                    x.Executant.Fio,
                    x.Executant.Position,
                    x.Executant.Phone,
                    x.Executant.Email

                })
                //              .OrderBy(x=>x.Id).Distinct()
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public IDataResult AddExecutants(BaseParams baseParams)
        {
            try
            {
                var userManager = Container.Resolve<IGkhUserManager>();
                var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");
                var inspectorIds = baseParams.Params.GetAs<List<long>>("inspectorIds");
                var performanceDate = baseParams.Params.GetAs<DateTime?>("performanceDate");
                var authorId = baseParams.Params.GetAs<long>("authorId");
                var dfOrderDate = baseParams.Params.GetAs<DateTime?>("dfOrderDate");
                var taDescription = baseParams.Params.GetAs<string>("taDescription");
                var cbIsResponsible = baseParams.Params.GetAs<bool>("cbIsResponsible",false); 

                 var appealCitsExecutantDomain = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();
                if (authorId == 0)
                {
                    var userOperator = userManager.GetActiveOperator();
                    if (userOperator != null)
                    {
                        if (userOperator.Inspector != null)
                        {
                            authorId = userOperator.Inspector.Id;
                        }
                        else
                        {
                            return new BaseDataResult { Success = false, Message = "Не указан инспектор текущего оператора!" };
                        }
                    }
                }

                try
                {
                    foreach (var inspectorId in inspectorIds)
                    {                       
                        var newObj = new AppealCitsExecutant
                        {
                            AppealCits = new AppealCits { Id = appealCitizensId },
                            Executant = new Inspector { Id = inspectorId },
                            OrderDate = dfOrderDate.HasValue ? dfOrderDate.Value : DateTime.Now,
                            PerformanceDate = performanceDate,
                            Description = taDescription,
                            IsResponsible = cbIsResponsible,
                            Author = new Inspector { Id = authorId }
                        };

                        appealCitsExecutantDomain.Save(newObj);                       
                       
                    }

                    if (!inspectorIds.Any())
                    {
                        var newObj = new AppealCitsExecutant
                        {
                            AppealCits = new AppealCits { Id = appealCitizensId },
                            OrderDate = DateTime.Now,
                            PerformanceDate = performanceDate,
                            IsResponsible = false,
                            Author = new Inspector { Id = authorId }
                        };

                        appealCitsExecutantDomain.Save(newObj);
                    }

                    return new BaseDataResult();
                }
                finally 
                {
                    this.Container.Release(appealCitsExecutantDomain);
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
                var appealCitsExecutantId = baseParams.Params.GetAs<long>("executantId");
                var newRecordIds = baseParams.Params.GetAs<long[]>("objectIds");
                var performanceDate = baseParams.Params.GetAs<DateTime?>("performanceDate");
                
                if (newRecordIds == null)
                {
                    return new BaseDataResult { Success = false, Message = "Необходимо выбрать исполнителей!" };
                }

                var appealCitsExecutantDomain = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();
                var stateDomain = this.Container.Resolve<IDomainService<State>>();

                using (this.Container.Using(appealCitsExecutantDomain, stateDomain))
                {
                    var appealCitsExecutant = appealCitsExecutantDomain.Get(appealCitsExecutantId);
                    if (appealCitsExecutant.State.FinalState || appealCitsExecutant.State.Name == "Перенаправлено")
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

                    var listToSave = new List<AppealCitsExecutant>();
                    
                    // Прсотавляем новый статус
                    appealCitsExecutant.State = finalState;

                    // проходим по id вбранных исполнителей и сохздаем новые записи копируя поля
                    foreach (var id in newRecordIds)
                    {
                        listToSave.Add(new AppealCitsExecutant
                        {
                            Executant = new Inspector {Id = id},
                            AppealCits = appealCitsExecutant.AppealCits,
                            Author = appealCitsExecutant.Author,
                            OrderDate = appealCitsExecutant.OrderDate,
                            PerformanceDate = performanceDate,
                            Description = appealCitsExecutant.Description
                        });
                    }

                    using (var tr = this.Container.Resolve<IDataTransaction>())
                    {

                        try
                        {
                            appealCitsExecutantDomain.Update(appealCitsExecutant);
                            
                            listToSave.ForEach(appealCitsExecutantDomain.Save);
                            
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

        public object GetSOPRId(BaseParams baseParams)
        {           

            var AppealOrderDomain = this.Container.Resolve<IDomainService<AppealOrder>>();
          
            var recordId = baseParams.Params.GetAs("recordId", 0L); // это идентификатор Disposal, энтити наследуется от DocumentGji
            if (recordId > 0)
            {
                
                var order  = AppealOrderDomain.GetAll()
                    .Where(x=> x.AppealCits.Id == recordId).FirstOrDefault();

                if (order != null)
                {
                    var data = new
                    {
                        soprId = order.Id
                    };

                    return data;
                }
                else
                {
                    return null;
                }

            }
            else
            {
                return null;
            }
        }
    }
}