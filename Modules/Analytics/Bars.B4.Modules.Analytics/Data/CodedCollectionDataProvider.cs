namespace Bars.B4.Modules.Analytics.Data
{
    using System.Linq;
    using Castle.Windsor;

    public class CodedCollectionDataProvider<T> : BaseCollectionDataProvider<T> where T : class, new()
    {
        private readonly string _name;
        private readonly string _desc;
        private readonly IQueryable<T> _data;


        public CodedCollectionDataProvider(string name, string description,
            IQueryable<T> data, IWindsorContainer container)
            : base(container)
        {
            _name = name;
            _desc = description;
            _data = data;

        }

        protected override IQueryable<T> GetDataInternal(BaseParams baseParams)
        {
            return _data;
        }

        public override string Name
        {
            get { return _name; }
        }

        public override string Description
        {
            get { return _desc; }
        }
    }
}
