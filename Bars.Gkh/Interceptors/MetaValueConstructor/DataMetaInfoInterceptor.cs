namespace Bars.Gkh.Interceptors.MetaValueConstructor
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.EfficiencyRating;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.MetaValueConstructor.Enums;
    using Bars.Gkh.MetaValueConstructor.FormulaValidating;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Интерцептор для <see cref="DataMetaInfo"/>
    /// </summary>
    public class DataMetaInfoInterceptor : EmptyDomainInterceptor<DataMetaInfo>
    {
        /// <summary>
        /// Домен-сервис <see cref="BaseDataValue"/>
        /// </summary>
        public IDomainService<BaseDataValue> BaseDataValueDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ManagingOrganizationDataValue"/>
        /// </summary>
        public IDomainService<ManagingOrganizationDataValue> ManagingOrganizationDataValueDomain { get; set; }

        /// <summary>Метод вызывается перед удалением объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeDeleteAction(IDomainService<DataMetaInfo> service, DataMetaInfo entity)
        {
            // удаляем все реализованные значения данного описания
            this.BaseDataValueDomain.GetAll().Where(x => x.MetaInfo.Id == entity.Id).Select(x => x.Id).ForEach(x => this.BaseDataValueDomain.Delete(x));

            // удаляем все потомки данного описания
            service.GetAll().Where(x => x.Parent.Id == entity.Id).Select(x => x.Id).ForEach(x => service.Delete(x));

            return base.BeforeDeleteAction(service, entity);
        }

        /// <summary>Метод вызывается перед созданием объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeCreateAction(IDomainService<DataMetaInfo> service, DataMetaInfo entity)
        {
            var validateResult = this.ValidateEntity(service, entity, ServiceOperationType.Save);
            if (!validateResult.Success)
            {
                return validateResult;
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <summary>Метод вызывается перед обновлением объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<DataMetaInfo> service, DataMetaInfo entity)
        {
            var validateResult = this.ValidateEntity(service, entity, ServiceOperationType.Update);
            if (!validateResult.Success)
            {
                return validateResult;
            }

            return base.BeforeUpdateAction(service, entity);
        }

        /// <summary>Метод вызывается после обновления объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult AfterUpdateAction(IDomainService<DataMetaInfo> service, DataMetaInfo entity)
        {
            if (entity.Group.ConstructorType == DataMetaObjectType.EfficientcyRating && entity.Level == 1)
            {
                // пересохраняем фактор, чтобы в нем записалась формула
                service.Update(entity.Parent);
            }

            return this.Success();
        }

        /// <summary>Метод вызывается после создания объекта</summary>
        /// <param name="service">Домен</param>
        /// <param name="entity">Объект</param>
        /// <returns>Результат выполнения</returns>
        public override IDataResult AfterCreateAction(IDomainService<DataMetaInfo> service, DataMetaInfo entity)
        {
            if (entity.Group.ConstructorType == DataMetaObjectType.EfficientcyRating && entity.Level == 1)
            {
                // пересохраняем фактор, чтобы в нем записалась формула
                service.Update(entity.Parent);
            }

            return this.Success();
        }

        private IDataResult ValidateEntity(IDomainService<DataMetaInfo> service, DataMetaInfo entity, ServiceOperationType operationType)
        {
            var fieldList = new List<string>();

            if (entity.Name.IsEmpty())
            {
                fieldList.Add("Наименование");
            }

            if (entity.Code.IsEmpty())
            {
                fieldList.Add("Код");
            }

            if (fieldList.Any())
            {
                return this.Failure($"Не заполнены обязательные поля: {fieldList.AggregateWithSeparator(", ")}");
            }

            if (!this.ValidateCode(entity.Group.ConstructorType, entity.Code))
            {
                return this.Failure("Код указан неверно");
            }

            var existsEntity = service.GetAll()
                .Where(x => x.Id != entity.Id && entity.Parent == x.Parent && x.Group.Id == entity.Group.Id) // элементы, которые принадлежать той же ноде
                .FirstOrDefault(x => x.Name == entity.Name || x.Code == entity.Code);

            if (existsEntity != null)
            {
                var displayName = entity.Group.ConstructorType == DataMetaObjectType.EfficientcyRating ? this.GetDisplayName(entity.Level) : "Элемент";

                if (entity.Name == existsEntity.Name)
                {
                    return this.Failure($"{displayName} с таким значением уже существует");
                }

                if (entity.Code == existsEntity.Code)
                {
                    return this.Failure("Указанный код уже существует");
                }
            }

            return this.ValidateFormula(entity);
        }

        private string GetDisplayName(int level)
        {
            switch (level)
            {
                case 0:
                    return "Фактор";

                case 1:
                    return "Коэффициент";

                case 2:
                    return "Атрибут";

                default: throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        private bool ValidateCode(DataMetaObjectType type, string code)
        {
            switch (type)
            {
                case DataMetaObjectType.EfficientcyRating:
                    return Regex.IsMatch(code, "^[A-ZА-Я]{1,3}[a-zа-я0-9]{0,4}$", RegexOptions.Compiled);

                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private IDataResult ValidateFormula(DataMetaInfo entity)
        {
            var formulaValidators = this.Container.ResolveAll<IFormulaValiudator>();

            using (this.Container.Using(formulaValidators))
            {
                foreach (var validator in formulaValidators.Where(x => x.CanValidate(entity)))
                {
                    var validateResult = validator.Validate(entity);
                    if (!validateResult.Success)
                    {
                        return validateResult;
                    }
                }
            }

            return this.Success();
        }
    }
}