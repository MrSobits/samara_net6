using System;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Zabaykalye.DomainService;

namespace Bars.GkhGji.Regions.Zabaykalye.Interceptors
{
    using Bars.B4;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;

    public class ProtocolDefInterceptor : ProtocolDefinitionInterceptor<ZabaykalyeProtocolDefinition>
    {
        public IDefinitionService DefinitionService { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<ZabaykalyeProtocolDefinition> service, ZabaykalyeProtocolDefinition entity)
        {
            if (string.IsNullOrEmpty(entity.DocumentNum) && !entity.DocumentNumber.HasValue)
            {
                // при изменении генерируем новый номер только в случае если оба поля пустые 
                return CreateNumber(entity);
            }
            else if (entity.DocumentNumber.HasValue && entity.DocumentNum != entity.DocumentNumber.ToString())
            {
                // если номер целочисленный непустой, то значит пользователь решил изменить номер вручную 
                entity.DocumentNum = entity.DocumentNumber.Value.ToString();
            }

            return Success();
        }

        public override IDataResult BeforeCreateAction(IDomainService<ZabaykalyeProtocolDefinition> service, ZabaykalyeProtocolDefinition entity)
        {
            return CreateNumber(entity);
        }

        private IDataResult CreateNumber(ZabaykalyeProtocolDefinition entity)
        {
            if (!entity.DocumentDate.HasValue)
            {
                return Failure("Необходимо указать дату определения");
            }

            var maxNum = DefinitionService.GetMaxDefinitionNum(entity.DocumentDate.Value.Year);

            entity.DocumentNumber = maxNum + 1;
            entity.DocumentNum = entity.DocumentNumber.Value.ToString();

            return Success();
        }
    }
}
