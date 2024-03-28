namespace Bars.GisIntegration.Tor.Service.Impl
{
	using System;
	using System.IO;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using System.Threading;

	using Bars.B4;
	using Bars.B4.DataAccess;
	using Bars.B4.IoC;
	using Bars.B4.Logging;
	using Bars.B4.Modules.Analytics.Reports;
	using Bars.GisIntegration.Tor.ConfigSections;
	using Bars.GisIntegration.Tor.Entities;
	using Bars.GisIntegration.Tor.Enums;
	using Bars.GisIntegration.Tor.GraphQl;
	using Bars.GisIntegration.Tor.Service.LogService;
	using Bars.Gkh.Entities.Base;
	using Bars.Gkh.Utils;

	using Castle.Windsor;

	using Fasterflect;

	using Newtonsoft.Json;

	public class TorIntegrationService : ITorIntegrationService
	{
		private string Url { get; }

		private string AuthToken { get; }

		public ILogManager LogManager { get; set; }

		public IWindsorContainer Container { get; set; }

		public ITorLogService TorLogService { get; set; }

		public IUserInfoProvider UserInfoProvider { get; set; }

		public TorIntegrationService(IWindsorContainer container)
		{
			var config = container.GetGkhConfig<TorIntegrationConfig>();
			this.Url = config.Address ?? string.Empty;
			this.AuthToken = config.Token ?? string.Empty;
		}

        public void SendRequest<TObject>(
            CancellationToken cancellationToken,
            TypeRequest typeRequest,
            TObject sendObject,
            string request,
            string entityResponse,
            TorTask task,
            out string responseValue)
            where TObject : IUsedInTorIntegration
        {
            responseValue = null;
            this.TorLogService.AddLogRecord("Отправление запроса.", task.Id);
            var response = this.GetResponse(task, request, cancellationToken, out var exception);

            if (response == null)
            {
                task.TorTaskState = TorTaskState.NotComplete;
                var logMessage = $"Нет ответа от сервера. {exception}";
                this.LogManager.Debug(logMessage);
                this.TorLogService.AddLogRecord(logMessage, task.Id);
                return;
            }

            if (!response.IsSuccessStatusCode)
            {
                var logMessage = $"Ошибка отправки. {response}";
                this.LogManager.Debug(logMessage);
                this.TorLogService.AddLogRecord(logMessage, task.Id);
                task.TorTaskState = TorTaskState.NotComplete;
                return;
            }

            this.TorLogService.AddLogRecord("Ответ получен. Обработка ответа.", task.Id);
            var responseStream = response.Content.ReadAsStreamAsync().Result;
            using (var reader = new StreamReader(responseStream))
            {
                responseValue = reader.ReadToEnd();
            }

            try
            {
                IGraphQlResponse deserializeObject = null;
                if (typeRequest == TypeRequest.Getting)
                {
                    deserializeObject = this.DeserializeObject<GraphQlResponse<Query>>(responseValue);
                    task.TorTaskState = TorTaskState.CompleteSuccess;
                }
                else
                {
                    deserializeObject = this.DeserializeObject<GraphQlResponse<Mutation>>(responseValue);
                    this.ProcessResponse(sendObject, deserializeObject, entityResponse, task);
                    task.TorTaskState = task.TorTaskState == TorTaskState.CompleteWithErrors ? task.TorTaskState : TorTaskState.CompleteSuccess;
                }

                this.TorLogService.AddLogRecord("Успешное выполнение.", task.Id);
            }
            catch
            {
                var logMessage = $"Получен некорректный ответ. {response}";
                this.LogManager.Debug(logMessage);
                this.TorLogService.AddLogRecord(logMessage, task.Id);
                task.TorTaskState = TorTaskState.NotComplete;
            }
        }

        public virtual void ProcessResponse<TObject>(TObject sendObject, IGraphQlResponse response, string typeClass, TorTask task)
			where TObject : IUsedInTorIntegration
		{
			var data = response.TryGetPropertyValue("Data");
		    var errors = response.TryGetPropertyValue("Errors");

		    if (data is null || !(errors is null))
		    {
		        task.TorTaskState = TorTaskState.CompleteWithErrors;
                return;
		    }

			var entity = data.TryGetPropertyValue(typeClass);
			var id = entity?.TryGetPropertyValue("Id");

			if (id is Guid guid)
			{
				this.SaveGuid(guid, sendObject);
			}
		}

		public IGraphQlResponse SendGetRequest(string typeClass, string request, CancellationToken cancellationToken)
		{
			var response = this.GetResponse(null, request, cancellationToken, out _);
			
			IGraphQlResponse responseObject = null;

			if (response != null)
			{
                string responseValue;
                var responseStream = response.Content.ReadAsStreamAsync().Result;
				using (var reader = new StreamReader(responseStream))
				{
					responseValue = reader.ReadToEnd();
				}

				if (response.IsSuccessStatusCode)
				{
					try
					{
						responseObject = this.DeserializeObject<GraphQlResponse<Query>>(responseValue);

					    var data = responseObject.TryGetPropertyValue("Data");
                        var entity = data.TryGetPropertyValue(typeClass);

						if (data is null || entity is null) responseObject = null;
					}
					catch (Exception e)
					{
						responseObject = null;
					}
				}
			}

			return responseObject;
		}

		private HttpClient GetClient(string token)
		{
			var authValue = new AuthenticationHeaderValue("Bearer", token);

			var client = new HttpClient()
			{
				DefaultRequestHeaders = { Authorization = authValue }
			};
			return client;
		}

		private HttpContent GenerateHttpContent(string request)
		{
			return new StringContent(JsonConvert.SerializeObject(new { query = request }), Encoding.UTF8, "application/json");
		}

		private T DeserializeObject<T>(string responseValue)
		{
			return JsonConvert.DeserializeObject<T>(responseValue);
		}

		private HttpResponseMessage GetResponse(TorTask task, string request, CancellationToken cancellationToken, out Exception exception)
		{
			HttpResponseMessage response = null;

			var uri = new Uri(this.Url);
			using (var client = this.GetClient(this.AuthToken))
			{
				try
				{
					response = client.PostAsync(uri, this.GenerateHttpContent(request), cancellationToken).Result;
                    exception = null;
                }
				catch (Exception e)
                {
                    exception = e;
					this.LogManager.Error("Ошибка отправки: ", e);
				}
			}

			return response;
		}

		private void SaveGuid<T>(Guid? guidTor, T sendObject)
			where T : IUsedInTorIntegration
		{
            var sendObjectType = sendObject.GetType().IsAutoClass ? sendObject.GetType().BaseType :sendObject.GetType();
            var domainServiceType = typeof(IDomainService<>).MakeGenericType(sendObjectType);
            var domainService = (IDomainService)this.Container.Resolve(domainServiceType);

            using (this.Container.Using(domainService))
            {
                if (domainService.Get(sendObject.Id) is IUsedInTorIntegration entity && entity.TorId != guidTor)
                {
                    entity.TorId = guidTor;
                    domainService.Update(entity);
                }
            }
        }
	}
}