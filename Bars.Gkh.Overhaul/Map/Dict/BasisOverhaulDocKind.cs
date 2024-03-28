namespace Bars.Gkh.Overhaul.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Entities.Dict;

    public class BasisOverhaulDocKindMap : BaseGkhDictMap<BasisOverhaulDocKind>
    {
        public BasisOverhaulDocKindMap()
            : base("Вид документа основания ДПКР", "OVRHL_DICT_BASIS_OVERHAUL_DOC_KIND")
        {
        }
    }
}
