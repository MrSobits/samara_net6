namespace Bars.GkhGji.Regions.Chelyabinsk.Interceptors
{
    using System;
    using System.Collections.Generic;
    using Bars.B4.Modules.States;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Entities;
    using System.Linq;
    using Bars.GkhGji.Entities;

    class ResolProsInterceptor : EmptyDomainInterceptor<ResolPros>
    {
        public IGkhUserManager UserManager { get; set; }

       

        public override IDataResult BeforeUpdateAction(IDomainService<ResolPros> service, ResolPros entity)
        {
            try
            {
                string UIN = "39645f";
                var document = this.Container.Resolve<IDomainService<DocumentGji>>().Get(entity.Id);
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
                entity.UIN = (s1 + s2 + s3).Substring(0, 24);
                entity.UIN += CheckSum(entity.UIN);
            }
            catch { }

            return Success();
                
            
        }

        private Int32 CheckSum(String number)
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
