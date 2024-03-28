/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map.CommonEstateObject
/// {
///     using System.Collections.Generic;
///     using B4.DataAccess.ByCode;
///     using B4.DataAccess.UserTypes;
///     using Entities.CommonEstateObject;
/// 
///     public class StructuralElementGroupMap : BaseImportableEntityMap<StructuralElementGroup>
///     {
///         public StructuralElementGroupMap() : base("OVRHL_STRUCT_EL_GROUP")
///         {
///             References(x => x.CommonEstateObject, "CMN_ESTATE_OBJ_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.Name, "NAME", true);
///             Map(x => x.Formula, "FORMULA", true);
///             Map(x => x.FormulaName, "FORMULA_NAME", true);
///             Map(x => x.FormulaDescription, "FORMULA_DESC");
///             Map(x => x.Required, "REQUIRED", true);
///             Map(x => x.UseInCalc, "USE_IN_CALC", true, true);
/// 
///             Property(
///                 x => x.FormulaParams,
///                 mapper =>
///                 {
///                     mapper.Column("FORMULA_PARAMS_BIN");
///                     mapper.Type<BinaryJsonType<List<FormulaParams>>>();
///                 });
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map.CommonEstateObject
{
    using Bars.Gkh.Entities.CommonEstateObject;
    using System.Collections.Generic;
    
    using Bars.Gkh.DataAccess;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Группа конструктивных элементов"</summary>
    public class StructuralElementGroupMap : BaseImportableEntityMap<StructuralElementGroup>
    {
        
        public StructuralElementGroupMap() : 
                base("Группа конструктивных элементов", "OVRHL_STRUCT_EL_GROUP")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.CommonEstateObject, "Объект общего имущества").Column("CMN_ESTATE_OBJ_ID").NotNull().Fetch();
            Property(x => x.Name, "Наименование").Column("NAME").Length(250).NotNull();
            Property(x => x.Formula, "Текст формула").Column("FORMULA").Length(250).NotNull();
            Property(x => x.FormulaName, "Название формулы").Column("FORMULA_NAME").Length(250).NotNull();
            Property(x => x.FormulaDescription, "Описание формулы").Column("FORMULA_DESC").Length(250);
            Property(x => x.UseInCalc, "Используется в расчете").Column("USE_IN_CALC").DefaultValue(true).NotNull();
            Property(x => x.FormulaParams, "Параметры формулы").Column("FORMULA_PARAMS_BIN");
            Property(x => x.Required, "Обязательность группы конструктивных элементов").Column("REQUIRED").NotNull();
        }
    }

    public class StructuralElementGroupNHibernateMapping : ClassMapping<StructuralElementGroup>
    {
        public StructuralElementGroupNHibernateMapping()
        {
            Property(
                x => x.FormulaParams,
                mapper =>
                {
                    mapper.Type<ImprovedBinaryJsonType<List<FormulaParams>>>();
                });
        }
    }
}
