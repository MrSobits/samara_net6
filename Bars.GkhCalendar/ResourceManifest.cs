
namespace Bars.GkhCalendar
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/Appointment.js");
            AddResource(container, "libs/B4/controller/IndustrialCalendar.js");
            AddResource(container, "libs/B4/model/AppointmentDiffDayGridModel.js");
            AddResource(container, "libs/B4/model/AppointmentGridModel.js");
            AddResource(container, "libs/B4/model/AppointmentTimeGridModel.js");
            AddResource(container, "libs/B4/model/Day.js");
            AddResource(container, "libs/B4/store/AppointmentDiffDayGridStore.js");
            AddResource(container, "libs/B4/store/AppointmentGridStore.js");
            AddResource(container, "libs/B4/store/AppointmentTimeGridStore.js");
            AddResource(container, "libs/B4/store/Month.js");
            AddResource(container, "libs/B4/view/AppointmentDiffDayEditWindow.js");
            AddResource(container, "libs/B4/view/AppointmentDiffDayGrid.js");
            AddResource(container, "libs/B4/view/AppointmentEditWindow.js");
            AddResource(container, "libs/B4/view/AppointmentGrid.js");
            AddResource(container, "libs/B4/view/AppointmentTimeEditWindow.js");
            AddResource(container, "libs/B4/view/AppointmentTimeGrid.js");
            AddResource(container, "libs/B4/view/IndustrialCalendar.js");
            AddResource(container, "libs/B4/view/calendarday/EditWindow.js");


        }

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhCalendar.dll/Bars.GkhCalendar.{0}", path.Replace("/", ".")));
        }
    }
}
