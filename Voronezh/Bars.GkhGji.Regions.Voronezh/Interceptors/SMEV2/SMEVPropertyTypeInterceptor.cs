namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;
    using Entities;
    using Enums;
    using System;
    using System.Linq;

    class SMEVPropertyTypeInterceptor : EmptyDomainInterceptor<SMEVPropertyType>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<SMEVPropertyTypeFile> SMEVPropertyTypeFileDomain { get; set; }
        public IDomainService<Inspector> InspectorDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SMEVPropertyType> service, SMEVPropertyType entity)
        {
            entity.CalcDate = DateTime.Now;
            try
            {
//#if DEBUG
               entity.Inspector = InspectorDomain.GetAll().First();
//#else
                //Operator thisOperator = UserManager.GetActiveOperator();
                //    if (thisOperator?.Inspector == null)
                //        return Failure("Обмен информацией с ГИС ЕРП доступен только сотрудникам ГЖИ");

                //    entity.Inspector = thisOperator.Inspector;
//#endif
                //
                entity.RequestState = RequestState.NotFormed;
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<SMEVPropertyTypeFileDomain>: {e.Message}");
               

            }
        }     

        public override IDataResult BeforeDeleteAction(IDomainService<SMEVPropertyType> service, SMEVPropertyType entity)
        {
            try
            {
                //чистка приаттаченных файлов
                SMEVPropertyTypeFileDomain.GetAll()
               .Where(x => x.SMEVPropertyType.Id == entity.Id)
               .Select(x => x.Id)
               .ToList()
               .ForEach(x => SMEVPropertyTypeFileDomain.Delete(x));

             
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeDeleteAction<SMEVPropertyTypeFileDomain>: {e.ToString()}");
            }
        }
 
       
    }
}
