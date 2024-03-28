namespace Bars.Gkh.RegOperator.Imports.FsGorod
{
    using System.Linq;
    using Bars.Gkh.RegOperator.Entities;

    public class FsGorodImportInfoProxy
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int DataHeadIndex { get; set; }

        public string Delimiter { get; set; }

        public FsGorodMapItemProxy[] MapItems { get; set; }

        public FsGorodImportInfo AsEntity()
        {
            return new FsGorodImportInfo
            {
                Code = Code,
                DataHeadIndex = DataHeadIndex,
                Delimiter = Delimiter,
                Description = Description,
                Name = Name
            };
        }

        public static FsGorodImportInfoProxy FromEntity(FsGorodImportInfo entity)
        {
            return new FsGorodImportInfoProxy
            {
                Code = entity.Code,
                DataHeadIndex = entity.DataHeadIndex,
                Delimiter = entity.Delimiter,
                Description = entity.Description,
                Name = entity.Name,
                MapItems = entity.MapItems.Select(FsGorodMapItemProxy.FromEntity).ToArray()
            };
        }
    }
}
