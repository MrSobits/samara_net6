using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.Regions.Tatarstan.Entities.ContractService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.Gkh.Regions.Tatarstan.Interceptors
{
    public class ManOrgContractOwnersInterceptor : Gkh.Interceptors.ManOrg.ManOrgContractOwnersInterceptor
    {
        /// <summary>
        /// Домен-сервис "Работа / услуга управляющей организации"
        /// </summary>
        public IDomainService<ManOrgContractService> ManOrgContractServiceDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ManOrgComContractService" />
        /// </summary>
        public IDomainService<ManOrgComContractService> ManOrgComContractServiceDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ManOrgAgrContractService" />"
        /// </summary>
        public IDomainService<ManOrgAgrContractService> ManOrgAgrContractServiceDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ManOrgAddContractService" />"
        /// </summary>
        public IDomainService<ManOrgAddContractService> ManOrgAddContractServiceDomain { get; set; }

        ///<inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<ManOrgContractOwners> service, ManOrgContractOwners entity)
        {
            var serviceIds = this.ManOrgContractServiceDomain.GetAll()
                .Where(x => x.Contract.Id == entity.Id)
                .Select(x => x.Id)
                .ToList();

            this.ManOrgComContractServiceDomain.GetAll()
                .Where(x => serviceIds.Contains(x.Id))
                .Select(x => x.Id)
                .ForEach(x => this.ManOrgComContractServiceDomain.Delete(x));

            this.ManOrgAgrContractServiceDomain.GetAll()
                .Where(x => serviceIds.Contains(x.Id))
                .Select(x => x.Id)
                .ForEach(x => this.ManOrgAgrContractServiceDomain.Delete(x));

            this.ManOrgAddContractServiceDomain.GetAll()
                .Where(x => serviceIds.Contains(x.Id))
                .Select(x => x.Id)
                .ForEach(x => this.ManOrgAddContractServiceDomain.Delete(x));

            return base.BeforeDeleteAction(service, entity);
        }
    }
}