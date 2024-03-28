using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.Gkh.Regions.Voronezh.Schema;

namespace Bars.Gkh.Regions.Voronezh.Entities
{
    public class RosRegExtract : PersistentObject
    {
        //Description
        public virtual RosRegExtractDesc desc_id { get; set; }
        //Registration
        public virtual RosRegExtractRight right_id { get; set; }

    }
}
