using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Service;
using System;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Sobits.RosReg.Tasks.ExtractParse
{
    using System.Collections.Generic;
    using System.Xml;
    using Sobits.RosReg.Entities;
    public class ParseOldExtractsPaspTaskExecutor : ITaskExecutor
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public string ExecutorCode { get; private set; }
        public IDomainService<ExtractEgrnRightInd> ExtractEgrnRightIndDomain { get; set; }

        public IDataResult Execute(
            BaseParams @params,
            Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            try
            {
                List<long> rightinds = @params.Params.GetAs<List<long>>("inds");
                
                
                var counter = 0;
                
                foreach (var ind in rightinds)
                {
                    counter++;
                    SetExtractData(ind);
                }

                indicator.Report(null, (uint) (counter / rightinds.Count * 100), "Заполнение данных");

                return new BaseDataResult(true, $@"выполнено {counter}");
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }

        private void SetExtractData(long indId)
        {
            
            var ind = ExtractEgrnRightIndDomain.Get(indId);
                
            var xDoc = new XmlDocument();
            if (!string.IsNullOrEmpty(ind?.RightId?.EgrnId?.ExtractId?.Xml))
            {
                xDoc.LoadXml(ind.RightId.EgrnId.ExtractId.Xml);
                var xRoot = xDoc.DocumentElement;

                var rightNodes = xRoot.SelectNodes("//*/right_record");
                for (var indexRight = 0; indexRight < rightNodes.Count; indexRight++)
                {
                    var rightNumber = rightNodes[indexRight].SelectSingleNode("right_data/right_number")?.InnerText;

                    if (rightNumber == ind.RightId.Number)
                    {
                        
                        
                        var ownerNodes = rightNodes[indexRight].SelectNodes("right_holders/right_holder/individual");
                        for (var indexOwner = 0; indexOwner < ownerNodes.Count; indexOwner++)
                        {
                            // Собственники - физ.лица

                            var surname = ownerNodes[indexOwner].SelectSingleNode("surname")?.InnerText;
                            var firstName = ownerNodes[indexOwner].SelectSingleNode("name")?.InnerText;
                            var patronymic = ownerNodes[indexOwner].SelectSingleNode("patronymic")?.InnerText;
                            
                            if (ind.Surname == surname && ind.FirstName == firstName && ind.Patronymic == patronymic)
                            {
                                ind.DocIndCode = ownerNodes[indexOwner].SelectSingleNode("identity_doc/document_code/code")?.InnerText;
                                ind.DocIndName = ownerNodes[indexOwner].SelectSingleNode("identity_doc/document_code/value")?.InnerText;
                                ind.DocIndSerial = ownerNodes[indexOwner].SelectSingleNode("identity_doc/document_series")?.InnerText;
                                ind.DocIndNumber = ownerNodes[indexOwner].SelectSingleNode("identity_doc/document_number")?.InnerText;
                                var dt = new DateTime();
                                DateTime.TryParse(ownerNodes[indexOwner].SelectSingleNode("identity_doc/document_date")?.InnerText, out dt);
                                ind.DocIndDate = dt;
                                ind.DocIndIssue = ownerNodes[indexOwner].SelectSingleNode("identity_doc/document_issuer")?.InnerText;
                                this.ExtractEgrnRightIndDomain.Update(ind);
                            }
                        }
                    }
                }
            }
                
            
            
        }
    }
}
