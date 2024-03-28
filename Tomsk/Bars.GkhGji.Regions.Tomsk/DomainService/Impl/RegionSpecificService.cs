namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;

    using Castle.Windsor;

    public class RegionSpecificService : IRegionSpecificService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<AppealCits> AppealCitsService { get; set; }

        public IDomainService<AppealCitsRealityObject> AppealCitsRealityObjectService { get; set; }

        public IDomainService<ManOrgContractRealityObject> ManOrgContractRealityObjectService { get; set; }

        public IDataResult GetAppealCitizenResponder(BaseParams baseParams)
        {
            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");

            var citizenAppeal = AppealCitsService.GetAll().FirstOrDefault(x => x.Id == appealCitizensId);

            var realityObjects = AppealCitsRealityObjectService.GetAll()
                .Where(x => x.AppealCits.Id == appealCitizensId)
                .Select(x => new { x.RealityObject.Id, x.RealityObject.Address })
                .ToList();

            var realityObject = realityObjects.Any() ? realityObjects.First() : null;

            if (citizenAppeal == null || realityObject == null)
            {
                return new BaseDataResult();
            }

            var contragent = ManOrgContractRealityObjectService.GetAll()
                .Where(x => x.ManOrgContract.StartDate <= citizenAppeal.DateFrom)
                .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= citizenAppeal.DateFrom)
                .Where(x => x.RealityObject.Id == realityObject.Id)
                .Select(x => new 
                { 
                    Id = (long?)x.ManOrgContract.ManagingOrganization.Contragent.Id,
                    x.ManOrgContract.ManagingOrganization.Contragent.Name
                })
                .FirstOrDefault();

            return new BaseDataResult(new 
            { 
                Id = contragent != null ? contragent.Id : null,
                Name = contragent != null ? contragent.Name : null,
                roId = realityObjects.Count == 1 ? (long?)realityObject.Id : null,
                roAddress = realityObjects.Count == 1 ? realityObject.Address : null
            });
        }

        public IDataResult CreateNewBaseStatment(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                var service = Container.Resolve<IDomainService<BaseStatement>>();
                var serviceAppeal = Container.Resolve<IDomainService<AppealCits>>();
                var serviceBaseStatementAppeal = Container.Resolve<IDomainService<InspectionAppealCits>>();
                var servicePrimaryBaseStatementAppeal = Container.Resolve<IDomainService<PrimaryBaseStatementAppealCits>>();
                var serviceContragent = Container.Resolve<IDomainService<Contragent>>();
                
                try
                {
                    var contragentId = baseParams.Params.GetAs<long>("contragentId");
                    var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizenId");

                    var dict = baseParams.Params["baseStatement"] as DynamicDictionary;

                    if (dict.ContainsKey("Contragent") && dict["Contragent"] is long)
                    {
                        dict["Contragent"] = new Contragent { Id = ((long)dict["Contragent"]) };
                    }

                    var baseStatement = baseParams.Params.GetAs<BaseStatement>("baseStatement");
                    
                    if (contragentId > 0)
                    {
                        baseStatement.Contragent = serviceContragent.Load(contragentId);
                    }

                    service.Save(baseStatement);

                    if (appealCitizensId > 0)
                    {
                        var appealCit = serviceAppeal.Load(appealCitizensId);

                        var newRec = new InspectionAppealCits
                        {
                            AppealCits = appealCit,
                            Inspection = baseStatement
                        };

                        serviceBaseStatementAppeal.Save(newRec);

                        servicePrimaryBaseStatementAppeal.Save(new PrimaryBaseStatementAppealCits { BaseStatementAppealCits = newRec });
                    }
                    
                    transaction.Commit();
                    return new BaseDataResult { Success = true, Data = baseStatement };
                }
                catch (ValidationException exc)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = exc.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(service);
                    Container.Release(serviceAppeal);
                    Container.Release(serviceContragent);
                    Container.Release(serviceBaseStatementAppeal);
                }
            }
        }

        /// <summary>
        /// Данный метод в томске создает Адресаты в субтаблице адресатов
        /// </summary>
        public IDataResult CreateAnswerAddressee(BaseParams baseParams)
        {

            var serviceAnswer = Container.Resolve<IDomainService<AppealCitsAnswer>>();
            var serviceAddressee = Container.Resolve<IDomainService<AppealCitsAnswerAddressee>>();
            
            try
            {
                // Необходимо получить Все адресаты из Сущности, чтобы перенести их в субтабличку
                var data =
                    serviceAnswer.GetAll()
                                 .Where(x => x.Addressee != null)
                                 .Where(x => !serviceAddressee.GetAll().Any(y => y.Answer.Id == x.Id))
                                 .Select(x => new { x.Id, AddresseeId = x.Addressee.Id })
                                 .ToList();

                var listToSave = new List<AppealCitsAnswerAddressee>();

                foreach (var item in data)
                {
                    listToSave.Add(new AppealCitsAnswerAddressee
                                       {
                                           Answer = new AppealCitsAnswer { Id = item.Id },
                                           Addressee = new RevenueSourceGji { Id = item.AddresseeId }
                                       });
                }

                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {

                        listToSave.ForEach(serviceAddressee.Save);
                        tr.Commit();
                        return new BaseDataResult();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                Container.Release(serviceAnswer);
                Container.Release(serviceAddressee);
            }
        }
    }
}