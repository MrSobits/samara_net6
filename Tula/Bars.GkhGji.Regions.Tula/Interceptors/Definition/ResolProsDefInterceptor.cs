using System;
using Bars.B4.Utils;
using Bars.GkhGji.Regions.Tula.DomainService;

namespace Bars.GkhGji.Regions.Tula.Interceptors
{
    using Bars.B4;
    using Bars.GkhGji.Regions.Tula.Entities;

    public class ResolProsDefInterceptor : EmptyDomainInterceptor<ResolProsDefinition>
    {
        public IDefinitionService DefinitionService { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<ResolProsDefinition> service, ResolProsDefinition entity)
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

        public override IDataResult BeforeCreateAction(IDomainService<ResolProsDefinition> service, ResolProsDefinition entity)
        {
            return CreateNumber(entity);
        }

        private IDataResult CreateNumber(ResolProsDefinition entity)
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
