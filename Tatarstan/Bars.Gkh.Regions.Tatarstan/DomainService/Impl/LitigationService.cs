namespace Bars.Gkh.Regions.Tatarstan.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Gis.DomainService.BilConnection;
    using Bars.Gkh.Gis.Enum;
    using Bars.Gkh.SqlExecutor;
    using Bars.Gkh.Regions.Tatarstan.Dto.PaymentsGku;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;

    using Castle.Windsor;

    /// <summary>
    /// Реализация сервиса судебных распоряжений по ЖКУ
    /// </summary>
    public class LitigationService : ILitigationService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public string GetIndEntrRegistrationNumbers(long addressId)
        {
            var litigationDomainService = this.Container.ResolveDomain<Litigation>();

            using (this.Container.Using(litigationDomainService))
            {
                var registrationNumbersList = litigationDomainService
                    .GetAll()
                    .Where(x => x.DebtorFsspAddress != null)
                    .Where(x => x.DebtorFsspAddress.PgmuAddress != null)
                    .Where(x => x.DebtorFsspAddress.PgmuAddress.Id == addressId)
                    .Select(x => x.IndEntrRegistrationNumber)
                    .ToList();

                return string.Join(", ", registrationNumbersList);
            }
        }

        /// <inheritdoc />
        public IEnumerable<PaymentGkuListDto> PaymentList(BaseParams baseParams)
        {
            var addressId = baseParams.Params.GetAs<long>("addressId");
            var periodStart = baseParams.Params.GetAs<DateTime>("startDate");
            var periodEnd = baseParams.Params.GetAs<DateTime>("endDate");
            var loadParams = baseParams.GetLoadParam();
            var connectionString = string.Empty;
            var paymentList = new List<PaymentGkuListDto>();
            PgmuAddress address = null;

            var pgmuAddressDomainService = this.Container.ResolveDomain<PgmuAddress>();
            using (this.Container.Using(pgmuAddressDomainService))
            {
                address = pgmuAddressDomainService.Get(addressId);
            }

            if (address == null)
            {
                throw new Exception("Не найден адрес ПГМУ");
            }

            var bilConnectionService = this.Container.Resolve<IBilConnectionService>();
            using (this.Container.Using(bilConnectionService))
            {
                connectionString = bilConnectionService.GetConnection(ConnectionType.GisConnStringPgu);
            }
            
            using (var sqlExecutor = new SqlExecutor(connectionString))
            {
                var currentPeriod = periodStart;
                
                while (currentPeriod <= periodEnd)
                {
                    var sql = $@"SELECT EXISTS (
                                    SELECT 
                                    FROM   information_schema.tables 
                                    WHERE  table_schema = 'public'
                                    AND    table_name   = 'charge_{address.ErcCode}_{currentPeriod:yyyyMM}'
                                );";

                    if (!sqlExecutor.ExecuteScalar<bool>(sql))
                    {
                        currentPeriod = currentPeriod.AddMonths(1);
                        continue;    
                    }
                    
                    sql = 
                        $@"SELECT   '{currentPeriod}'                                   Period,
                                    c.pkod                                              AccountNumber,
                                    SUM(c.sum_insaldo - c.sum_money) DebtSum,
                                    SUM(c.rsum_tarif)                                   Accured,
                                    SUM(c.sum_money)                                    PayedForPreviousMonth
                           FROM     charge_{address.ErcCode}_{currentPeriod:yyyyMM}     c
                           JOIN     parameters_{address.ErcCode}_{currentPeriod:yyyyMM} p
                              ON    p.pkod = c.pkod
                             AND    TRIM(p.rajon) = '{address.District.Trim()}'
                             AND    TRIM(p.town) = '{address.Town.Trim()}'
                             AND    TRIM(p.ulica) = '{address.Street.Trim()}'
                             AND    TRIM(p.ndom) = '{address.House.Trim()}'
                             AND    TRIM(p.nkor) {(address.Building != null ? "='" + address.Building.Trim() + "'" : "IS NULL")}
                             AND    TRIM(p.nkvar) {(address.Apartment != null ? "='" + address.Apartment.Trim() + "'" : "IS NULL")}
                             AND    TRIM(p.nkvar_n) {(address.Room != null ? "='" + address.Room.Trim() + "'" : "IS NULL")}
                           GROUP BY c.pkod";
                    
                    paymentList.AddRange(sqlExecutor.ExecuteSql<PaymentGkuListDto>(sql));
                    currentPeriod = currentPeriod.AddMonths(1);
                }
            }

            return paymentList
                .AsQueryable()
                .Filter(loadParams, this.Container)
                .Order(loadParams)
                .Paging(loadParams)
                .ToList();
        }
    }
}