using Bars.B4.Modules.Mapping.Mappers;
using Bars.GkhGji.Entities.Dict;

namespace Bars.GkhGji.Map.Dict
{
    public class QuestionKindMap : BaseEntityMap<QuestionKind>
    {
        public QuestionKindMap()
            : base("Виды вопросов", "GJI_DICT_QUESTION_KIND")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.QuestionType, "Тип вопроса").Column("TYPE");
            this.Property(x => x.Code, "Код записи").Column("CODE").Length(3).NotNull();
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(100).NotNull();
        }
    }
}
