namespace Bars.Gkh.ViewModel.Dict
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.Gkh1468.Entities;

    public class OrganizationFormViewModel : BaseViewModel<OrganizationForm>
    {
        public override IDataResult List(IDomainService<OrganizationForm> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var goal = baseParams.Params.GetAs<string>("goal");
            var filterPlan = baseParams.Params.GetAs<bool>("filterPlan");

            // Требуется фильтрация для плана проверки?
            if (filterPlan)
            {
                IQueryable<OrganizationForm> data;

                if (goal.IsNotEmpty())
                {
                    data = this.ListDependsGoal(goal).Filter(loadParams, this.Container);
                }
                else
                {
                    data = this.ListDependsGoal(string.Empty).Filter(loadParams, this.Container);
                }

                return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
            }

            return base.List(domainService, baseParams);
        }

        /// <summary>
        /// Возвращает отфильтрованный список в зависимости от кода цели
        /// </summary>
        /// <param name="goal">Код цели</param>
        /// <returns>Список организационно-правовых форм</returns>
        private IQueryable<OrganizationForm> ListDependsGoal(string goal)
        {
            var magOrgDomain = this.Container.ResolveDomain<ManagingOrganization>(); // Управляющая организация
            var servOrgDomain = this.Container.ResolveDomain<ServiceOrganization>(); // Обслуживающая организация (Поставщик жилищных услуг)
            var supplyResourceOrgDomain = this.Container.ResolveDomain<SupplyResourceOrg>(); // Поставщик коммунальных услуг
            var regOperatorDomain = this.Container.ResolveDomain<Modules.RegOperator.Entities.RegOperator.RegOperator>(); // Региональный оператор
            var publicServiceOrgDomain = this.Container.ResolveDomain<PublicServiceOrg>(); // Поставщик ресурсов
            var localGovDomain = this.Container.ResolveDomain<LocalGovernment>(); // Органы местного самоуправления
            var politicAuthorityDomain = this.Container.ResolveDomain<PoliticAuthority>(); // Органы государственной власти

            try
            {
                var manOrg = magOrgDomain.GetAll()
                    .Where(x => x.Contragent != null && x.Contragent.OrganizationForm != null)
                    .Select(x => x.Contragent.OrganizationForm).Distinct().ToArray();
                var manOrgWithoutUk = magOrgDomain.GetAll()
                    .Where(x => x.Contragent != null && x.Contragent.OrganizationForm != null)
                    .Where(x => x.TypeManagement != TypeManagementManOrg.UK)
                    .Select(x => x.Contragent.OrganizationForm).Distinct().ToArray();
                var servOrg = servOrgDomain.GetAll()
                    .Where(x => x.Contragent != null && x.Contragent.OrganizationForm != null)
                    .Select(x => x.Contragent.OrganizationForm).Distinct().ToArray();
                var supplyResourceOrg = supplyResourceOrgDomain.GetAll()
                    .Where(x => x.Contragent != null && x.Contragent.OrganizationForm != null)
                    .Select(x => x.Contragent.OrganizationForm).Distinct().ToArray();
                var regOperatorOrg = regOperatorDomain.GetAll()
                    .Where(x => x.Contragent != null && x.Contragent.OrganizationForm != null)
                    .Select(x => x.Contragent.OrganizationForm).Distinct().ToArray();
                var publicServiceOrg = publicServiceOrgDomain.GetAll()
                    .Where(x => x.Contragent != null && x.Contragent.OrganizationForm != null)
                    .Select(x => x.Contragent.OrganizationForm).Distinct().ToArray();
                var localGovOrg = localGovDomain.GetAll()
                    .Where(x => x.Contragent != null && x.Contragent.OrganizationForm != null)
                    .Select(x => x.Contragent.OrganizationForm).Distinct().ToArray();
                var politicAuthorityOrg = politicAuthorityDomain.GetAll()
                    .Where(x => x.Contragent != null && x.Contragent.OrganizationForm != null)
                    .Select(x => x.Contragent.OrganizationForm).Distinct().ToArray();

                IEnumerable<OrganizationForm> data;
                switch (goal)
                {
                    // Если цели нет
                    case "":
                        data = manOrg.Concat(servOrg)
                            .Concat(supplyResourceOrg)
                            .Concat(regOperatorOrg)
                            .Concat(publicServiceOrg)
                            .Concat(localGovOrg)
                            .Concat(politicAuthorityOrg);
                        break;
                    case "01":
                        data = manOrgWithoutUk
                            .Concat(servOrg)
                            .Concat(supplyResourceOrg)
                            .Concat(regOperatorOrg)
                            .Concat(publicServiceOrg);
                        break;
                    case "02":
                        data = localGovOrg.Concat(politicAuthorityOrg);
                        break;
                    case "03":
                        data = magOrgDomain.GetAll()
                            .Where(x => x.Contragent != null && x.Contragent.OrganizationForm != null)
                            .Select(x => x.Contragent.OrganizationForm).Distinct().ToArray();
                        break;
                    case "04":
                        data = manOrgWithoutUk;
                        break;
                    case "05":
                        data = magOrgDomain.GetAll()
                            .Where(x => x.Contragent != null && x.Contragent.OrganizationForm != null)
                            .Where(x => x.TypeManagement == TypeManagementManOrg.UK)
                            .Select(x => x.Contragent.OrganizationForm).Distinct().ToArray();
                        break;
                    default:
                        data = Enumerable.Empty<OrganizationForm>().AsQueryable();
                        break;
                }

                return data.Distinct().AsQueryable();
            }
            catch (Exception)
            {
                return Enumerable.Empty<OrganizationForm>().AsQueryable();
            }
            finally
            {
                this.Container.Release(magOrgDomain);
                this.Container.Release(servOrgDomain);
                this.Container.Release(supplyResourceOrgDomain);
                this.Container.Release(regOperatorDomain);
                this.Container.Release(publicServiceOrgDomain);
                this.Container.Release(localGovDomain);
                this.Container.Release(politicAuthorityDomain);
            }
        }
    }
}