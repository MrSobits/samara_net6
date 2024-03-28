namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using Enums;
    using Bars.GkhGji.Enums;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;
    using B4.Modules.States;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.B4.DataAccess;

    class AppealCitsPrescriptionFondInterceptor : EmptyDomainInterceptor<AppealCitsPrescriptionFond>
    {

        public override IDataResult BeforeCreateAction(IDomainService<AppealCitsPrescriptionFond> service, AppealCitsPrescriptionFond entity)
        {
            try
            {
                var prevPrescription = service.GetAll()
                    .Where(x => x.DocumentNumber != null && x.DocumentNumber != "")
                    .OrderByDescending(x => x.Id).FirstOrDefault();
                if (prevPrescription != null)
                {
                    int number = 1;
                    if (int.TryParse(prevPrescription.DocumentNumber, out number))
                    {
                        int newNumber = number + 1;
                        entity.DocumentNumber = newNumber.ToString();
                    }
                    else
                    {
                        entity.DocumentNumber = "ошибка";
                    }
                }
                else
                {
                    entity.DocumentNumber = "1";
                }




                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось сохранить задачу");
            }
        }

        

    }
}
