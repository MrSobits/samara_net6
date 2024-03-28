namespace Bars.Gkh.DomainService
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;

    // �������� �������� ���� ���� ������������ �� ����� �����
    public class LocalGovernmentDomainService : LocalGovernmentDomainService<LocalGovernment>
    {
        // �������� ��� override ������ � Generic ������
    }

    // Generic, ����� ����� ����������� � ��������� �������
    public class LocalGovernmentDomainService<T>: BaseDomainService<T>
        where T : LocalGovernment
    {
        public override IQueryable<T> GetAll()
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            IQueryable<T> query;

            using(Container.Using(userManager))
            {
                var contragentList = userManager.GetContragentIds();
                var municipalityList = userManager.GetMunicipalityIds();
                query =  base.GetAll()
                .WhereIf(contragentList.Count > 0, x => contragentList.Contains(x.Contragent.Id))
                .WhereIf(municipalityList.Count > 0, x => municipalityList.Contains(x.Contragent.Municipality.Id));
            }
            
            return query;
        }
    }
}