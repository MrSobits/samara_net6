namespace Bars.B4.Modules.FIAS
{
    using System;
    using System.Linq;

    using Bars.B4;

    using Castle.Windsor;

    public class FiasServiceInterceptor : EmptyDomainInterceptor<Fias>
    {
        public override IDataResult BeforeCreateAction(IDomainService<Fias> service, Fias entity)
        {
            if (entity.TypeRecord == FiasTypeRecordEnum.User)
            {
                // При создании новой записи в ФИАС генерируем новый гуид Id
                entity.AOId = Guid.NewGuid().ToString();
                entity.AOGuid = Guid.NewGuid().ToString();

                // Проверяем заполненность кода
                var errorText = DomainFias.ValidationCode(entity);

                if (!string.IsNullOrEmpty(errorText))
                {
                    return BaseDataResult.Error("Необходимо заполнить:" + errorText);
                }

                // Формируем код
                entity.CodeRecord = DomainFias.GetCode(entity);
            }

            return new BaseDataResult();
        }
        
        public override IDataResult BeforeUpdateAction(IDomainService<Fias> service, Fias entity)
        {
            if (entity.TypeRecord == FiasTypeRecordEnum.User)
            {
                // Проверяем заполненность кода
                var errorText = DomainFias.ValidationCode(entity);

                if (!string.IsNullOrEmpty(errorText))
                {
                    return BaseDataResult.Error("Необходимо заполнить:" + errorText);
                }

                // Формируем код
                entity.CodeRecord = DomainFias.GetCode(entity);
            }

            return new BaseDataResult();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<Fias> service, Fias entity)
        {
            /*
            * Перед удалением необходимо проверить :
            * 1. Если удаляемая запись загружена из ФИАС то удалять нельзя
            * 2. Если есть в таблице записи ссылающиееся на данную запись по ParentGuid то удалять нельзя
            */

            if (entity.TypeRecord == FiasTypeRecordEnum.Fias)
            {
                return BaseDataResult.Error("Записи загруженные из ФИАС удалить нельзя");
            }

            // Удалить документ можно только если несуществует документа с типом Паспорт готовности
            if (service.GetAll().Count(x => x.ParentGuid == entity.AOGuid) > 0)
            {
                return BaseDataResult.Error("Данная запись имеет дочерние элементы, удаление невозможно");
            }

            return new BaseDataResult();
        }
    }
}
