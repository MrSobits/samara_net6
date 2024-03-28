namespace Bars.Gkh.Overhaul.Hmao.DomainService.Version.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Service for <see cref="DpkrDocumentProgramVersion"/>
    /// </summary>
    public class DpkrDocumentProgramVersionService : IDpkrDocumentProgramVersionService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult GetProgramVersionList(BaseParams baseParams)
        {
            var dpkrDocumentDomain = this.Container.ResolveDomain<DpkrDocument>();
            var programVersionDomain = this.Container.ResolveDomain<ProgramVersion>();
            var dpkrDocumentProgramVersionDomain = this.Container.ResolveDomain<DpkrDocumentProgramVersion>();
            var publishedProgramRecordDomain = this.Container.ResolveDomain<PublishedProgramRecord>();

            using (this.Container.Using(dpkrDocumentDomain, programVersionDomain,
                       dpkrDocumentProgramVersionDomain, publishedProgramRecordDomain))
            {
                var loadParams = baseParams.GetLoadParam();

                var dpkrDocumentId = baseParams.Params.GetAsId("dpkrDocumentId");
                var dpkrDocument = dpkrDocumentDomain.Get(dpkrDocumentId);

                if (dpkrDocument?.DocumentDate == null)
                {
                    return new ListDataResult();
                }

                var availableVersionYears = new[] { dpkrDocument.DocumentDate.Value.Year - 1, dpkrDocument.DocumentDate.Value.Year };
                var availableMunicipalityLevels = new[] { TypeMunicipality.Settlement, TypeMunicipality.UrbanArea };

                var data = programVersionDomain.GetAll()
                    .Where(x => availableVersionYears.Contains(x.VersionDate.Year))
                    .Where(x => availableMunicipalityLevels.Contains(x.Municipality.Level))
                    .Where(x => !dpkrDocumentProgramVersionDomain.GetAll().Any(y => y.ProgramVersion.Id == x.Id))
                    .Where(x => publishedProgramRecordDomain.GetAll().Any(y => y.PublishedProgram.ProgramVersion.Id == x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.Municipality.Name,
                        x.Name,
                        x.VersionDate,
                        x.IsMain
                    })
                    .Filter(loadParams, this.Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }

        /// <inheritdoc />
        public IDataResult AddProgramVersions(BaseParams baseParams)
        {
            var dpkrDocumentId = baseParams.Params.GetAsId("dpkrDocumentId");
            var ids = baseParams.Params.GetAs<long[]>("ids");

            var versions = ids.Select(id => new DpkrDocumentProgramVersion
            {
                DpkrDocument = new DpkrDocument { Id = dpkrDocumentId },
                ProgramVersion = new ProgramVersion { Id = id }
            });

            TransactionHelper.InsertInManyTransactions(this.Container, versions);

            return new BaseDataResult();
        }
    }
}