namespace Bars.GkhCalendar
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.GkhCalendar.Enums;
    using System;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/DayType.js", new ExtJsEnumResource<DayType>("B4.enums.DayType"));
            container.Add("libs/B4/enums/TimeSlot.js", new ExtJsEnumResource<TimeSlot>("B4.enums.TimeSlot"));
            container.Add("libs/B4/enums/DayOfWeekRus.js", new ExtJsEnumResource<DayOfWeekRus>("B4.enums.DayOfWeekRus"));
            container.Add("libs/B4/enums/TypeOrganisation.js", new ExtJsEnumResource<TypeOrganisation>("B4.enums.TypeOrganisation"));
            container.Add("libs/B4/enums/ChangeAppointmentDay.js", new ExtJsEnumResource<ChangeAppointmentDay>("B4.enums.ChangeAppointmentDay"));
        }
    }
}
