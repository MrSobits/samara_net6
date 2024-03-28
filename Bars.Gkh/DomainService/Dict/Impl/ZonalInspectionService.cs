namespace Bars.Gkh.DomainService
{
    using System;
    using System.Linq;
    using Castle.Windsor;

    using B4;
    using B4.DataAccess;
    
    using Entities;

    public class ZonalInspectionService : IZonalInspectionService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddInspectors(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var serviceZonalInsp = this.Container.Resolve<IDomainService<ZonalInspection>>();
                var serviceZonalInspector = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
                var serviceInspector = this.Container.Resolve<IDomainService<Inspector>>();

                try
                {
                    var zonalInspectionId = baseParams.Params.GetAs<long>("zonalInspectionId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds") ?? new long[0];

                    var listObjects =
                        serviceZonalInspector.GetAll()
                            .Where(x => x.ZonalInspection.Id == zonalInspectionId)
                            .Select(x => x.Inspector.Id)
                            .Where(x => objectIds.Contains(x))
                            .Distinct()
                            .ToList();

                    var zonalInspection = serviceZonalInsp.Load(zonalInspectionId);

                    foreach (var id in objectIds)
                    {
                        if (listObjects.Contains(id)) 
                            continue;

                        var newRecord = new ZonalInspectionInspector
                            {
                                ZonalInspection = zonalInspection,
                                Inspector = serviceInspector.Load(id)
                            };

                        serviceZonalInspector.Save(newRecord);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
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
                    this.Container.Release(serviceZonalInsp);
                    this.Container.Release(serviceInspector);
                    this.Container.Release(serviceZonalInspector);
                }
            }
        }

        public IDataResult AddMunicipalities(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                var serviceZonalInsp = this.Container.Resolve<IDomainService<ZonalInspection>>();
                var serviceZonalMunicipality = this.Container.Resolve<IDomainService<ZonalInspectionMunicipality>>();
                var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();

                try
                {
                    var zonalInspectionId = baseParams.Params.GetAs<long>("zonalInspectionId");
                    var objectIds = baseParams.Params.GetAs<long[]>("objectIds") ?? new long[0];

                    if(objectIds.Length < 1)
                        return new BaseDataResult(false, "Необходимо выбрать муниципальные образования");

                    var listObjects =
                        serviceZonalMunicipality.GetAll()
                            .Where(x => x.ZonalInspection.Id == zonalInspectionId)
                            .Select(x => x.Municipality.Id)
                            .Where(x => objectIds.Contains(x))
                            .Distinct()
                            .ToList();

                    var zonalInspection = serviceZonalInsp.Load(zonalInspectionId);

                    foreach (var id in objectIds)
                    {
                        if (listObjects.Contains(id))
                            continue;

                        var newRecord = new ZonalInspectionMunicipality
                        {
                            ZonalInspection = zonalInspection,
                            Municipality = serviceMunicipality.Load(id)
                        };

                        serviceZonalMunicipality.Save(newRecord);
                    }

                    transaction.Commit();
                    return new BaseDataResult();
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
                    this.Container.Release(serviceZonalInsp);
                    this.Container.Release(serviceMunicipality);
                    this.Container.Release(serviceZonalMunicipality);
                }
            }
        }

        public IDataResult GetByOkato(BaseParams baseParams)
        {
            var serviceZonalInsp = this.Container.ResolveDomain<ZonalInspection>();
            var okato = baseParams.Params.GetAs<string>("Okato");

            try
            {
                var result = serviceZonalInsp.GetAll().First(x => x.Okato == okato);
                return new BaseDataResult(result);
            }
            finally
            {
                this.Container.Release(serviceZonalInsp);
            }

        }

    }
}