namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Gkh.Entities;
    using Entities;
    using Enums;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с основаниями обращениями граждан
    /// </summary>
    public class BaseStatementService : IBaseStatementService
    {
        /// <summary>
        /// Windsor-контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис по таблицам связи тематик, подтематик и характеристик
        /// </summary>
        public IDomainService<AppealCitsStatSubject> AppealStatSubjectDomain { get; set; }

        /// <summary>
        /// Метод добавления обращения граждан
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public virtual IDataResult AddAppealCitizens(BaseParams baseParams)
        {
            var inspectionId = baseParams.Params.GetAs<long>("inspectionId");
            var objectIds = baseParams.Params.GetAs("objectIds", string.Empty);

            var servBaseStatementAppealCitizens = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var servAppealCitRealObj = Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var servInspectionRealityObj = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var servBaseStatement = Container.Resolve<IDomainService<BaseStatement>>();
            var servAppealCits = Container.Resolve<IDomainService<AppealCits>>();
            var serInspectionGjis = Container.Resolve<IDomainService<InspectionGji>>();

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var listObjects = ArrayToDeleteBaseStatementAppealCits(inspectionId);

                    foreach (var id in listObjects)
                    {
                        servBaseStatementAppealCitizens.Delete(id);
                    }

                    if (!string.IsNullOrEmpty(objectIds))
                    {
                        foreach (var newId in objectIds.Split(',').Select(x => x.ToLong()))
                        {
                            servBaseStatementAppealCitizens.Save(new InspectionAppealCits
                            {
                                Inspection = servBaseStatement.Load(inspectionId),
                                AppealCits = servAppealCits.Load(newId)
                            });
                        }

                        var appealsIds = objectIds.Split(',').Select(x => x.ToLong()).ToList();

                        // Проверяемые дома обращения которых нет в проверки
                        var realObjs = servAppealCitRealObj.GetAll()
                            .Where(y => appealsIds.Contains(y.AppealCits.Id) &&
                                !servInspectionRealityObj.GetAll().Any(x => x.Inspection.Id == inspectionId && x.RealityObject.Id == y.RealityObject.Id))
                            .ToArray();

                        // Добавляем  дома в проверку
                        foreach (var realObj in realObjs.Distinct(x => x.RealityObject.Id))
                        {
                            servInspectionRealityObj.Save(new InspectionGjiRealityObject
                                             {
                                                 Inspection = serInspectionGjis.Load(inspectionId),
                                                 RealityObject = realObj.RealityObject
                                             });
                        }
                    }

                    transaction.Commit();
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
                    Container.Release(servBaseStatementAppealCitizens);
                    Container.Release(servAppealCitRealObj);
                    Container.Release(servInspectionRealityObj);
                    Container.Release(servBaseStatement);
                    Container.Release(servAppealCits);
                    Container.Release(serInspectionGjis);
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Метод получения информации по обращению граждан
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public virtual IDataResult GetInfo(BaseParams baseParams)
        {
            try
            {
                var appealCitizensNames = string.Empty;
                var appealCitizensIds = string.Empty;

                var inspectionId = baseParams.Params.GetAs<long>("inspectionId");

                if (inspectionId > 0)
                {
                    this.GetAppealCitizenInfo(ref appealCitizensNames, ref appealCitizensIds, inspectionId);
                }

                return new BaseDataResult(new { appealCitizensNames, appealCitizensIds });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }

        /// <summary>
        /// Метод формирующий список проверок по обращению граждан
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult ListByAppealCits(BaseParams baseParams)
        {
                var loadParam = baseParams.GetLoadParam();

            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");

            var baseStatementIds =
                Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                             .Where(x => x.AppealCits.Id == appealCitizensId)
                    .Select(x => x.Inspection.Id)
                             .ToArray();

                var disposalDict =
                Container.Resolve<IDomainService<Disposal>>().GetAll()
                    .Where(x => baseStatementIds.Contains(x.Inspection.Id) && (x.TypeDisposal == TypeDisposalGji.Base || x.TypeDisposal == TypeDisposalGji.Licensing))
                    .Select(x => new {x.Inspection.Id, x.DocumentNumber, x.DocumentDate})
                             .AsEnumerable()
                             .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => new { x.DocumentDate, x.DocumentNumber }).First());

            var dicisionDict =
                Container.Resolve<IDomainService<Decision>>().GetAll()
                    .Where(x => baseStatementIds.Contains(x.Inspection.Id) && (x.TypeDisposal == TypeDisposalGji.Base || x.TypeDisposal == TypeDisposalGji.Licensing))
                    .Select(x => new { x.Inspection.Id, x.DocumentNumber, x.DocumentDate })
                             .AsEnumerable()
                             .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => new {x.DocumentDate, x.DocumentNumber }).First());
            if (dicisionDict.Count > 0)
            {
                dicisionDict.ForEach(x =>
                {
                    if (!disposalDict.ContainsKey(x.Key))
                    {
                        disposalDict.Add(x.Key, x.Value);
                    }
                });
            }

            var disposalDict2 = Container.Resolve<IDomainService<DocumentGjiInspector>>()
                .GetAll().Where(x=> baseStatementIds.Contains(x.DocumentGji.Inspection.Id))
                .Select(x=> new
                {
                    x.DocumentGji.Inspection.Id,
                    Inspector = x.Inspector.Fio
                }).AsEnumerable().GroupBy(x => x.Id)
                 .ToDictionary(x => x.Key, y => y.Select(x => new { x.Inspector}).First());

            var realtyObjDict =
                Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                             .Where(x => baseStatementIds.Contains(x.Inspection.Id))
                    .Select(x => new {x.Inspection.Id, x.RealityObject.Address})
                             .AsEnumerable()
                             .GroupBy(x => x.Id)
                             .ToDictionary(x => x.Key, x => x.Select(y => y.Address).FirstOrDefault());
            
            var data = Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                    .Where(x => x.AppealCits.Id == appealCitizensId)
                    .Select(x => new
                    {
                    x.Inspection.Id,
                    x.Inspection.InspectionNumber,
                    x.Inspection.TypeBase
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.InspectionNumber,
                    x.TypeBase,
                    DocumentNumber = disposalDict.Get(x.Id).ReturnSafe(y => y.DocumentNumber),
                    DocumentDate = disposalDict.Get(x.Id).ReturnSafe(y => y.DocumentDate),
                    DocumentGjiInspector = disposalDict2.Get(x.Id).ReturnSafe(y => y.Inspector),
                    RealtyObject = realtyObjDict.Get(x.Id)
                    })
                    .AsQueryable()
                    .Filter(loadParam, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }

        /// <summary>
        /// Метод создания проверки по обращению граждан
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public virtual IDataResult CreateWithAppealCits(BaseParams baseParams)
        {
            var service = Container.Resolve<IDomainService<InspectionGji>>();
            var serviceAppeal = Container.Resolve<IDomainService<AppealCits>>();
            var serviceBaseStatementAppeal = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var serviceInspectionRobject = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var serviceAppCitsRobject = Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var serviceContragent = Container.Resolve<IDomainService<Contragent>>();
            var serviceRobject = Container.Resolve<IDomainService<RealityObject>>();
            var serviceBaseStatement = Container.Resolve<IDomainService<BaseStatement>>();
            var serviceBaseProsClaim = Container.Resolve<IDomainService<BaseProsClaim>>();

            var contragentId = baseParams.Params.GetAs<long>("contragentId");
            //var appealCitizensId = baseParams.Params.GetAs("appealCitizensId", 0);
            var inspection = baseParams.Params.GetAs<InspectionGji>("baseStatement");
            var appealCitsIds = baseParams.Params.GetAs<long[]>("appealCits") ?? new long[0];
            var realityObjId = baseParams.Params.GetAs<long>("realtyObjId");

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    IDomainService domainService = null;

                    if (inspection.TypeBase == TypeBase.CitizenStatement)
                    {
                        inspection = baseParams.Params.GetAs<BaseStatement>("baseStatement");

                        domainService = serviceBaseStatement;
                    }
                    else
                    {
                        inspection = CopyPublicProperties(typeof(BaseProsClaim), inspection);

                        var baseProsClaim = (BaseProsClaim)inspection;

                        baseProsClaim.TypeBaseProsClaim = TypeBaseProsClaim.ProvidingSpecialist;
                        baseProsClaim.TypeForm = baseParams.Params.GetAs<TypeFormInspection>("typeForm");

                        domainService = serviceBaseProsClaim;
                    }

                    if (contragentId > 0)
                    {
                        inspection.Contragent = serviceContragent.Load(contragentId);
                    }

                    domainService.Save(inspection);

                    if (realityObjId > 0)
                    {
                        var hasAppCitsWithoutRealObj =
                            appealCitsIds.Except(serviceAppCitsRobject.GetAll()
                                .Where(x => appealCitsIds.Contains(x.AppealCits.Id))
                                .Select(x => x.AppealCits.Id))
                                .Any();

                        if (hasAppCitsWithoutRealObj)
                        {
                            return new BaseDataResult
                                       {
                                           Success = false,
                                           Message = "Необходимо указать адрес в разделе \"Место возникновения\" в обращениях, по которым создается проверка"
                                       };
                        }

                        var newRec = new InspectionGjiRealityObject
                            {
                                Inspection = inspection,
                                RealityObject = serviceRobject.Load(realityObjId)
                            };

                        serviceInspectionRobject.Save(newRec);
                    }

                    SaveBaseStatementAppealCits(inspection, appealCitsIds);

                    transaction.Commit();
                    return new BaseDataResult {Success = true, Data = inspection};
                }
                catch (ValidationException exc)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = exc.Message};
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
                    Container.Release(serviceRobject);
                    Container.Release(serviceContragent);
                    Container.Release(serviceInspectionRobject);
                    Container.Release(serviceBaseStatementAppeal);
                    Container.Release(serviceAppCitsRobject);
                    Container.Release(serviceBaseStatement);
                    Container.Release(serviceBaseProsClaim);
                }
            }
        }

        /// <inheritdoc />
        public virtual IDataResult CreateWithBasis(BaseParams baseParams)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public virtual IDataResult AddBasisDocs(BaseParams baseParams)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Метод проверки наличия заполненой тематики у обращения граждан
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public IDataResult AnyThematics(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("appealCitizensId");

            var any = AppealStatSubjectDomain.GetAll().Any(x => x.AppealCits.Id == id);

            return new BaseDataResult(any, !any ? "Не заполнены тематики" : string.Empty);
        }

        /// <summary>
        /// Метод валидации обращения граждан
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения запроса</returns>
        public virtual IDataResult CheckAppealCits(BaseParams baseParams)
        {
           return AnyThematics(baseParams);
        }

        /// <summary>
        /// Метод получения информации об обращениях граждан
        /// </summary>
        /// <param name="appealCitizensNames">Имена граждан</param>
        /// <param name="appealCitizensIds">Идентификаторы граждан</param>
        /// <param name="inspectionId">Идентификатор проверки ГЖИ</param>
        protected virtual void GetAppealCitizenInfo(ref string appealCitizensNames, ref string appealCitizensIds, long inspectionId)
        {
            // Пробегаемся по обращениям и формируем итоговую строку наименований и строку идентификаторов
            var dataInspectors =
                Container.Resolve<IDomainService<InspectionAppealCits>>().GetAll()
                    .Where(x => x.Inspection.Id == inspectionId)
                    .Select(x => new { x.AppealCits.Id, x.AppealCits.DocumentNumber, x.AppealCits.NumberGji })
                    .ToList();

            foreach (var item in dataInspectors)
            {
                if (!string.IsNullOrEmpty(appealCitizensNames))
                {
                    appealCitizensNames += ", ";
                }

                appealCitizensNames += string.Format("{0} ({1})", item.DocumentNumber, item.NumberGji);

                if (!string.IsNullOrEmpty(appealCitizensIds))
                {
                    appealCitizensIds += ", ";
                }

                appealCitizensIds += item.Id.ToStr();
            }
        }

        /// <summary>
        /// Метод сохранения проверки по обращению граждан
        /// </summary>
        /// <param name="inspection">Проверка ГЖИ</param>
        /// <param name="appealCitsIds">Идентификаторы обращений граждан</param>
        protected virtual void SaveBaseStatementAppealCits(InspectionGji inspection, long[] appealCitsIds)
        {
            var serviceBaseStatementAppeal = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var serviceAppeal = Container.Resolve<IDomainService<AppealCits>>();

            foreach (var appealCitId in appealCitsIds)
            {
                var newRec = new InspectionAppealCits
                {
                    AppealCits = serviceAppeal.Load(appealCitId),
                    Inspection = inspection
                };

                serviceBaseStatementAppeal.Save(newRec);
            }
        }

        /// <summary>
        /// Метод формирующий массив идентификаторов проверок по обращению
        /// </summary>
        /// <param name="inspectionId">Идентификатор проверки ГЖИ</param>
        /// <returns>Массив идентификаторов</returns>
        protected virtual object[] ArrayToDeleteBaseStatementAppealCits(long inspectionId)
        {
            var servBaseStatementAppealCitizens = Container.Resolve<IDomainService<InspectionAppealCits>>();
            
            var array = servBaseStatementAppealCitizens.GetAll()
                .Where(x => x.Inspection.Id == inspectionId)
                .Select(x => (object)x.Id)
                .ToArray();

            return array;
        }

        /// <summary>
        /// Метод клонирующий исходный объект
        /// </summary>
        /// <param name="type">Тип объекта на выходе</param>
        /// <param name="source">Исходный объект</param>
        /// <returns></returns>
        protected virtual InspectionGji CopyPublicProperties(Type type, InspectionGji source)
        {
            var result = Activator.CreateInstance(type);

            foreach (var property in typeof(InspectionGji).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                property.SetValue(result, property.GetValue(source, new object[0]), new object[0]);
            }
            return (InspectionGji) result;
        }

       
    }
}