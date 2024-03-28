namespace Bars.GkhGji.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class BaseLicenseApplicantsInterceptor : BaseLicenseApplicantsInterceptor<BaseLicenseApplicants>
    {
    }

    /// <summary>
    /// Интерцептор для основания проверки соискателей лицензии
    /// </summary>
    /// <typeparam name="T">Основание проверки соискателей лицензии</typeparam>
    public class BaseLicenseApplicantsInterceptor<T> : InspectionGjiInterceptor<T>
        where T : BaseLicenseApplicants
    {
        /// <summary>
        /// Проставить номер документа, объект провекри и тип юридического лица
        /// </summary>
        /// <returns></returns>
        public override IDataResult BeforeCreateAction(IDomainService<T> service, T entity)
        {
            var maxNum = service.GetAll()
                .Where(x => x.TypeBase == entity.TypeBase)
                .Select(x => x.InspectionNum).Max();

            entity.InspectionNum = maxNum.ToInt() + 1;
            entity.InspectionNumber = entity.InspectionNum.ToStr();
            entity.Contragent = entity.ManOrgLicenseRequest.Contragent;
            entity.PersonInspection = PersonInspection.Organization;
            entity.TypeJurPerson = TypeJurPerson.ManagingOrganization;

            return base.BeforeCreateAction(service, entity);
        }
    }
}