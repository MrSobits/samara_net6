using Bars.Gkh.Enums;
using Bars.GkhGji.Entities.Dict;

namespace Bars.GkhGji.Entities.PosAppeal
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    public class PosAnswer
    {
        public bool answer { get; set; }
        public string answerText { get; set; }
        public string appealAnswerType { get; set; }
        public string appealIssueType { get; set; }
        public string[] attachmentIds { get; set; }
        public string comment { get; set; }
        public Executorinfo executorInfo { get; set; }
        public Int64 id { get; set; }
        public Managerinfo managerInfo { get; set; }
        public bool notifyApplicant { get; set; }
        public Int64 opaId { get; set; }
        public string opaName { get; set; }
        public string outgoingNumber { get; set; }
        public string outgoingNumberSetAt { get; set; }
        public DateTime? postponedDate { get; set; }
        public string reassignmentInfo { get; set; }
        public string regNumber { get; set; }
        public string regNumberSetAt { get; set; }
        public string status { get; set; }
        public string statusText { get; set; }
    }

    public class Executorinfo
    {
        public string email { get; set; }
        public string name { get; set; }
        public string patronymic { get; set; }
        public string phone { get; set; }
        public string surname { get; set; }
    }

    public class Managerinfo
    {
        public string email { get; set; }
        public string name { get; set; }
        public string patronymic { get; set; }
        public string phone { get; set; }
        public string surname { get; set; }
    }
}