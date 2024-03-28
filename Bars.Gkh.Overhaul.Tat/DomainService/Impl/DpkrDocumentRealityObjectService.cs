namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class DpkrDocumentRealityObjectService : IDpkrDocumentRealityObjectService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }
        
        /// <inheritdoc />
        public IDataResult GetRealityObjectsList(BaseParams baseParams)
        {
            var isIncluded = baseParams.Params.GetAs<bool>("isIncluded");
            
            var versionRecordDomain = this.Container.ResolveDomain<VersionRecord>();
            var dpkrDocumentRealityObjectDomain = this.Container.ResolveDomain<DpkrDocumentRealityObject>();

            using (this.Container.Using(versionRecordDomain, dpkrDocumentRealityObjectDomain))
            {
                var roIds = dpkrDocumentRealityObjectDomain.GetAll()
                    .Where(x=> x.IsIncluded == isIncluded).Select(x => x.RealityObject.Id);
                
                if (isIncluded)
                {
                    return versionRecordDomain.GetAll()
                        .Where(x=> x.ProgramVersion.IsMain && !roIds.Any(y=> y == x.RealityObject.Id))
                        .Select(x=> new
                        {
                            x.RealityObject.Id,
                            x.RealityObject.Address,
                            Municipality = x.RealityObject.Municipality.Name
                        })
                        .AsEnumerable()
                        .Distinct()
                        .ToListDataResult(baseParams.GetLoadParam(), this.Container);
                }

                return dpkrDocumentRealityObjectDomain.GetAll()
                    .Where(x=> x.IsIncluded && 
                        !roIds.Any(y=> y == x.RealityObject.Id) &&
                        !versionRecordDomain.GetAll().Any(y=> y.ProgramVersion.IsMain && y.RealityObject.Id == x.RealityObject.Id))
                    .Select(x=> new
                    {
                        x.RealityObject.Id,
                        x.RealityObject.Address,
                        Municipality = x.RealityObject.Municipality.Name
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }

        /// <inheritdoc />
        public IDataResult AddRealityObjects(BaseParams baseParams)
        {
            var dpkrDocumentId = baseParams.Params.GetAsId("dpkrDocumentId");
            var roIds = baseParams.Params.GetAs<long[]>("ids");
            var isIncluded = baseParams.Params.GetAs<bool>("isIncluded");
            var dpkrDocumentRealityObjectDomain = this.Container.ResolveDomain<DpkrDocumentRealityObject>();
            
            using (this.Container.Using(dpkrDocumentRealityObjectDomain))
            {
                var roToSave = roIds.Select(id => new DpkrDocumentRealityObject()
                {
                    DpkrDocument = new DpkrDocument() { Id = dpkrDocumentId },
                    RealityObject = new RealityObject() { Id = id },
                    IsIncluded = isIncluded,
                    IsExcluded = !isIncluded
                });

                TransactionHelper.InsertInManyTransactions(this.Container, roToSave);

                return new BaseDataResult();
            }
        }
    }
}