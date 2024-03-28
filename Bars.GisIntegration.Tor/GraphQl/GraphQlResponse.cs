namespace Bars.GisIntegration.Tor.GraphQl
{
    using System.Collections.Generic;

    public interface IGraphQlResponse
    {
        GraphQlErrors[] Errors { get; set; }
    }

    public class GraphQlResponse<T> : IGraphQlResponse
    {
        /// <summary>
        /// Тело ответа
        /// </summary>
        public T Data { get; set; }

	    /// <summary>
	    ///  Ошибка
	    /// </summary>
		public GraphQlErrors[] Errors { get; set; }
    }

    public class GraphQlErrors
    {
        public IDictionary<string, dynamic> Extentions { get; set; }

        public GraphQlLocation[] Locations { get; set; }

        public string ErrorType { get; set; }

        public string Message { get; set; }

        public dynamic[] Path { get; set; }
    }

    public class GraphQlLocation
    {
        public int Column { get; set; }

        public int Line { get; set; }

        public string SourceName { get; set; }
    }
}