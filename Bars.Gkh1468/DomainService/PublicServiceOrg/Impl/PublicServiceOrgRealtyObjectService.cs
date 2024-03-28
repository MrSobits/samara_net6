namespace Bars.Gkh1468.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    using Castle.Windsor;

    using Entities;

    /// <summary>
    /// Сервис по работе с домами РСО
    /// </summary>
    public class PublicServiceOrgRealtyObjectService : IPublicServiceOrgRealtyObjectService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PublicServiceOrgRealtyObject"/>
        /// </summary>
        public IDomainService<PublicServiceOrgRealtyObject> DomainPublicServiceOrgRealObj { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PublicServiceOrgContractRealObj"/>
        /// </summary>
        public IDomainService<PublicServiceOrgContractRealObj> RealObjPublicServiceOrgRealObjDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PublicServiceOrgContract"/>
        /// </summary>
        public IDomainService<PublicServiceOrgContract> RealObjPublicServiceOrgDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PublicServiceOrg"/>
        /// </summary>
        public IDomainService<PublicServiceOrg> DomainPublicServiceOrg { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="RealityObject"/>
        /// </summary>
        public IDomainService<RealityObject> DomainRealObj { get; set; }

        /// <inheritdoc />
        public IDataResult AddRealtyObjects(BaseParams baseParams)
        {
            var publicSerOrgId = baseParams.Params.GetAs<long>("publicServOrgId");
            var roIds = baseParams.Params.GetAs<long[]>("roIds");

            // Получаем существующие записи Жилых домов
            var existRecs = this.DomainPublicServiceOrgRealObj.GetAll()
                        .Where(x => x.PublicServiceOrg.Id == publicSerOrgId)
                        .Select(x => x.RealityObject.Id)
                        .Distinct()
                        .AsEnumerable()
                        .ToDictionary(x => x);

            var org = this.DomainPublicServiceOrg.Load(publicSerOrgId);

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    // По переданным id если их нет в списке существующих, то добавляем
                    foreach (var id in roIds)
                    {
                        if (existRecs.ContainsKey(id))
                        {
                            continue;
                        }

                        var newObj = new PublicServiceOrgRealtyObject
                        {
                            PublicServiceOrg = org,
                            RealityObject = this.DomainRealObj.Load(id)
                        };

                        this.DomainPublicServiceOrgRealObj.Save(newObj);
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
            }
        }

        /// <inheritdoc />
        public IDataResult AddRealtyObjectsToContract(BaseParams baseParams)
        {
            var contractId = baseParams.Params.GetAs<long>("сontractId");
            var roIds = baseParams.Params.GetAs<long[]>("roIds");

            // Получаем существующие записи Жилых домов
            var existRecs = this.RealObjPublicServiceOrgRealObjDomain.GetAll()
                .Where(x => x.RsoContract.Id == contractId)
                .Select(x => x.RealityObject.Id)
                .Distinct()
                .AsEnumerable()
                .ToDictionary(x => x);

            var contract = this.RealObjPublicServiceOrgDomain.Load(contractId);

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    // По переданным id если их нет в списке существующих, то добавляем
                    foreach (var id in roIds)
                    {
                        if (existRecs.ContainsKey(id))
                        {
                            continue;
                        }

                        var newObj = new PublicServiceOrgContractRealObj { RsoContract = contract, RealityObject = this.DomainRealObj.Load(id) };

                        this.RealObjPublicServiceOrgRealObjDomain.Save(newObj);
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
            }
        }

        /// <inheritdoc />
        public IDataResult ListByRealityObject(BaseParams baseParams)
        {
            var domain = this.Container.ResolveDomain<PublicServiceOrgContractRealObj>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var objectId = baseParams.Params.GetAs<long>("objectId");

                var data = domain.GetAll()
                    .Where(x => x.RealityObject.Id == objectId)
                    .Select(x => new
                        {
                            x.Id,
                            RealityObject = x.RealityObject.Id,
                            PublicServiceOrg = x.RsoContract.PublicServiceOrg.Contragent.Name,
                            PublicServiceOrgId = x.RsoContract.PublicServiceOrg.Id,
                            ContractId = x.RsoContract.Id,
                            x.RsoContract.DateStart,
                            x.RsoContract.DateEnd
                        })
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();
                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(domain);
            }
        }
    }
}