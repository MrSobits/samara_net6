namespace Bars.GkhGji.Regions.Khakasia.Interceptors
{
	using System;
	using Bars.B4.Utils;
	using Bars.GkhGji.Entities;
	using Bars.GkhGji.Regions.Khakasia.DomainService;

	using Bars.B4;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Khakasia.Entities;

    public class ProtocolDefInterceptor : ProtocolDefinitionInterceptor<KhakasiaProtocolDefinition>
    {
        public IDefinitionService DefinitionService { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<KhakasiaProtocolDefinition> service, KhakasiaProtocolDefinition entity)
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

        public override IDataResult BeforeCreateAction(IDomainService<KhakasiaProtocolDefinition> service, KhakasiaProtocolDefinition entity)
        {
            return CreateNumber(entity);
        }

        private IDataResult CreateNumber(KhakasiaProtocolDefinition entity)
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
