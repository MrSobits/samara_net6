namespace Bars.GkhGji.Regions.Stavropol.Interceptors
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Stavropol.NumberRule;

    public class ProtocolDefinitionInterceptor : EmptyDomainInterceptor<ProtocolDefinition>
    {
        public IDefinitionNumberRule DefinitionNumberRule { get; set; }
        public override IDataResult BeforeCreateAction(IDomainService<ProtocolDefinition> service, ProtocolDefinition entity)
        {
            if (!entity.DocumentDate.HasValue)
            {
                return Failure("Укажите дату определения");
            }

            if (entity.DocumentNumber.HasValue)
            {
                var checkNumber =
                    DefinitionNumberRule.CheckNumber(entity.DocumentDate.Value, entity.DocumentNumber.Value, entity.Id, typeof(ProtocolDefinition));

                if (checkNumber.IsExists)
                {
                    return Failure(string.Format(
                        "Определение с таким номером уже создано в документе \"{0}\" от {1}",
                        checkNumber.DocumentOfDefinition.GetEnumMeta().Display,
                        checkNumber.DefinitionDate.ToShortDateString()));
                }
            }

            entity.DocumentNumber = DefinitionNumberRule.SetNumber(entity.DocumentDate.Value);
            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<ProtocolDefinition> service, ProtocolDefinition entity)
        {
            if (!entity.DocumentDate.HasValue)
            {
                return Failure("Укажите дату определения");
            }

            if (entity.DocumentNumber.HasValue)
            {
                var checkNumber =
                    DefinitionNumberRule.CheckNumber(entity.DocumentDate.Value, entity.DocumentNumber.Value, entity.Id, typeof(ProtocolDefinition));

                if (checkNumber.IsExists)
                {
                    return Failure(string.Format(
                        "Определение с таким номером уже создано в документе \"{0}\" от {1}",
                        checkNumber.DocumentOfDefinition.GetEnumMeta().Display,
                        checkNumber.DefinitionDate.ToShortDateString()));
                }
            }

            return Success();
        }
    }
}