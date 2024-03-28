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
using System;
using Bars.B4.DataAccess;

namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange.Rule
{
    public class ProtocolNumberValidationChelyabinskRule : BaseDocNumberValidationChelyabinskRule
    {
        public override string Id { get { return "gji_nso_protocol_validation_number"; } }

        public override string Name { get { return "Челябинск - Присвоение номера протокола"; } }

        public override string TypeId { get { return "gji_document_prot"; } }

        public override string Description { get { return "Данное правило присваивает номера протокола"; } }
        public IDomainService<ProtocolArticleLaw> ProtocolArticleLawDomain { get; set; }
        public override ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var result = new ValidateResult();

            

            var document = statefulEntity as DocumentGji;
            //провверяем статьи закона            
            var artLaw = ProtocolArticleLawDomain.GetAll()
                .Where(x => x.Protocol.Id == document.Id).FirstOrDefault();
            if (artLaw == null)
            {
                result.Message = "Не указана статья закона, перевод статуса запрещен.";
                result.Success = false;
                return result;
            }

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
                    case TypeDocumentGji.Decision:
                    case TypeDocumentGji.ProtocolMhc:
                    case TypeDocumentGji.ProtocolMvd:
                        this.SetDisposalDocNum(document);
                        break;
                    default:
                        this.SetDocNum(document);
                        break;
                }


            }

            result.Success = true;
            return result;
        }

    }
}