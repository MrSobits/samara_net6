using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bars.B4;
using Bars.Gkh.Entities.Dicts;

namespace Bars.Gkh.Overhaul.Interceptors
{
    class WorkInterceptor : EmptyDomainInterceptor<Work>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Work> service, Work entity)
        {
            return ValidateEntity(entity);
        }

        public override IDataResult BeforeUpdateAction(IDomainService<Work> service, Work entity)
        {
            return ValidateEntity(entity);
        }

        private IDataResult ValidateEntity(Work entity)
        {
            if (entity.Normative < 0)
            {
                return Failure("Значение поля Норматив должно быть неотрицательным");
            }

            return Success();
        }
    }
}
