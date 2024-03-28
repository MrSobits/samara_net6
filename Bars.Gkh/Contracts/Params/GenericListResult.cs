namespace Bars.Gkh.Contracts.Params
{
    using System.Collections.Generic;

    using Bars.B4;

    public class GenericListResult<TDto> : ListDataResult, IDataResult<IEnumerable<TDto>>
    {
        public GenericListResult(IEnumerable<TDto> data, int totalCount)
        {
            this.Data = data;
            this.TotalCount = totalCount;
            this.Success = true;
        }

        /// <inheritdoc />
        public new IEnumerable<TDto> Data
        {
            get { return (IEnumerable<TDto>)base.Data; }
            set { base.Data = value; }
        }
    }
}