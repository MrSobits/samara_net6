namespace Bars.GkhCr.DomainService.Impl
{
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using Gkh.Modules.ClaimWork.DomainService;
    using Gkh.Modules.ClaimWork.Entities;
    using Gkh.Modules.ClaimWork.Enums;
    using Modules.ClaimWork.Entities;

    /// <summary>
    /// Реализация IClaimWorkService для модуля CR
    /// </summary>
    public class CrClaimWorkService : IClaimWorkService
    {
        public IWindsorContainer Container { get; set; }

        public ClaimWorkTypeBase TypeBase
        {
            get { return ClaimWorkTypeBase.BuildContract; }
        }

        public ClaimWorkReportInfo ReportInfoByClaimwork(long id)
        {
            var buildContrCwDomain = Container.ResolveDomain<BuildContractClaimWork>();

            try
            {
                var buildContractClaimWork = buildContrCwDomain.Get(id);

                return new ClaimWorkReportInfo
                {
                    Info = buildContractClaimWork.BuildContract.DocumentNum,
                    MunicipalityName = buildContractClaimWork.BuildContract
                    .Return(x => x.Builder)
                    .Return(x => x.Contragent)
                    .Return(x => x.Municipality)
                    .Return(x => x.Name) ?? string.Empty
                };
            }
            finally
            {
                Container.Release(buildContrCwDomain);
            }
        }

        /// <inheritdoc />
        public ClaimWorkReportInfo ReportInfoByClaimworkDetail(long id)
        {
            throw new System.NotImplementedException();
        }
    }
}