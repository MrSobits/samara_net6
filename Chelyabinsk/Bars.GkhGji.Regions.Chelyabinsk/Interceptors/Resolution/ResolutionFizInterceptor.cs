namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;
    using Enums;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;

    class ResolutionFizInterceptor : EmptyDomainInterceptor<ResolutionFiz>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ResolutionFiz> service, ResolutionFiz entity)
        {
            try {   
              
                  entity.PayerCode = CreateAltPayerIdentifier(entity);             
                  return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось создать запрос");
            }
            
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ResolutionFiz> service, ResolutionFiz entity)
        {
            try
            {          
               entity.PayerCode = CreateAltPayerIdentifier(entity);
               return Success();
               
            }
            catch (Exception e)
            {
                return Failure("Не удалось сохранить запрос");
            }
        }

        public override IDataResult AfterCreateAction(IDomainService<ResolutionFiz> service, ResolutionFiz entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ResolutionFiz, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.Id.ToString() + " " + entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ResolutionFiz> service, ResolutionFiz entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.ResolutionFiz, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<ResolutionFiz> service, ResolutionFiz entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.ResolutionFiz, entity.Id, entity.GetType(), GetPropertyValues(), entity.Resolution.DocumentNumber + " " + entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "Resolution", "Постановление" },
                { "FLDocType", "Тип документа физ лица" },
                { "PhysicalPersonDocType", "Тип документа физ лица" },
                { "DocumentNumber", "Номер документа физлица" },
                { "PayerCode", "Код плательщика" },
                { "DocumentSerial", "Серия документа физлица" }
            };
            return result;
        }

        private String CreateAltPayerIdentifier(ResolutionFiz entity)
        {
            try
            {
                string documentCode = entity.PhysicalPersonDocType.Code.PadLeft(2, '0');
                string documentNumber = entity.DocumentNumber.ToUpper();
                string documentSerial = entity.DocumentSerial.ToUpper();
                string document = (documentSerial + documentNumber).PadLeft(20, '0');
                return documentCode + document + (entity.IsRF ? "643" : "999");
            }
            catch (Exception e)
            {
                return "";
            }
        }
    }
}
