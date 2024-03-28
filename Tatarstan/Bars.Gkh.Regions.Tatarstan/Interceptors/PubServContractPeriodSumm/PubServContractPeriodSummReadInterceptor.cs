namespace Bars.Gkh.Regions.Tatarstan.Interceptors.PubServContractPeriodSumm
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Интерцептор чтения <see cref="PubServContractPeriodSumm"/>
    /// </summary>
    public class PubServContractPeriodSummReadInterceptor : IDomainServiceReadInterceptor<PubServContractPeriodSumm>
    {
        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <inheritdoc />
        public IQueryable<PubServContractPeriodSumm> BeforeGetAll(IQueryable<PubServContractPeriodSumm> query)
        {
            var contragentIds = this.UserManager.GetContragentIds();
            return query.WhereIf(contragentIds.IsNotEmpty(), x => contragentIds.Contains(x.ContractPeriodSummRso.PublicServiceOrg.Contragent.Id));
        }
    }
}