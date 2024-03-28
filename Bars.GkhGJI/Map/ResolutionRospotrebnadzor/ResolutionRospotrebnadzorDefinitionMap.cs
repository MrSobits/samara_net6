namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>Маппинг для "Определение постановления Роспотребнадзора"</summary>
    public class ResolutionRospotrebnadzorDefinitionMap : BaseEntityMap<ResolutionRospotrebnadzorDefinition>
    {
        #region Названия полей
        /// <summary>
        /// Имя таблицы
        /// </summary>
        public const string TableName = "GJI_RESOLUTION_ROSPOTREBNADZOR_DEFINITION";

        /// <summary>
        /// Номер документа
        /// </summary>
        public const string DocumentNum = "DOCUMENT_NUM";

        /// <summary>
        /// Дата документа
        /// </summary>
        public const string DocumentDate = "DOCUMENT_DATE";

        /// <summary>
        /// тип определения
        /// </summary>
        public const string TypeDefinition = "TYPE_DEFINITION";

        /// <summary>
        /// Дата документа
        /// </summary>
        public const string ExecutionDate = "EXECUTION_DATE";

        /// <summary>
        /// Описание
        /// </summary>
        public const string Description = "DESCRIPTION";

        /// <summary>
        /// ДЛ, вынесшее определение
        /// </summary>
        public const string IssuedDefinition = "ISSUED_DEFINITION_ID";

        /// <summary>
        /// Постановление Роспотребнадзора
        /// </summary>
        public const string Resolution = "RESOLUTION_ID";
        #endregion
        public ResolutionRospotrebnadzorDefinitionMap() :
                base("Определение постановления Роспотребнадзора", ResolutionRospotrebnadzorDefinitionMap.TableName)
        {
        }

        protected override void Map()
        {
            this.Property(x => x.DocumentNum, "Номер документа").Column(ResolutionRospotrebnadzorDefinitionMap.DocumentNum).Length(50);
            this.Property(x => x.DocumentDate, "Дата документа").Column(ResolutionRospotrebnadzorDefinitionMap.DocumentDate);
            this.Property(x => x.TypeDefinition, "Тип определения")
                .Column(ResolutionRospotrebnadzorDefinitionMap.TypeDefinition)
                .NotNull()
                .DefaultValue(TypeDefinitionResolution.Deferment);
            this.Property(x => x.ExecutionDate, "Дата исполнения").Column(ResolutionRospotrebnadzorDefinitionMap.ExecutionDate);
            this.Property(x => x.Description, "Описание").Column(ResolutionRospotrebnadzorDefinitionMap.Description);
            this.Reference(x => x.IssuedDefinition, "ДЛ, вынесшее определение").Column(ResolutionRospotrebnadzorDefinitionMap.IssuedDefinition).Fetch();
            this.Reference(x => x.Resolution, "Постановление").Column(ResolutionRospotrebnadzorDefinitionMap.Resolution).NotNull().Fetch();
        }
    }
}
