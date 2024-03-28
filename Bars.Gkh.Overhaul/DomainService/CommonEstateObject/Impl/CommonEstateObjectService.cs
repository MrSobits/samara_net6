namespace Bars.Gkh.Overhaul.DomainService.Impl
{
    using System.IO;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.Modules.Reports;

    using Bars.Gkh.Entities.Dicts;

    using Entities;
    using Gkh.Entities.CommonEstateObject;
    using Gkh.Report;

    using Castle.Windsor;

    public class CommonEstateObjectService : ICommonEstateObjectService
    {
        public IWindsorContainer Container { get; set; }

        public virtual Stream PrintReport(BaseParams baseParams)
        {
            var stream = new MemoryStream();

            var report = Container.Resolve<IGkhBaseReport>("StructElementList");

            var reportProvider = Container.Resolve<IGkhReportProvider>();

            //собираем сборку отчета и формируем reportParams
            var reportParams = new ReportParams();
            report.PrepareReport(reportParams);

            //получаем Генератор отчета
            var generatorName = report.GetReportGenerator();

            var generator = Container.Resolve<IReportGenerator>(generatorName);

            reportProvider.GenerateReport(report, stream, generator, reportParams);

            return stream;
        }

        public virtual IDataResult ListForRealObj(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var realObjId = baseParams.Params.GetAs<long>("realObjId");
            var realObjStructElemDomain = Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var roMissingCommonEstObjDomain = Container.Resolve<IDomainService<RealityObjectMissingCeo>>();

            var data =
                Container.Resolve<IDomainService<CommonEstateObject>>().GetAll()
                    .Where(x => !realObjStructElemDomain.GetAll()
                                .Any(y => y.RealityObject.Id == realObjId 
                                    && y.StructuralElement.Group.CommonEstateObject.Id == x.Id))

                    .Where(x => !roMissingCommonEstObjDomain.GetAll()
                                .Any(y => y.RealityObject.Id == realObjId 
                                    && y.MissingCommonEstateObject.Id == x.Id))

                    .Where(x => x.IncludedInSubjectProgramm)
                    .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public virtual IDataResult AddWorks(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                var serviceJob = Container.Resolve<IDomainService<Job>>();
                var serviceStructuralElementWork = Container.Resolve<IDomainService<StructuralElementWork>>();
                var serviceStructuralElement = Container.Resolve<IDomainService<StructuralElement>>();

                try
                {
                    var elementId = baseParams.Params.GetAs<long>("elementId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds");

                    // получаем у контроллера услуги что бы не добавлять их повторно
                    var existRecs =
                        serviceStructuralElementWork.GetAll()
                            .Where(x => x.StructuralElement.Id == elementId)
                            .Where(x => objectIds.Contains(x.Job.Id))
                            .Select(x => x.Job.Id)
                            .ToList();

                    var elemObj = serviceStructuralElement.Load(elementId);

                    foreach (var id in objectIds)
                    {
                        if (existRecs.Contains(id))
                        {
                            continue;
                        }

                        var newStructuralElementWork = new StructuralElementWork
                        {
                            Job = serviceJob.Load(id),
                            StructuralElement = elemObj
                        };

                        serviceStructuralElementWork.Save(newStructuralElementWork);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = e.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    Container.Release(serviceJob);
                    Container.Release(serviceStructuralElementWork);
                    Container.Release(serviceStructuralElement);
                }
            }
        }

        /// <inheritdoc />
        public IDataResult AddFeatureViol(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var structuralElementFeatureViolDomain = this.Container.ResolveDomain<StructuralElementFeatureViol>();

                try
                {
                    var elementId = baseParams.Params.GetAs<long>("elementId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds");

                    // получаем у контроллера услуги что бы не добавлять их повторно
                    var existRecs =
                        structuralElementFeatureViolDomain.GetAll()
                            .Where(x => x.StructuralElement.Id == elementId)
                            .Where(x => objectIds.Contains(x.FeatureViol.Id))
                            .Select(x => x.FeatureViol.Id)
                            .ToList();
                   
                    foreach (var id in objectIds)
                    {
                        if (existRecs.Contains(id))
                        {
                            continue;
                        }

                        var newStructuralElementFeatureViol = new StructuralElementFeatureViol
                        {
                            FeatureViol = new FeatureViolGji {Id = id },
                            StructuralElement = new StructuralElement { Id = elementId }
                        };

                        structuralElementFeatureViolDomain.Save(newStructuralElementFeatureViol);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = e.Message };
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
                finally
                {
                    this.Container.Release(structuralElementFeatureViolDomain);
                }
            }
        }
    }
}