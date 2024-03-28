using System;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Tula.DomainService;

namespace Bars.GkhGji.Regions.Tula.Interceptors
{
    using Bars.B4;

    public class ActCheckDefInterceptor : EmptyDomainInterceptor<ActCheckDefinition>
    {
        public IDefinitionService DefinitionService { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<ActCheckDefinition> service, ActCheckDefinition entity)
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

        public override IDataResult BeforeCreateAction(IDomainService<ActCheckDefinition> service, ActCheckDefinition entity)
        {
            return CreateNumber(entity);
        }

        private IDataResult CreateNumber(ActCheckDefinition entity)
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
