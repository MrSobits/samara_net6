namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;
    using B4.DataAccess;
    using System;

    public abstract class BaseDocNumberValidationChelyabinskRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocumentGjiChildrenDomain { get; set; }

        public IDomainService<Inspector> InspectorDomain { get; set; }

        public IDomainService<ZonalInspectionInspector> ZonalInspectionInspectorDomain { get; set; }

        public IDomainService<DocumentCode> DocumentCodeDomain { get; set; }

        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }

        public IDomainService<Disposal> DisposalDomain { get; set; }

        public abstract string Id { get; }

        public abstract string Name { get; }

        public abstract string TypeId { get; }

        public abstract string Description { get; }        

        public virtual ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
          

            var result = new ValidateResult();

            var document = statefulEntity as DocumentGji;

            if (document != null)
            {
                if (!document.DocumentDate.HasValue)
                {
                    result.Message = "Невозможно сформировать номер, поскольку дата документа не указана";
                    result.Success = false;
                    return result;
                }

                if (document.DocumentNum != null)
                {
                    if (document.TypeDocumentGji == TypeDocumentGji.Protocol)
                    {
                        string UIN = "39645f";
                        var prot = this.Container.ResolveDomain<Protocol>().Get(document.Id);
                        string s1 = Convert.ToInt32(UIN, 16).ToString().PadLeft(8, '0');
                        string s2 = (document.DocumentDate?.ToString("yyyyMMdd") ?? "00000000");
                        string s3 = "";
                        if (document.DocumentNumber.Contains("-"))
                        {
                                if (document.DocumentNumber.Split('-').Count() > 2)
                                {
                                    s3 = (document.DocumentNumber.Split('-')[1] + document.DocumentNumber.Split('-')[2]).PadRight(8, '0');
                                }
                                else if (document.DocumentNumber.Split('-').Count() == 2)
                                {
                                    s3 = document.DocumentNumber.Split('-')[1].PadRight(8, '0');
                                }
                                else
                                {
                                    s3 = document.DocumentNumber.Replace("-", "").PadRight(8, '0');
                                }                      
                        }
                        else
                        {
                            s3 = document.DocumentNumber.PadRight(8, '0');
                        }
                        s3 = s3.Replace('/', '1');
                        s3 = s3.Replace('\\', '0');
                        s3 = s3.Replace('№', '4');
                        char[] charsS3 = s3.ToCharArray();
                        for (int i = 0; i < s3.Length; i++)
                        {
                            if (!char.IsDigit(charsS3[i]))
                            {
                                s3 = s3.Replace(charsS3[i], '0');
                            }
                        }
                        prot.UIN = (s1 + s2 + s3).Substring(0, 24);
                        prot.UIN += CheckSum(prot.UIN);
                        Container.ResolveDomain<Protocol>().Update(prot);

                  }
                    else if (document.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                    {
                        string UIN = "39645f";
                        var resPros = this.Container.ResolveDomain<ResolPros>().Get(document.Id);
                        string s1 = Convert.ToInt32(UIN, 16).ToString().PadLeft(8, '0');
                        string s2 = (document.DocumentDate?.ToString("yyyyMMdd") ?? "00000000");
                        string s3 = "";
                        if (document.DocumentNumber.Contains("-"))
                        {
                            if (document.DocumentNumber.Split('-').Count() > 2)
                            {
                                s3 = (document.DocumentNumber.Split('-')[1] + document.DocumentNumber.Split('-')[2]).PadRight(8, '0');
                            }
                            else if (document.DocumentNumber.Split('-').Count() == 2)
                            {
                                s3 = document.DocumentNumber.Split('-')[1].PadRight(8, '0');
                            }
                            else
                            {
                                s3 = document.DocumentNumber.Replace("-", "").PadRight(8, '0');
                            }
                        }
                        else
                        {
                            s3 = document.DocumentNumber.PadRight(8, '0');
                        }
                        s3 = s3.Replace('/', '1');
                        s3 = s3.Replace('\\', '0');
                        s3 = s3.Replace('№', '4');
                        char[] charsS3 = s3.ToCharArray();
                        for (int i = 0; i < s3.Length; i++)
                        {
                            if (!char.IsDigit(charsS3[i]))
                            {
                                s3 = s3.Replace(charsS3[i], '0');
                            }
                        }
                        resPros.UIN = (s1 + s2 + s3).Substring(0, 24);
                        resPros.UIN += CheckSum(resPros.UIN);
                        Container.ResolveDomain<ResolPros>().Update(resPros);

                    }
                    result.Success = true;
                return result;
                }

                if (document.TypeDocumentGji == TypeDocumentGji.Protocol)
                {

                }

                switch (document.TypeDocumentGji)
                {
                    case TypeDocumentGji.Disposal:
                        this.SetDisposalDocNum(document);
                        break;
                    case TypeDocumentGji.Decision:
                    case TypeDocumentGji.ProtocolMhc:
                    case TypeDocumentGji.ProtocolMvd:
                        this.SetAlterDocNum(document);
                        break;
                    default:
                        this.SetDocNum(document);
                        break;
                }

        
            }

            result.Success = true;
            return result;
        }

        internal void SetDisposalDocNum(DocumentGji document)
        {
            var docNum = this.DocumentGjiDomain.GetAll()
                .Where(x => x.TypeDocumentGji == document.TypeDocumentGji)
                .Where(x => x.DocumentDate.HasValue &&
                      x.DocumentDate.Value.Year == document.DocumentDate.Value.Year)
                .Where(x => x.DocumentNum.HasValue)
                .Select(x => x.DocumentNum.Value)
                .SafeMax(x => x);

            docNum++;

            document.DocumentNum = docNum;
            document.DocumentNumber = document.DocumentDate.Value.Year.ToString().Substring(2) + "-0" + docNum;
        }

        internal void SetAlterDocNum(DocumentGji document)
        {
            var docNum = this.DocumentGjiDomain.GetAll()
                .Where(x => x.TypeDocumentGji == document.TypeDocumentGji)
                .Where(x => x.DocumentDate.HasValue &&
                      x.DocumentDate.Value.Year == document.DocumentDate.Value.Year)
                .Where(x => x.DocumentNum.HasValue)
                .Select(x => x.DocumentNum.Value)
                .SafeMax(x => x);

            docNum++;

            document.DocumentNum = docNum;
            document.DocumentNumber = document.DocumentDate.Value.Year.ToString().Substring(2) + "-" + docNum;
        }

        internal void SetDocNum(DocumentGji document)
        {
            var typeDoc = new[]
            {
                TypeDocumentGji.Disposal, TypeDocumentGji.ProtocolMhc, TypeDocumentGji.ProtocolMvd, TypeDocumentGji.Decision
            };
            var disposal = this.GetParentDocument(document, typeDoc);

            if (disposal != null)
            {
                var docNum = this.GetChildrenDocuments(disposal)
                    .Where(x => x.DocumentNum.HasValue)
                    .Select(x => x.DocumentNum.Value)
                    .SafeMax(x => x);
                docNum++;

                document.DocumentNum = docNum;
                document.DocumentNumber = document.DocumentDate.Value.Year.ToString().Substring(2) + '-' + disposal.DocumentNum + '-' + docNum;
                if (document.TypeDocumentGji == TypeDocumentGji.Protocol)
                {
                    string UIN = "39645f";
                    var prot = this.Container.ResolveDomain<Protocol>().Get(document.Id);
                    string s1 = Convert.ToInt32(UIN, 16).ToString().PadLeft(8, '0');
                    string s2 = (document.DocumentDate?.ToString("yyyyMMdd") ?? "00000000");
                    string s3 = "";
                    if (document.DocumentNumber.Contains("-"))
                    {
                        if (document.DocumentNumber.Split('-').Count() > 2)
                        {
                            s3 = (document.DocumentNumber.Split('-')[1] + document.DocumentNumber.Split('-')[2]).PadRight(8, '0');
                        }
                        else if (document.DocumentNumber.Split('-').Count() == 2)
                        {
                            s3 = document.DocumentNumber.Split('-')[1].PadRight(8, '0');
                        }
                        else
                        {
                            s3 = document.DocumentNumber.Replace("-", "").PadRight(8, '0');
                        }
                    }
                    else
                    {
                        s3 = document.DocumentNumber.PadRight(8, '0');
                    }
                    s3 = s3.Replace('/', '1');
                    s3 = s3.Replace('\\', '0');
                    s3 = s3.Replace('№', '4');
                    char[] charsS3 = s3.ToCharArray();
                    for (int i = 0; i < s3.Length; i++)
                    {
                        if (!char.IsDigit(charsS3[i]))
                        {
                            s3 = s3.Replace(charsS3[i], '0');
                        }
                    }
                    prot.UIN = (s1 + s2 + s3).Substring(0, 24);
                    prot.UIN += CheckSum(prot.UIN);
                    Container.ResolveDomain<Protocol>().Update(prot);

                }
            }
        }

        public DocumentGji GetParentDocument(DocumentGji document, TypeDocumentGji[] type)
        {
            var result = document;

            if (!type.Any(x => x == document.TypeDocumentGji))
            {
                var docs = this.DocumentGjiChildrenDomain.GetAll()
                                    .Where(x => x.Children.Id == document.Id)
                                    .Select(x => x.Parent)
                                    .ToList();

                foreach (var doc in docs)
                {
                    result = this.GetParentDocument(doc, type);
                }
            }

            if (result != null)
            {
                return type.Any(x => x == result.TypeDocumentGji) ? result : null;
            }

            return null;
        }

        public List<DocumentGji> GetChildrenDocuments(DocumentGji document)
        {
            var docs = this.DocumentGjiChildrenDomain.GetAll()
                               .Where(x => x.Parent.Id == document.Id)
                               .Select(x => x.Children)
                               .ToList();

            List<DocumentGji> children = new List<DocumentGji>();

            foreach (var doc in docs)
            {
                children.AddRange(this.GetChildrenDocuments(doc));
            }

            docs.AddRange(children);
            return docs;
        }

        internal Int32 CheckSum(String number)
        {
            Int32 result = CheckSum(number, 1);

            return result != 10 ? result : CheckSum(number, 3) % 10;
        }

        private Int32 CheckSum(String number, Int32 ves)
        {
            int sum = 0;
            for (int i = 0; i < number.Length; i++)
            {
                int t = (int)Char.GetNumericValue(number[i]);
                int rrr = ((ves % 10) == 0 ? 10 : ves % 10);

                sum += t * rrr;
                ves++;
            }

            return sum % 11;
        }

    }
}