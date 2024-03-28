namespace Bars.Gkh.BaseApiIntegration.Controllers
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Http;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Logging;
    using Bars.B4.Utils;
    using Bars.Gkh.BaseApiIntegration.Models;

    using Castle.Windsor;

    using Npgsql;

    /// <summary>
    /// Базовый класс API-контроллера
    /// </summary>
    [Authorize]
    public class BaseApiController : ApiController
    {
        /// <summary>
        /// IoC-контейнер
        /// </summary>
        protected readonly IWindsorContainer Container = ApplicationContext.Current.Container;

        /// <summary>
        /// Менеджер логгирования
        /// </summary>
        protected readonly ILogManager LogManager;

        /// <inheritdoc cref="BaseApiController"/>
        public BaseApiController()
        {
            this.LogManager = this.Container.Resolve<ILogManager>();
        }

        /// <summary>
        /// Вернуть успешный результат (статус 200 + result в data)
        /// </summary>
        protected IHttpActionResult SuccessResult(object result) =>
            this.Ok(new BaseApiResponse { Data = result });

        /// <summary>
        /// Вернуть ошибку (статус 400 + errorMessage)
        /// </summary>
        protected IHttpActionResult ErrorResult(string errorMessage) =>
            this.ErrorResult(new CustomError { Message = errorMessage });

        /// <summary>
        /// Вернуть ошибку (статус 400 + customError) 
        /// </summary>
        protected IHttpActionResult ErrorResult(CustomError customError) =>
            this.Content(HttpStatusCode.BadRequest, new BaseApiResponse { Error = customError });

        /// <summary>
        /// Выполнить метод сервиса (resolve + обработка ошибок)
        /// </summary>
        /// <param name="name">Наименование метода</param>
        /// <param name="params">Параметры метода</param>
        /// <typeparam name="TService">Тип сервиса</typeparam>
        protected async Task<IHttpActionResult> ExecuteApiServiceMethodAsync<TService>(string name, params object[] @params) =>
            await this.ExecuteApiServiceMethodWithResultCastingAsync<TService>(this.SuccessResult, name, @params);

        /// <summary>
        /// Выполнить метод сервиса (resolve + обработка ошибок)
        /// </summary>
        /// <param name="successResultCastingFunc">Функция для приведения успешного результата</param>
        /// <param name="name">Наименование метода</param>
        /// <param name="params">Параметры метода</param>
        /// <typeparam name="TService">Тип сервиса</typeparam>
        protected async Task<IHttpActionResult> ExecuteApiServiceMethodWithResultCastingAsync<TService>(Func<object, IHttpActionResult> successResultCastingFunc, string name, params object[] @params)
        {
            try
            {
                var service = this.Container.Resolve<TService>();

                using (this.Container.Using(service))
                {
                    var method = service.GetType().GetMethod(name);
                    var methodResult = method.Invoke(service, @params);

                    object result;
                    if (methodResult is Task)
                    {
                        await (Task)methodResult;
                        var resultProperty = methodResult.GetType().GetProperty("Result");
                        result = resultProperty?.GetValue(methodResult);
                    }
                    else
                    {
                        result = methodResult;
                    }

                    if (result is PagedApiServiceResult pagedResult)
                    {
                        return this.Ok(new PagedApiResponse { Data = pagedResult.Data, NextPageGuid = pagedResult.NextPageGuid });
                    }

                    return successResultCastingFunc.Invoke(result);
                }
            }
            catch (TargetInvocationException e)
            {
                return this.GetHandledErrorResult(e.InnerException ?? e);
            }
            catch (Exception e)
            {
                return this.GetHandledErrorResult(e);
            }
        }

        /// <summary>
        /// Добавить ошибку в лог
        /// </summary>
        /// <param name="exception">Ошибка</param>
        private void AddErrorToLog(Exception exception) =>
            this.LogManager.Error("Ошибка API-метода", exception);

        /// <summary>
        /// Получить результат обрабатываемой ошибки
        /// </summary>
        private IHttpActionResult GetHandledErrorResult(Exception exception)
        {
            var errorMessage = string.Empty;

            switch (exception)
            {
                case PostgresException postgresException:
                    // Postgresql Raise Exceptions
                    if (postgresException.Code == "P0001")
                    {
                        errorMessage = postgresException.MessageText;
                    }
                    break;
                case ApiServiceException apiServiceException:
                case ValidationException validationException:
                    errorMessage = exception.Message;
                    break;
            }

            // Вернуть ошибку в 400-ом статусе
            if (errorMessage.IsNotEmpty())
            {
                return this.ErrorResult(errorMessage);
            }

            this.AddErrorToLog(exception);

            // Вернуть 500-ую ошибку
            return this.InternalServerError(exception);
        }
    }

    /// <summary>
    /// Ошибка API-сервиса
    /// </summary>
    /// <remarks>
    /// Используется для отделения собственных ошибок сервисов от системных
    /// </remarks>
    public class ApiServiceException : Exception
    {
        public ApiServiceException(string errorMsg)
            : base(errorMsg)
        {
        }
    }
}