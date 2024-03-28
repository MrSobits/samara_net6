using Bars.B4;
using Bars.Gkh.Authentification;
using Bars.Gkh.Entities;
using Sobits.GisGkh.Entities;
using System;
using System.Linq;

namespace Sobits.GisGkh.Interceptors
{
    class NsiListInterceptor : EmptyDomainInterceptor<NsiList>
    {
        public override IDataResult BeforeCreateAction(IDomainService<NsiList> service, NsiList entity)
        {
            try
            {
                entity.ObjectCreateDate = DateTime.Now;
                entity.ObjectEditDate = DateTime.Now;
                entity.ObjectVersion = 1;
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<NsiList>: {e.Message}");
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<NsiList> service, NsiList entity)
        {
            try
            {
                entity.ObjectEditDate = DateTime.Now;

                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeUpdateAction<NsiList>: {e.Message}");
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<NsiList> service, NsiList entity)
        {
            try
            {
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<NsiList>: {e.ToString()}");
            }
        }
    }
}
