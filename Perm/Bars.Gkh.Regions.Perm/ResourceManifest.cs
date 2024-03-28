namespace Bars.Gkh.Regions.Perm
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            this.AddResource(container, "libs/B4/controller/manorglicense/EditLicense.js");
            this.AddResource(container, "libs/B4/controller/manorglicense/EditRequest.js");
            this.AddResource(container, "libs/B4/controller/manorglicense/License.js");
            this.AddResource(container, "libs/B4/controller/manorglicense/Navi.js");
            this.AddResource(container, "libs/B4/controller/manorglicense/Request.js");
            this.AddResource(container, "libs/B4/controller/manorglicense/RequestList.js");
            this.AddResource(container, "libs/B4/controller/report/UnderstandingHomeReport.js");
            this.AddResource(container, "libs/B4/form/FiasSelectAddress.js");
            this.AddResource(container, "libs/B4/form/FiasSelectAddressWindow.js");
            this.AddResource(container, "libs/B4/model/manorglicense/License.js");
            this.AddResource(container, "libs/B4/model/manorglicense/LicenseDoc.js");
            this.AddResource(container, "libs/B4/model/manorglicense/LicensePerson.js");
            this.AddResource(container, "libs/B4/model/manorglicense/Request.js");
            this.AddResource(container, "libs/B4/model/manorglicense/RequestAnnex.js");
            this.AddResource(container, "libs/B4/model/manorglicense/RequestPerson.js");
            this.AddResource(container, "libs/B4/model/manorglicense/RequestProvDoc.js");
            this.AddResource(container, "libs/B4/store/manorglicense/License.js");
            this.AddResource(container, "libs/B4/store/manorglicense/LicenseDoc.js");
            this.AddResource(container, "libs/B4/store/manorglicense/LicensePerson.js");
            this.AddResource(container, "libs/B4/store/manorglicense/LicensePersonByContragent.js");
            this.AddResource(container, "libs/B4/store/manorglicense/ListManOrg.js");
            this.AddResource(container, "libs/B4/store/manorglicense/NavigationMenu.js");
            this.AddResource(container, "libs/B4/store/manorglicense/PersonByContragent.js");
            this.AddResource(container, "libs/B4/store/manorglicense/Request.js");
            this.AddResource(container, "libs/B4/store/manorglicense/RequestAnnex.js");
            this.AddResource(container, "libs/B4/store/manorglicense/RequestPerson.js");
            this.AddResource(container, "libs/B4/store/manorglicense/RequestProvDoc.js");
            this.AddResource(container, "libs/B4/view/emergencyobj/AddWindow.js");
            this.AddResource(container, "libs/B4/view/emergencyobj/EditPanel.js");
            this.AddResource(container, "libs/B4/view/manorglicense/AddWindow.js");
            this.AddResource(container, "libs/B4/view/manorglicense/EditLicensePanel.js");
            this.AddResource(container, "libs/B4/view/manorglicense/EditRequestPanel.js");
            this.AddResource(container, "libs/B4/view/manorglicense/Grid.js");
            this.AddResource(container, "libs/B4/view/manorglicense/LicenseDocEditWindow.js");
            this.AddResource(container, "libs/B4/view/manorglicense/LicenseDocGrid.js");
            this.AddResource(container, "libs/B4/view/manorglicense/LicenseGrid.js");
            this.AddResource(container, "libs/B4/view/manorglicense/LicensePersonGrid.js");
            this.AddResource(container, "libs/B4/view/manorglicense/NavigationPanel.js");
            this.AddResource(container, "libs/B4/view/manorglicense/PersonGrid.js");
            this.AddResource(container, "libs/B4/view/manorglicense/ProvDocGrid.js");
            this.AddResource(container, "libs/B4/view/manorglicense/RequestAnnexEditWindow.js");
            this.AddResource(container, "libs/B4/view/manorglicense/RequestAnnexGrid.js");
            this.AddResource(container, "libs/B4/view/manorglicense/RequestInspectionGrid.js");
            this.AddResource(container, "libs/B4/view/manorglicense/RequestListGrid.js");
            this.AddResource(container, "libs/B4/view/manorglicense/RequestListPanel.js");
            this.AddResource(container, "libs/B4/view/manorglicense/RequestProvDocEditWindow.js");
            this.AddResource(container, "libs/B4/view/person/QualificationGrid.js");
            this.AddResource(container, "libs/B4/view/person/RequestToExamEditWindow.js");
            this.AddResource(container, "libs/B4/view/realityobj/Grid.js");
            this.AddResource(container, "libs/B4/view/report/UnderstandingHomeReportPanel.js");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.Gkh.Regions.Perm.dll/Bars.Gkh.Regions.Perm.{0}", path.Replace("/", ".")));
        }
    }
}
