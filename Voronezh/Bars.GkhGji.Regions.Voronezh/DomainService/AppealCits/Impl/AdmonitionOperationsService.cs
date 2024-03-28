namespace Bars.GkhGji.Regions.Voronezh.DomainService
{
    using Entities;  

    using System.Linq;
    using Bars.B4;
    using Castle.Windsor;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.B4.DataAccess;
    using System;
    using System.Collections.Generic;

    using Microsoft.Extensions.Logging;

    public class AdmonitionOperationsService : IAdmonitionOperationsService
    {
        public IWindsorContainer Container { get; set; }

        public virtual IDataResult SaveViolations(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var serviceInspectionViol = this.Container.Resolve<IDomainService<AppCitAdmonVoilation>>();
                var serviceViolation = this.Container.Resolve<IDomainService<ViolationGji>>();
                var serviceAdmonViolation = this.Container.Resolve<IDomainService<AppCitAdmonVoilation>>();

                try
                {
                    var admonId = baseParams.Params.GetAs<long>("admonId");
                    var violations = baseParams.Params.GetAs<List<AppCitAdmonVoilationProxy>>("violations");
                    var existsViolation = serviceAdmonViolation.GetAll().Where(x => x.AppealCitsAdmonition.Id == admonId)
                        .Select(x => x.ViolationGji.Id).ToList(); ;


                    if (admonId > 0 && violations != null)
                    {
                        foreach (var viol in violations)
                        {
                            if (viol.ViolationGjiId > 0 && !existsViolation.Contains(viol.ViolationGjiId))
                            {
                                var newObj = new AppCitAdmonVoilation
                                {
                                    ViolationGji = new ViolationGji { Id = viol.ViolationGjiId },
                                    AppealCitsAdmonition = new AppealCitsAdmonition { Id = admonId },
                                    PlanedDate = viol.PlanedDate,
                                    FactDate = viol.FactDate
                                };
                                serviceInspectionViol.Save(newObj);
                            }
                        }

                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = e.Message };
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    this.Container.Resolve<ILogger>().LogError(e, e.Message);
                    return new BaseDataResult { Success = false, Message = e.Message };
                }
                finally
                {
                    this.Container.Release(serviceInspectionViol);
                    this.Container.Release(serviceViolation);
                }
            }
        }

        /// <summary>
        /// Добавить обращения
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult SaveAppeal(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDomainService<AppCitAdmonAppeal>>();

            try
            {
                var admonId = baseParams.Params.ContainsKey("admonId")
                                           ? baseParams.Params["admonId"].ToLong()
                                           : 0;

                var objectIds = baseParams.Params.ContainsKey("objectIds")
                                    ? baseParams.Params["objectIds"].ToString()
                                    : "";

                if (!string.IsNullOrEmpty(objectIds))
                {
                    // в этом списке будут обращения которые уже связаны с этой инспекцией (чтобы недобавлять несколько одинаковых)
                    var listObjects =
                        service.GetAll()
                               .Where(x => x.AppealCitsAdmonition.Id == admonId)
                               .ToList();
                    var oldIds = listObjects.Select(rac => rac.AppealCits.Id).Distinct().ToArray();

                    var newIds = objectIds.Split(',').Select(x => x.ToLong()).ToArray();
                    var idsToAdd = newIds.Except(oldIds).ToArray();
                    var idsToRemove = oldIds.Except(newIds).ToArray();

                    foreach (var newId in idsToAdd)
                    {
                        var newObj = new AppCitAdmonAppeal
                        {
                            AppealCitsAdmonition = new AppealCitsAdmonition { Id = admonId },
                            AppealCits = new AppealCits { Id = newId }
                        };

                        service.Save(newObj);
                    }

                    //foreach (var id in idsToRemove)
                    //{
                    //    var objToRemove = listObjects.Single(rac => rac.AppealCits.Id == id);
                    //    service.Delete(objToRemove.Id);
                    //}
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(new { success = false, message = e.Message });
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        /// <summary>
		/// Удалить связанные обращения
		/// </summary>
		/// <param name="id">Идентификатор дочернего обращения</param>
		/// <param name="parentId">Идентификатор родительского обращения</param>
		/// <returns>Результат выполнения запроса</returns>
        public IDataResult RemoveRelated(long admonId, long appealNumber)
        {
            var service = this.Container.Resolve<IDomainService<AppCitAdmonAppeal>>();
            var appealService = this.Container.Resolve<IDomainService<AppealCits>>();

            var appeal = service.GetAll().FirstOrDefault(x => x.Id == appealNumber);
            try
            {
                var relation = service.GetAll().FirstOrDefault(x => x.AppealCitsAdmonition.Id == admonId && x.AppealCits.Id == appeal.AppealCits.Id);
                if (relation != null)
                {
                    service.Delete(relation.Id);
                }
                return new BaseDataResult();
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public IDataResult ListDocsForSelect(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var docsServ = this.Container.Resolve<IDomainService<AppealCitsAdmonition>>();

            var data = docsServ.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNumber,
                    x.DocumentName,
                    x.DocumentDate
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();
            Container.Release(docsServ);

            return new ListDataResult(data.ToArray(), totalCount);
        }
        private class AppCitAdmonVoilationProxy
        {
            public long Id { get; set; }
            public long ViolationGjiId { get; set; }
            public DateTime? PlanedDate { get; set; }
            public DateTime? FactDate { get; set; }

        }




    }
}