namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System;
    using System.Linq;
    using B4.Utils;
    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Enums;

    public class AdministrativeCaseDocInterceptor : EmptyDomainInterceptor<AdministrativeCaseDoc>
    {
        public override IDataResult BeforeCreateAction(IDomainService<AdministrativeCaseDoc> service, AdministrativeCaseDoc entity)
        {
            var administrativeCaseDocs = service.GetAll()
                .Where(x => x.AdministrativeCase.Id == entity.AdministrativeCase.Id)
                .Where(x => x.TypeAdminCaseDoc == TypeAdminCaseDoc.PetitionProlongation)
                .Select(x => x.Id)
                .ToList();

            if (administrativeCaseDocs.Count >= 1)
            {
                if (entity.TypeAdminCaseDoc == TypeAdminCaseDoc.PetitionProlongation)
                {
                    return Failure("Вы не можете создавать более одного ходатайства о продлении срока в рамках одного административного дела");
                }
            }
            
            SetNumber(service, entity);

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<AdministrativeCaseDoc> service, AdministrativeCaseDoc entity)
        {
            SetNumber(service, entity);

            return Success();
        }

        private void SetNumber(IDomainService<AdministrativeCaseDoc> service, AdministrativeCaseDoc entity)
        {
            if (entity.DocumentNumber.IsEmpty())
            {
                var number = service.GetAll()
                    .Where(x => x.AdministrativeCase.Id == entity.AdministrativeCase.Id)
                    .Where(x => x.TypeAdminCaseDoc == entity.TypeAdminCaseDoc)
                    .Where(x => x.Id != entity.Id)
                    .Max(x => x.DocumentNum) ?? 0;

                entity.DocumentNum = number + 1;

                entity.DocumentNumber =
                    !entity.AdministrativeCase.DocumentNumber.IsEmpty()
                        ? string.Format("{0}/{1}", entity.AdministrativeCase.DocumentNumber, entity.DocumentNum)
                        : null;
            }
            else
            {
                entity.DocumentNum = null;
                entity.DocumentNumber = null;
            }
        }

        //public override IDataResult BeforeUpdateAction(IDomainService<ZonalInspection> service, ZonalInspection entity)
        //{
        //    var zonalInspList = service.GetAll()
        //        .Where(x => x.Id != entity.Id)
        //        .Select(x => x.DepartmentCode)
        //        .ToList();

        //    var zonalInspDepartmentCode = entity.DepartmentCode;

        //    if (zonalInspList.Contains(zonalInspDepartmentCode))
        //    {
        //        return Failure("Ошибка. Номер отдела с таким значением уже заведен в систему.");
        //    }
        //    return this.Success();
        //}
    }
}
