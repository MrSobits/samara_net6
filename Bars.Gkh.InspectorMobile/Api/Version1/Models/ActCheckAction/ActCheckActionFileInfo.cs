namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheckAction
{
    using System;

    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Common;

    using Newtonsoft.Json;

    /// <inheritdoc />
    public class ActCheckActionFileInfoGet : FileInfoGet
    {
        /// <inheritdoc />
        [JsonIgnore]
        public override DateTime? FileDate { get; set; }
    }

    /// <inheritdoc />
    public class ActCheckActionFileInfoCreate : FileInfoCreate
    {
        /// <inheritdoc />
        [JsonIgnore]
        public override DateTime? FileDate { get; set; }
    }

    /// <inheritdoc />
    public class ActCheckActionFileInfoUpdate : FileInfoUpdate
    {
        /// <inheritdoc />
        [JsonIgnore]
        public override DateTime? FileDate { get; set; }
    }
}