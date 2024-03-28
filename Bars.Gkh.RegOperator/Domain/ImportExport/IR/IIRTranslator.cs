namespace Bars.Gkh.RegOperator.Domain.ImportExport.IR
{
    using System.Collections.Generic;
    using System.IO;

    public interface IIRTranslator
    {
        IEnumerable<IRModel> Parse(Stream data);

        Stream FromModel(IRModel model, List<IRModelProperty> metaData);
    }
}