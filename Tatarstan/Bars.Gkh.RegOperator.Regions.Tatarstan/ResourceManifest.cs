namespace Bars.Gkh.RegOperator.Regions.Tatarstan
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/aspects/permission/ConfirmContribution.js");
            AddResource(container, "libs/B4/aspects/regop/PersAccImportAspect.js");
            AddResource(container, "libs/B4/controller/ConfirmContribution.js");
            AddResource(container, "libs/B4/controller/TransferFunds.js");
            AddResource(container, "libs/B4/controller/WorkActRegister.js");
            AddResource(container, "libs/B4/controller/objectcr/PerformedWorkActRo.js");
            AddResource(container, "libs/B4/controller/regop/personal_account/Details.js");
            AddResource(container, "libs/B4/model/ConfirmContribution.js");
            AddResource(container, "libs/B4/model/confirmcontribution/ManagOrg.js");
            AddResource(container, "libs/B4/model/confirmcontribution/RealityObject.js");
            AddResource(container, "libs/B4/model/confirmcontribution/Record.js");
            AddResource(container, "libs/B4/model/regop/personal_account/ApartmentNumber.js");
            AddResource(container, "libs/B4/model/regop/personal_account/BasePersonalAccount.js");
            AddResource(container, "libs/B4/model/transferfunds/Hire.js");
            AddResource(container, "libs/B4/model/workactregister/Details.js");
            AddResource(container, "libs/B4/store/ConfirmContribution.js");
            AddResource(container, "libs/B4/store/confirmcontribution/ManagOrg.js");
            AddResource(container, "libs/B4/store/confirmcontribution/RealityObject.js");
            AddResource(container, "libs/B4/store/confirmcontribution/Record.js");
            AddResource(container, "libs/B4/store/regop/personal_account/ApartmentNumber.js");
            AddResource(container, "libs/B4/store/transferfunds/Hire.js");
            AddResource(container, "libs/B4/store/workactregister/Details.js");
            AddResource(container, "libs/B4/view/confirmcontribution/EditWindow.js");
            AddResource(container, "libs/B4/view/confirmcontribution/Grid.js");
            AddResource(container, "libs/B4/view/confirmcontribution/RecordEditWindow.js");
            AddResource(container, "libs/B4/view/confirmcontribution/RecordGrid.js");
            AddResource(container, "libs/B4/view/import/PersAccImportWindow.js");
            AddResource(container, "libs/B4/view/objectcr/performedworkact/Grid.js");
            AddResource(container, "libs/B4/view/regop/personal_account/ChangevalbtnRoomAddress.js");
            AddResource(container, "libs/B4/view/regop/personal_account/PersonalAccountEditPanel.js");
            AddResource(container, "libs/B4/view/transferfunds/HireGrid.js");
            AddResource(container, "libs/B4/view/transferfunds/RecordEditWindow.js");
            AddResource(container, "libs/B4/view/workactregister/DetailsPanel.js");
            AddResource(container, "libs/B4/view/workactregister/Grid.js");

        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.Gkh.RegOperator.Regions.Tatarstan.dll/Bars.Gkh.RegOperator.Regions.Tatarstan.{0}", path.Replace("/", ".")));
        }
    }
}
