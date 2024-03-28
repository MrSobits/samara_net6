namespace Bars.Gkh.Regions.Nso.Interceptors.dict
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;

    public class ZonalInspectionInterceptor : EmptyDomainInterceptor<ZonalInspection>
    {
        public override IDataResult BeforeUpdateAction(IDomainService<ZonalInspection> service, ZonalInspection entity)
        {
#warning Поменять на один запрос.
            var zonalInspList = service.GetAll()
                .Where(x => x.Id != entity.Id)
                .Select(x => x.DepartmentCode)
                .ToList();

            var zonalInspDepartmentCode = entity.DepartmentCode;

            if (zonalInspList.Contains(zonalInspDepartmentCode))
            {
                return Failure("Ошибка. Номер отдела с таким значением уже заведен в систему.");
            }
            return this.Success();
        }
    }
}
