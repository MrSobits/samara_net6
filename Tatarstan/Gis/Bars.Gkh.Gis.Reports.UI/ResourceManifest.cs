namespace Bars.Gkh.Gis.Reports.UI
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/BaseGisReportController.js");
            AddResource(container, "libs/B4/controller/report/Report_10_61_1.js");
            AddResource(container, "libs/B4/controller/report/Report_3_63_3.js");
            AddResource(container, "libs/B4/controller/report/Report_5_37_1.js");
            AddResource(container, "libs/B4/controller/report/report_5_37_2.js");
            AddResource(container, "libs/B4/controller/report/report_5_37_3.js");
            AddResource(container, "libs/B4/controller/report/Report_6_46_1.js");
            AddResource(container, "libs/B4/controller/report/Report_MKD.js");
            AddResource(container, "libs/B4/controller/report/Report_SZ_Charge.js");
            AddResource(container, "libs/B4/controller/report/Report_SZ_Charge_MKD.js");
            AddResource(container, "libs/B4/controller/report/Report_SZ_Collection.js");
            AddResource(container, "libs/B4/controller/report/Report_SZ_Collection_Service.js");
            AddResource(container, "libs/B4/controller/report/Report_SZ_Indicator.js");
            AddResource(container, "libs/B4/controller/report/Report_SZ_Security.js");
            AddResource(container, "libs/B4/form/GisMonthPicker.js");
            AddResource(container, "libs/B4/view/report/Report_10_61_1Panel.js");
            AddResource(container, "libs/B4/view/report/Report_3_63_3Panel.js");
            AddResource(container, "libs/B4/view/report/Report_5_37_1Panel.js");
            AddResource(container, "libs/B4/view/report/Report_5_37_2Panel.js");
            AddResource(container, "libs/B4/view/report/Report_5_37_3Panel.js");
            AddResource(container, "libs/B4/view/report/Report_6_46_1Panel.js");
            AddResource(container, "libs/B4/view/report/Report_MKDPanel.js");
            AddResource(container, "libs/B4/view/report/Report_SZ_ChargePanel.js");
            AddResource(container, "libs/B4/view/report/Report_SZ_Charge_MKDPanel.js");
            AddResource(container, "libs/B4/view/report/Report_SZ_CollectionPanel.js");
            AddResource(container, "libs/B4/view/report/Report_SZ_Collection_ServicePanel.js");
            AddResource(container, "libs/B4/view/report/Report_SZ_IndicatorPanel.js");
            AddResource(container, "libs/B4/view/report/Report_SZ_SecurityPanel.js");
        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Gis.Reports.UI.dll/Bars.Gkh.Gis.Reports.UI.{0}", path.Replace("/", ".")));
        }
    }
}
