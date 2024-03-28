namespace Bars.GkhDi.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhDi.Attributes;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Windsor;

    public class TemplateServService : ITemplateServService
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получаем имена настраиваемых полей (Только те которые нужно скрывать)
        /// </summary>
        /// <returns></returns>
        public IDataResult GetOptionsFields(BaseParams baseParams)
        {
            try
            {
                var templateServiceId = baseParams.Params.GetAs<long>("templateServiceId");

                var optionFields = this.Container.Resolve<IDomainService<TemplateServiceOptionFields>>()
                        .GetAll()
                        .Where(x => x.TemplateService.Id == templateServiceId)
                        .Select(x => new
                        {
                            x.FieldName,
                            x.IsHidden
                        });

                return new BaseDataResult(optionFields)
                    {
                        Success = true
                    };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        /// <summary>
        /// Метод строящий настраиваемые поля шаблонной услуги
        /// </summary>
        /// <returns></returns>
        public IDataResult ConstructOptionsFields(BaseParams baseParams)
        {
            try
            {
                var kindService = baseParams.Params["kindService"].To<KindServiceDi>();
                var templateServiceId = baseParams.Params.GetAs<long>("templateServiceId");

                var pis = new List<PropertyInfo>();

                // Взависимости от типа услуги получаем поля(настраиваемые) помечанные аттрибутом OptionFieldAttribute
                switch (kindService)
                {
                    case KindServiceDi.Additional:
                        pis = typeof(AdditionalService).GetProperties().Where(x => Attribute.IsDefined(x, typeof(OptionFieldAttribute))).ToList();
                        break;
                    case KindServiceDi.Housing:
                        pis = typeof(HousingService).GetProperties().Where(x => Attribute.IsDefined(x, typeof(OptionFieldAttribute))).ToList();
                        break;
                    case KindServiceDi.Communal:
                        pis = typeof(CommunalService).GetProperties().Where(x => Attribute.IsDefined(x, typeof(OptionFieldAttribute))).ToList();
                        break;
                    case KindServiceDi.Repair:
                        pis = typeof(RepairService).GetProperties().Where(x => Attribute.IsDefined(x, typeof(OptionFieldAttribute))).ToList();
                        break;
                    case KindServiceDi.CapitalRepair:
                        pis = typeof(CapRepairService).GetProperties().Where(x => Attribute.IsDefined(x, typeof(OptionFieldAttribute))).ToList();
                        break;
                    case KindServiceDi.Managing:
                        pis = typeof(ControlService).GetProperties().Where(x => Attribute.IsDefined(x, typeof(OptionFieldAttribute))).ToList();
                        break;
                }

                // Вытаскиваем в коллекцию из полученных полей имя поля и имя поля для пользователя
                var fields = new List<OptionFieldData>();
                foreach (var pi in pis)
                {
                    var attr = (OptionFieldAttribute)pi.GetCustomAttributes(typeof(OptionFieldAttribute), false).FirstOrDefault();
                    if (attr != null)
                    {
                        fields.Add(new OptionFieldData { Name = attr.Name, FieldName = pi.Name });
                    }
                }

                var serviceOptionField = this.Container.Resolve<IDomainService<TemplateServiceOptionFields>>();

                // Получаем уже сохраненные имена полей
                var savedFields = serviceOptionField
                        .GetAll()
                        .Where(x => x.TemplateService.Id == templateServiceId)
                        .ToList();

                // Поля для добавления(помеченные атрибутом спустя какое то время)
                var addFieldList = fields.Where(x => !savedFields.Select(y => y.FieldName).Distinct().Contains(x.FieldName));

                // Поля для удаления(у которых атрибут убран спустя какое то время)
                var deleteFieldList = savedFields.Select(y => y.FieldName).Distinct().Where(x => !fields.Select(y => y.FieldName).Distinct().Contains(x));

                // Удаляем/добавляем поля
                foreach (var addField in addFieldList)
                {
                    serviceOptionField.Save(
                        new TemplateServiceOptionFields
                        {
                            FieldName = addField.FieldName,
                            IsHidden = true,
                            Name = addField.Name,
                            TemplateService = new TemplateService { Id = templateServiceId },
                        });
                }

                foreach (var deleteField in deleteFieldList)
                {
                    var field = deleteField;
                    var ids = savedFields.Where(x => x.FieldName == field).Select(x => x.Id);
                    foreach (var id in ids)
                    {
                        serviceOptionField.Delete(id);
                    }
                }

                return new BaseDataResult { Success = true };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        /// <summary>
        /// Получаем имена настраиваемых полей (Только те которые нужно скрывать)
        /// </summary>
        /// <returns></returns>
        public IDataResult GetUnitMeasure(BaseParams baseParams)
        {
            try
            {
                var templateServiceId = baseParams.Params.GetAs<long>("templateServiceId");

                var unitMeasureInfo = this.Container.Resolve<IDomainService<TemplateService>>()
                        .GetAll()
                        .Where(x => x.Id == templateServiceId)
                        .Select(x => new
                        {
                            x.UnitMeasure,
                            x.Changeable
                        })
                        .FirstOrDefault();

                return new BaseDataResult(unitMeasureInfo)
                {
                    Success = true
                };
            }
            catch (ValidationException exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        protected class OptionFieldData
        {
            /// <summary>
            /// Имя поля которое видит пользователь
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Имя поля
            /// </summary>
            public string FieldName { get; set; }
        }
    }
}
