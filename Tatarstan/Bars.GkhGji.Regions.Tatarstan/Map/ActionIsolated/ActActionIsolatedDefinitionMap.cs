namespace Bars.GkhGji.Regions.Tatarstan.Map.ActionIsolated
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class ActActionIsolatedDefinitionMap : BaseEntityMap<ActActionIsolatedDefinition>
    {
        /// <inheritdoc />
        public ActActionIsolatedDefinitionMap()
            : base(nameof(ActActionIsolatedDefinition),"GJI_ACT_ACTIONISOLATED_DEFINITION")
        {
        }
        

        /// <inheritdoc />
        protected override void Map()
        {
            Property(x => x.Number, "Номер").Column("NUMBER");
            Property(x => x.Date, "Дата документа").Column("DATE");
            Property(x => x.ExecutionDate, "Дата исполнения").Column("EXECUTION_DATE");
            Property(x => x.Description, "Описание").Column("DESCRIPTION");
            Property(x => x.DefinitionType, "Тип определения").Column("DEFINITION_TYPE");
            Reference(x => x.Official, "ДЛ, вынесшее определение").Column("OFFICIAL_ID");
            Reference(x => x.RealityObject, "Дом").Column("REALITY_OBJECT_ID");
            Reference(x => x.Act, "Акт КНМ без взаимодействия с контролируемыми лицами").Column("ACT_ID");
        }
    }
}