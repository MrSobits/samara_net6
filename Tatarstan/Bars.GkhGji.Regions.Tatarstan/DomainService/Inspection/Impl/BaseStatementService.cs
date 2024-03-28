namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Gkh.Entities;
    using System.Text;

    using Bars.B4.IoC;
    using Bars.B4.Modules.NH.Extentions;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Enums;

    using GkhGji.Entities;

    public class BaseStatementService : Bars.GkhGji.DomainService.BaseStatementService
    {
        private bool CheckAppealCits(AppealCits appealCits, out string errStr)
        {
            errStr = "";
            var msg = new StringBuilder();

            if (!this.CheckThematics(appealCits.Id)) msg.AppendLine("не заполнены тематики;");

            if (!this.CheckAppealCitsRealityObjects(appealCits.Id)) msg.AppendLine("не заполнено место возникновения проблемы;");

            if (!this.CheckAppealCitsConsideration(appealCits)) msg.AppendLine("не заполнено одно или несколько полей вкладки \'Рассмотрение\'");

            if (msg.Length != 0)
            {
                msg.Insert(0, string.Format("Ошибки заполнения обращения {0}: ", appealCits.DocumentNumber)); 
                errStr = msg.ToString();
            }

            return errStr.IsEmpty();
        }

        private bool CheckAppealCitsConsideration(AppealCits appealCits)
        {
            if (appealCits.ExecuteDate == null ||
                appealCits.SuretyDate == null ||
                appealCits.Surety == null ||
                appealCits.Executant == null ||
                appealCits.SuretyResolve == null)
            {
                return false;
            }

            return true;
        }

        private bool CheckThematics(long id)
        {
            return this.AppealStatSubjectDomain.GetAll().Any(x => x.AppealCits.Id == id);
        }

        private bool CheckAppealCitsRealityObjects(long id)
        {
            var servAppealCitRealObj = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();

            return servAppealCitRealObj.GetAll().Any(x => x.AppealCits.Id == id);
        }

        public override IDataResult CheckAppealCits(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("appealCitizensId");
            string msg;
            var res = this.CheckAppealCits(this.Container.Resolve<IDomainService<AppealCits>>().Load(id), out msg);

            return new BaseDataResult(res, !res ? msg : string.Empty);
        }

        public override IDataResult AddAppealCitizens(BaseParams baseParams)
        {
            var inspectionId = baseParams.Params.GetAs<long>("inspectionId");
            var objectIds = baseParams.Params.GetAs("objectIds", string.Empty);

            var servBaseStatementAppealCitizens = this.Container.Resolve<IDomainService<InspectionAppealCits>>();
            var servAppealCitRealObj = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();
            var servInspectionRealityObj = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var servBaseStatement = this.Container.Resolve<IDomainService<BaseStatement>>();
            var servAppealCits = this.Container.Resolve<IDomainService<AppealCits>>();
            var serInspectionGjis = this.Container.Resolve<IDomainService<InspectionGji>>();

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var listObjects = this.ArrayToDeleteBaseStatementAppealCits(inspectionId);

                    foreach (var id in listObjects)
                    {
                        servBaseStatementAppealCitizens.Delete(id);
                    }

                    if (!string.IsNullOrEmpty(objectIds))
                    {
                        var appealsIds = objectIds.Split(',').Select(x => x.ToLong()).ToList();
                        var appealsRemainIds = appealsIds;

                        foreach (var newId in objectIds.Split(',').Select(x => x.ToLong()).ToList())
                        {
                            var appealCits = servAppealCits.Load(newId);

                            string errStr;
                            if (!this.CheckAppealCits(appealCits, out errStr))
                            {
                                throw (new ValidationException(errStr));
                            }

                            servBaseStatementAppealCitizens.Save(new InspectionAppealCits
                            {
                                Inspection = servBaseStatement.Load(inspectionId),
                                AppealCits = appealCits
                            });
                        }

                        // Проверяемые дома обращения которых нет в проверки
                        var realObjs = servAppealCitRealObj.GetAll()
                            .Where(y => appealsRemainIds.Contains(y.AppealCits.Id) &&
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
                    return new BaseDataResult { Success = true };
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
                    this.Container.Release(servBaseStatementAppealCitizens);
                    this.Container.Release(servAppealCitRealObj);
                    this.Container.Release(servInspectionRealityObj);
                    this.Container.Release(servBaseStatement);
                    this.Container.Release(servAppealCits);
                    this.Container.Release(serInspectionGjis);
                }
            }
        }

        /// <inheritdoc />
        public override IDataResult AddBasisDocs(BaseParams baseParams)
        {
            var inspectionId = baseParams.Params.GetAs<long>("inspectionId");
            var objectIds = baseParams.Params.GetAs("objectIds", new long[0]);

            var inspectionDocDomain = this.Container.Resolve<IDomainService<InspectionGjiDocumentGji>>();
            var docDomain = this.Container.Resolve<IDomainService<DocumentGji>>();
            var serInspectionGjis = this.Container.Resolve<IDomainService<InspectionGji>>();

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var existsIds = inspectionDocDomain.GetAll()
                        .Where(x => x.Inspection.Id == inspectionId)
                        .Select(x => x.Id)
                        .ToList();

                    var idsToDelete = existsIds.Where(x => !objectIds.Contains(x))
                        .ToList();

                    var newIds = objectIds.Except(existsIds);

                    idsToDelete.ForEach(x => inspectionDocDomain.Delete(x));

                    if (newIds.Any())
                    {
                        var newDocIds = docDomain.GetAll()
                            .WhereContainsBulked(x => x.Id, newIds)
                            .Select(x => x.Id)
                            .ToList();

                        foreach (var newId in newDocIds)
                        {
                            inspectionDocDomain.Save(new InspectionGjiDocumentGji
                            {
                                Inspection = new InspectionGji { Id = inspectionId },
                                Document = new DocumentGji { Id = newId }
                            });
                        }
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                finally
                {
                    this.Container.Release(inspectionDocDomain);
                    this.Container.Release(docDomain);
                    this.Container.Release(serInspectionGjis);
                }
            }
        }

        public override IDataResult CreateWithAppealCits(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var service = this.Container.Resolve<IDomainService<BaseStatement>>();
                var serviceAppeal = this.Container.Resolve<IDomainService<AppealCits>>();
                var serviceBaseStatementAppeal = this.Container.Resolve<IDomainService<InspectionAppealCits>>();
                var serviceInspectionRobject = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

                var serviceContragent = this.Container.Resolve<IDomainService<Contragent>>();
                var serviceRobject = this.Container.Resolve<IDomainService<RealityObject>>();

                try
                {
                    var contragentId = baseParams.Params.GetAs<long>("contragentId");
                    var baseStatement = baseParams.Params.GetAs<BaseStatement>("baseStatement");
                    var appealCitsIds = baseParams.Params.GetAs<long[]>("appealCits") ?? new long[0];
                    var realityObjId = baseParams.Params.GetAs<long>("realtyObjId");

                    if (contragentId > 0)
                    {
                        baseStatement.Contragent = serviceContragent.Load(contragentId);
                    }

                    baseStatement.RequestType = BaseStatementRequestType.AppealCits;

                    service.Save(baseStatement);

                    //add citizens appeals
                    foreach (var appealCitId in appealCitsIds)
                    {
                        var appealCits = serviceAppeal.Load(appealCitId);
                        
                        string errStr;
                        if (!this.CheckAppealCits(appealCits, out errStr))
                        {
                            throw (new ValidationException(errStr));
                        }

                        var newRec = new InspectionAppealCits
                        {
                            AppealCits = appealCits,
                            Inspection = baseStatement
                        };

                        serviceBaseStatementAppeal.Save(newRec);
                    }

                    if (realityObjId > 0)
                    {
                        var newInspRo = new InspectionGjiRealityObject
                        {
                            Inspection = baseStatement,
                            RealityObject = serviceRobject.Load(realityObjId)
                        };

                        serviceInspectionRobject.Save(newInspRo);
                    }

                    transaction.Commit();

                    return new BaseDataResult{ Success = true, Data = baseStatement };
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
                    this.Container.Release(service);
                    this.Container.Release(serviceAppeal);
                    this.Container.Release(serviceRobject);
                    this.Container.Release(serviceContragent);
                    this.Container.Release(serviceInspectionRobject);
                    this.Container.Release(serviceBaseStatementAppeal);
                }
            }
        }

        public override IDataResult CreateWithBasis(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDomainService<BaseStatement>>();
            var inspRoDomain = this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>();
            var contragentDomain = this.Container.Resolve<IDomainService<Contragent>>();
            var realityObjDomain = this.Container.Resolve<IDomainService<RealityObject>>();

            try
            {
                var contragentId = baseParams.Params.GetAs<long>("contragentId");
                var baseStatement = baseParams.Params.GetAs<BaseStatement>("baseStatement");
                var basisDocIds = baseParams.Params.GetAs<long[]>("basisDocIds") ?? new long[0];
                var realityObjId = baseParams.Params.GetAs<long>("realtyObjId");

                this.Container.InTransaction(() =>
                {
                    if (contragentId > 0)
                    {
                        baseStatement.Contragent = contragentDomain.Load(contragentId);
                    }

                    service.Save(baseStatement);

                    if (baseStatement.RequestType == BaseStatementRequestType.MotivationConclusion)
                    {
                        this.AddDocBasis<MotivationConclusion>(baseStatement, basisDocIds);
                    }
                    else
                    {
                        this.AddAppCitsBasis(baseStatement, basisDocIds);
                    }

                    if (realityObjId > 0)
                    {
                        var newInspRo = new InspectionGjiRealityObject
                        {
                            Inspection = baseStatement,
                            RealityObject = realityObjDomain.Load(realityObjId)
                        };

                        inspRoDomain.Save(newInspRo);
                    }
                });

                return new BaseDataResult(baseStatement);
            }
            catch (ValidationException exc)
            {
                return BaseDataResult.Error(exc.Message);
            }
            finally
            {
                this.Container.Release(service);
                this.Container.Release(realityObjDomain);
                this.Container.Release(contragentDomain);
                this.Container.Release(inspRoDomain);
            }
        }

        /// <inheritdoc />
        public override IDataResult GetInfo(BaseParams baseParams)
        {
            var requestType = baseParams.Params.GetAs("requestType", BaseStatementRequestType.AppealCits);
            if (requestType == BaseStatementRequestType.AppealCits)
            {
                return base.GetInfo(baseParams);
            }

            var inspectionId = baseParams.Params.GetAs<long>("inspectionId");

            var gjiDocDomain =  this.Container.Resolve<IDomainService<InspectionGjiDocumentGji>>();
            var inspectionInfo = gjiDocDomain.GetAll()
                .Where(x => x.Inspection.Id == inspectionId)
                .Where(x => x.Document.TypeDocumentGji == TypeDocumentGji.MotivationConclusion)
                .Select(x => new
                {
                    x.Document.Id,
                    x.Document.DocumentNumber
                })
                .ToList();

            var documentIds = inspectionInfo.Select(x => x.Id).ToArray();
            var documentNumbers = inspectionInfo.Select(x => x.DocumentNumber)
                .WhereNotEmptyString(x => x)
                .AggregateWithSeparator(", ");

            return new BaseDataResult(new { documentIds, documentNumbers });
        }

        private void AddAppCitsBasis(BaseStatement baseStatement, IEnumerable<long> basisDocIds)
        {
            var appCitsDomain = this.Container.ResolveDomain<AppealCits>();
            var refDomain = this.Container.ResolveDomain<InspectionAppealCits>();

            using (this.Container.Using(appCitsDomain, refDomain))
            {
                var existsBasisDocIds = appCitsDomain.GetAll()
                    .WhereContainsBulked(x => x.Id, basisDocIds)
                    .Select(x => x.Id)
                    .ToList();

                foreach (var id in existsBasisDocIds)
                {
                    var newRec = new InspectionAppealCits
                    {
                        AppealCits = new AppealCits { Id = id },
                        Inspection = baseStatement
                    };

                    refDomain.Save(newRec);
                }
            }
        }

        private void AddDocBasis<T>(BaseStatement baseStatement, IEnumerable<long> basisDocIds)
            where T : DocumentGji, new()
        {
            var docDomain = this.Container.ResolveDomain<T>();
            var refDomain = this.Container.ResolveDomain<InspectionGjiDocumentGji>();

            using (this.Container.Using(docDomain, refDomain))
            {
                var existsBasisDocIds = docDomain.GetAll()
                    .WhereContainsBulked(x => x.Id, basisDocIds)
                    .Select(x => x.Id)
                    .ToList();

                foreach (var id in existsBasisDocIds)
                {
                    var newRec = new InspectionGjiDocumentGji
                    {
                        Document = new T { Id = id },
                        Inspection = baseStatement
                    };

                    refDomain.Save(newRec);
                }
            }
        }
    }
}