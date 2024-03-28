namespace Bars.GisIntegration.Tor.Service
{
	using System.Threading;

	using Bars.GisIntegration.Tor.Entities;
	using Bars.GisIntegration.Tor.Enums;
	using Bars.GisIntegration.Tor.GraphQl;
	using Bars.Gkh.Entities.Base;

	public interface ITorIntegrationService
	{
		/// <summary>
		/// Отправить запрос
		/// </summary>
		/// <param name="cancellationToken">Токен отмены</param>
		/// <param name="typeRequest">Тип запроса</param>
		/// <param name="sendObject">Отправляемый объект</param>
		/// <param name="request">Запрос</param>
		/// <param name="entityResponse">Сущность ответа</param>
		/// <param name="task">Задача</param>
		/// <param name="responseValue">Полученный ответ</param>
		void SendRequest<TObject>(
			CancellationToken cancellationToken,
			TypeRequest typeRequest,
			TObject sendObject,
			string request,
			string entityResponse,
			TorTask task,
			out string responseValue)
			where TObject : IUsedInTorIntegration;

        /// <summary>
        /// Обработка ответа
        /// </summary>
        /// <param name="sendObject">Отправляемый объект</param>
        /// <param name="response">Ответ</param>
        /// <param name="typeClass">Сущность ответа</param>
        /// <param name="task">Задача</param>
        void ProcessResponse<TObject>(TObject sendObject, IGraphQlResponse response, string typeClass, TorTask task)
	        where TObject : IUsedInTorIntegration;

		IGraphQlResponse SendGetRequest(string typeClass, string request, CancellationToken cancellationToken);
	}
}