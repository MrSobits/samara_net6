namespace Bars.GkhGji.Regions.Tatarstan.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Orgs;

    using NHibernate.Util;

    public class ControlOrganizationViewModel : BaseViewModel<ControlOrganization>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ControlOrganization> domainService, BaseParams baseParams)
        {
            var controlTypeControlOrgDomain = this.Container.Resolve<IDomainService<ControlOrganizationControlTypeRelation>>();
            var zonalInspectionInspectorDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();
            var tatarstanZonalInspectionDomain = this.Container.Resolve<IDomainService<TatarstanZonalInspection>>();
            var contragentDomain = this.Container.Resolve<IDomainService<Contragent>>();

            using (this.Container.Using(controlTypeControlOrgDomain, zonalInspectionInspectorDomain, tatarstanZonalInspectionDomain, contragentDomain))
            {
                var zonalInspectionInspectors = zonalInspectionInspectorDomain.GetAll()
                    .GroupBy(x => x.ZonalInspection.Id)
                    .ToDictionary(x => x.Key, x => x.ToList());

                return domainService.GetAll()
                .AsEnumerable()
                .Select(x =>
                {
                    //хэш идентификаторов инспекций, относящихся к КНО.
                    var zonalInspectionsHash = tatarstanZonalInspectionDomain.GetAll()
                        .Where(y => y.ControlOrganization.Id == x.Id)
                        .Select(y => y.Id)
                        .ToHashSet();

                    var inspectorsList = zonalInspectionInspectors.Where(y => zonalInspectionsHash.Contains(y.Key)).SelectMany(y => y.Value);

                    return new
                    {
                        x.Id,
                        x.Contragent,
                        ContragentName = x.Contragent.Name,
                        ContragentShortName = x.Contragent.ShortName,
                        ContragentOrganizationForm = x.Contragent.OrganizationForm?.Name,
                        ContragentParentOrgName = x.Contragent.Parent?.Name,
                        ContragentInn = x.Contragent.Inn,
                        ContragentKpp = x.Contragent.Kpp,
                        ContragentJurAddress = x.Contragent.JuridicalAddress,
                        ContragentFactAddress = x.Contragent.FactAddress,
                        ContragentPostAddress = x.Contragent.MailingAddress,
                        ContragentOutsideSubjectAddress = x.Contragent.AddressOutsideSubject,
                        ContragentOgrn = x.Contragent.Ogrn,
                        ContragentDateRegistration = x.Contragent.DateRegistration,
                        ContragentOgrnRegistration = x.Contragent.OgrnRegistration,
                        //подразделения
                        Department = string.Join(", ", tatarstanZonalInspectionDomain.GetAll()
                            .Where(y => y.ControlOrganization.Id == x.Id && y.ZoneName != null)
                            .Select(y => y.ZoneName)),
                        //сотрудник-должность
                        PersonPosition = string.Join(", ", inspectorsList.Select(y => $"{y.Inspector.Fio} - {y.Inspector.Position}")),
                        //виды контроля
                        ControlType = string.Join(", ", controlTypeControlOrgDomain.GetAll()
                            .Where(y => y.ControlOrganization != null && y.ControlOrganization.Id == x.Id)
                            .Select(y => y.ControlType.Name)),
                        //Подчиненные организации
                        ChildOrgs = string.Join(", ", contragentDomain.GetAll()
                            .Where(y => y.Parent.Id == x.Contragent.Id).Select(y => y.ShortName))
                    };
                })
                .ToListDataResult(this.GetLoadParam(baseParams), this.Container);
            }
        }
    }
}
