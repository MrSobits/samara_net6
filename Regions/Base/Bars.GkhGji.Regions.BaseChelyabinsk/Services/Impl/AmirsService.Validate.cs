using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Services.Impl.Intfs;
using Bars.GkhGji.Services.DataContracts;

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Services.Impl
{
    /// <summary>
    /// Сервис импорта из АМИРС - валидация
    /// </summary>
    public partial class AmirsService
    {
        /// <summary>
        /// Валидация
        /// </summary>
        protected Dictionary<AmirsData, IDataResult> ValidateAmirs(IEnumerable<AmirsData> records)
        {
            var result = new Dictionary<AmirsData, IDataResult>();

            if (records == null)
            {
                result.Add(
                    new AmirsData(),
                    new BaseDataResult
                    {
                        Message = "Пустой объект",
                        Success = false
                    });
                return result;
            }

            var errors = new StringBuilder();
            foreach (var record in records)
            {
                errors.Clear();
                IValidator<AmirsData> amirsValidator;
                using (this.Container.Using(amirsValidator = this.Container.Resolve<IValidator<AmirsData>>()))
                {
                    var properties = typeof(AmirsData).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    properties.ForEach(x => errors.Append(amirsValidator.Validate(x, record).Message));
                }
                if (record.prot_date == null)
                {
                    errors.Append($"Для постановления не найдена дата протокола");
                }

                if (record.prot_num == null)
                {
                    errors.Append($"Для постановления не найден номер протокола");
                }

                if (record.resolution_type == null)
                {
                    errors.Append($"Для постановления не найден тип постановления");
                }

                if (record.resolution_date == null)
                {
                    errors.Append($"Для постановления не найдена дата постановления");
                }

                if (record.resolution_num == null)
                {
                    errors.Append($"Для постановления не найден номер постановления");
                }

                //var currentProtocol = ProtocolDomain.GetAll()
                //    .Where(x => x.DocumentNumber == record.prot_num && x.DocumentDate.HasValue && x.DocumentDate.Value == record.prot_date).FirstOrDefault();
                var currentProtocol = ProtocolDomain.GetAll()
                    .Where(x => x.DocumentNumber == record.prot_num).OrderByDescending(x => x.Id).FirstOrDefault();

                var currentProtocol197 = Protocol197Domain.GetAll()
                   .Where(x => x.DocumentNumber == record.prot_num).OrderByDescending(x => x.Id).FirstOrDefault();

                if (currentProtocol == null && currentProtocol197 == null)
                {
                    errors.Append($"Для постановления не найден протокол в системе");
                }

                if (errors.Length == 0)
                {
                    result.Add(
                        record,
                        new BaseDataResult
                        {
                            Success = true
                        });
                }
                else
                {
                    result.Add(
                        record,
                        new BaseDataResult
                        {
                            Success = false,
                            Message = errors.ToString()
                        });
                }
            }

            return result;
        }
    }
}
