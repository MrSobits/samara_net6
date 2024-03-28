namespace Bars.Gkh.DomainService
{
    using B4;

    public interface IManagingOrganizationService
    {
        /// <summary>
        /// Получить управляющую организацию по id контрагента
        /// </summary>
        /// <param name="baseParams"> baseParams </param>
        /// <returns> Управляющая организация </returns>
        IDataResult GetManOrgByContagentId(BaseParams baseParams);
    }
}
