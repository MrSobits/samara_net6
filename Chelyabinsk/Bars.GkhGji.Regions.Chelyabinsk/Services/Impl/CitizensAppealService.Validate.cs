using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Utils;
using Bars.GkhGji.Regions.Chelyabinsk.Services.Impl.Intfs;
using Bars.GkhGji.Services.DataContracts;

namespace Bars.GkhGji.Regions.Chelyabinsk.Services.Impl
{
    /// <summary>
    /// Сервис сведений об обращениях граждан - валидация
    /// </summary>
    public partial class CitizensAppealService
    {
        /// <summary>
        /// Валидация
        /// </summary>
        protected Dictionary<CitizenAppeal, IDataResult> ValidateCitizenAppeals(IEnumerable<CitizenAppeal> appeals)
        {
            var result = new Dictionary<CitizenAppeal, IDataResult>();

            if (appeals == null)
            {
                result.Add(
                    new CitizenAppeal(),
                    new BaseDataResult
                    {
                        Message = "Пустой объект",
                        Success = false
                    });
                return result;
            }

            var errors = new StringBuilder();
            foreach (var appeal in appeals)
            {
                errors.Clear();
                IValidator<CitizenAppeal> appealValidator;
                using (this.Container.Using(appealValidator = this.Container.Resolve<IValidator<CitizenAppeal>>()))
                {
                    var properties = typeof(CitizenAppeal).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                    properties.ForEach(x => errors.Append(appealValidator.Validate(x, appeal).Message));
                }

                if (appeal.SourceOfReceipts == null || appeal.SourceOfReceipts.Length == 0)
                {
                    errors.Append($"Поле 'Источник поступления' не заполнено. {Environment.NewLine}");
                }
                else
                {
                    foreach (var sourceOfReceipt in appeal.SourceOfReceipts)
                    {
                        IValidator<SourceOfReceipt> validator;
                        using (this.Container.Using(validator = this.Container.Resolve<IValidator<SourceOfReceipt>>()))
                        {
                            var properties = typeof(SourceOfReceipt).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                            properties.ForEach(x => errors.Append(validator.Validate(x, sourceOfReceipt).Message));
                        }
                    }
                }

                if (appeal.CategoriesOfDeclarant == null || appeal.CategoriesOfDeclarant.Length == 0)
                {
                    errors.Append($"Поле 'Категория заявителя' не заполнено. {Environment.NewLine}");
                }

                if (appeal.AppealAttachments != null)
                {
                    foreach (var attachment in appeal.AppealAttachments)
                    {
                        IValidator<AppealAttachment> validator;
                        using (this.Container.Using(validator = this.Container.Resolve<IValidator<AppealAttachment>>()))
                        {
                            var properties = typeof(AppealAttachment).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                            properties.ForEach(x => errors.Append(validator.Validate(x, attachment).Message));
                        }
                    }
                }

                if (appeal.KindOfQuestions == null || appeal.KindOfQuestions.Length == 0)
                {
                    errors.Append($"Поле 'Виды и типы вопросов' не заполнено. {Environment.NewLine}");
                }

                if (appeal.AppealQuestions == null || appeal.AppealQuestions.Length == 0)
                {
                    errors.Append(
                        $"Поле 'Список вопросов, поставленных в обращении (из Тематического классификатора)' не заполнено. {Environment.NewLine}");
                }
                else
                {
                    foreach (var appealQuestion in appeal.AppealQuestions)
                    {
                        IValidator<AppealQuestion> validator;
                        using (this.Container.Using(validator = this.Container.Resolve<IValidator<AppealQuestion>>()))
                        {
                            var properties = typeof(AppealQuestion).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                            properties.ForEach(x => errors.Append(validator.Validate(x, appealQuestion).Message));
                        }
                    }
                }

                if (appeal.Executives == null || appeal.Executives.Length == 0)
                {
                    errors.Append($"Поле 'Кем рассмотрено обращение (руководитель)' не заполнено. {Environment.NewLine}");
                }
                else
                {
                    foreach (var executive in appeal.Executives)
                    {
                        IValidator<Executive> validator;
                        using (this.Container.Using(validator = this.Container.Resolve<IValidator<Executive>>()))
                        {
                            var properties = typeof(Executive).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                            properties.ForEach(x => errors.Append(validator.Validate(x, executive).Message));
                        }
                    }
                }

                if (appeal.Executants == null || appeal.Executants.Length == 0)
                {
                    errors.Append($"Поле 'Исполнитель' не заполнено. {Environment.NewLine}");
                }
                else
                {
                    foreach (var executant in appeal.Executants)
                    {
                        IValidator<Executant> validator;
                        using (this.Container.Using(validator = this.Container.Resolve<IValidator<Executant>>()))
                        {
                            var properties = typeof(Executant).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                            properties.ForEach(x => errors.Append(validator.Validate(x, executant).Message));
                        }
                    }
                }
                if (appeal.RegistrarOfAppeal != null)
                {
                    IValidator<RegistrarOfAppeal> validator;
                    using (this.Container.Using(validator = this.Container.Resolve<IValidator<RegistrarOfAppeal>>()))
                    {
                        var properties = typeof(RegistrarOfAppeal).GetProperties(BindingFlags.Instance | BindingFlags.Public);
                        properties.ForEach(x => errors.Append(validator.Validate(x, appeal.RegistrarOfAppeal).Message));
                    }
                }

                if (errors.Length == 0)
                {
                    result.Add(
                        appeal,
                        new BaseDataResult
                        {
                            Success = true
                        });
                }
                else
                {
                    result.Add(
                        appeal,
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
