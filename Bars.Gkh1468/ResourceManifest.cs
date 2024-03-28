namespace Bars.Gkh1468
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/CryptoPro.js");
            AddResource(container, "libs/B4/aspects/GkhDigitalSignatureGridAspect.js");
            AddResource(container, "libs/B4/aspects/permission/OkiPassport.js");
            AddResource(container, "libs/B4/aspects/permission/PublicServicesOrg.js");
            AddResource(container, "libs/B4/aspects/permission/administration/Profile.js");
            AddResource(container, "libs/B4/controller/ImportFrom1468.js");
            AddResource(container, "libs/B4/controller/Passport.js");
            AddResource(container, "libs/B4/controller/PassportStruct.js");
            AddResource(container, "libs/B4/controller/PublicServOrg.js");
            AddResource(container, "libs/B4/controller/dict/Municipality.js");
            AddResource(container, "libs/B4/controller/dict/PublicService.js");
            AddResource(container, "libs/B4/controller/dict/TypeCustomer.js");
            AddResource(container, "libs/B4/controller/passport/House.js");
            AddResource(container, "libs/B4/controller/passport/Oki.js");
            AddResource(container, "libs/B4/controller/providerpassport/MyHouse.js");
            AddResource(container, "libs/B4/controller/providerpassport/MyOki.js");
            AddResource(container, "libs/B4/controller/publicservorg/Contract.js");
            AddResource(container, "libs/B4/controller/publicservorg/Edit.js");
            AddResource(container, "libs/B4/controller/publicservorg/Municipality.js");
            AddResource(container, "libs/B4/controller/publicservorg/Navigation.js");
            AddResource(container, "libs/B4/controller/publicservorg/RealtyObject.js");
            AddResource(container, "libs/B4/controller/realityobj/PublicServOrg.js");
            AddResource(container, "libs/B4/dynamic/Helper.js");
            AddResource(container, "libs/B4/model/DataFiller.js");
            AddResource(container, "libs/B4/model/PublicServiceOrg.js");
            AddResource(container, "libs/B4/model/dict/PublicService.js");
            AddResource(container, "libs/B4/model/dict/TypeCustomer.js");
            AddResource(container, "libs/B4/model/dynamic/AttributeValue.js");
            AddResource(container, "libs/B4/model/passport/Attribute.js");
            AddResource(container, "libs/B4/model/passport/House.js");
            AddResource(container, "libs/B4/model/passport/HouseCombined.js");
            AddResource(container, "libs/B4/model/passport/HouseProviderPassport.js");
            AddResource(container, "libs/B4/model/passport/Oki.js");
            AddResource(container, "libs/B4/model/passport/OkiCombined.js");
            AddResource(container, "libs/B4/model/passport/OkiMunicipality.js");
            AddResource(container, "libs/B4/model/passport/OkiProviderPassport.js");
            AddResource(container, "libs/B4/model/passport/Part.js");
            AddResource(container, "libs/B4/model/passport/PassportStruct.js");
            AddResource(container, "libs/B4/model/providerpassport/MyHouse.js");
            AddResource(container, "libs/B4/model/providerpassport/MyOki.js");
            AddResource(container, "libs/B4/model/publicservorg/Contract.js");
            AddResource(container, "libs/B4/model/publicservorg/ContractTempGraph.js");
            AddResource(container, "libs/B4/model/publicservorg/Municipality.js");
            AddResource(container, "libs/B4/model/publicservorg/PublicOrgServiceQualityLevel.js");
            AddResource(container, "libs/B4/model/publicservorg/PublicServiceOrgContractRealObj.js");
            AddResource(container, "libs/B4/model/publicservorg/RealObjPublicServiceOrgService.js");
            AddResource(container, "libs/B4/model/publicservorg/RealtyObject.js");
            AddResource(container, "libs/B4/model/publicservorg/contractpart/BudgetOrgContract.js");
            AddResource(container, "libs/B4/model/publicservorg/contractpart/FuelEnergyResourceContract.js");
            AddResource(container, "libs/B4/model/publicservorg/contractpart/IndividualOwnerContract.js");
            AddResource(container, "libs/B4/model/publicservorg/contractpart/JurPersonOwnerContract.js");
            AddResource(container, "libs/B4/model/publicservorg/contractpart/OfferContract.js");
            AddResource(container, "libs/B4/model/publicservorg/contractpart/RsoAndServicePerformerContract.js");
            AddResource(container, "libs/B4/model/realityobj/PublicServiceOrg.js");
            AddResource(container, "libs/B4/store/ActiveOperator.js");
            AddResource(container, "libs/B4/store/DataFiller.js");
            AddResource(container, "libs/B4/store/DataFillerAll.js");
            AddResource(container, "libs/B4/store/Maplet.js");
            AddResource(container, "libs/B4/store/MapletPass.js");
            AddResource(container, "libs/B4/store/PeriodYear.js");
            AddResource(container, "libs/B4/store/PublicServiceOrg.js");
            AddResource(container, "libs/B4/store/administration/Operator.js");
            AddResource(container, "libs/B4/store/administration/operator/Municipality.js");
            AddResource(container, "libs/B4/store/desktop/ActiveOperator.js");
            AddResource(container, "libs/B4/store/dict/Municipality.js");
            AddResource(container, "libs/B4/store/dict/MunicipalityForSelect.js");
            AddResource(container, "libs/B4/store/dict/MunicipalityForSelected.js");
            AddResource(container, "libs/B4/store/dict/PublicService.js");
            AddResource(container, "libs/B4/store/dict/TypeCustomer.js");
            AddResource(container, "libs/B4/store/dynamic/AttributeValue.js");
            AddResource(container, "libs/B4/store/passport/AttrTreeStore.js");
            AddResource(container, "libs/B4/store/passport/House.js");
            AddResource(container, "libs/B4/store/passport/HouseCombined.js");
            AddResource(container, "libs/B4/store/passport/HouseProviderPassport.js");
            AddResource(container, "libs/B4/store/passport/Oki.js");
            AddResource(container, "libs/B4/store/passport/OkiCombined.js");
            AddResource(container, "libs/B4/store/passport/OkiMunicipality.js");
            AddResource(container, "libs/B4/store/passport/OkiProviderPassport.js");
            AddResource(container, "libs/B4/store/passport/PartTreeStore.js");
            AddResource(container, "libs/B4/store/passport/PassportStruct.js");
            AddResource(container, "libs/B4/store/providerpassport/MyHouse.js");
            AddResource(container, "libs/B4/store/providerpassport/MyOki.js");
            AddResource(container, "libs/B4/store/PublicResOrg/NavigationMenu.js");
            AddResource(container, "libs/B4/store/publicservorg/ByPublicServOrg.js");
            AddResource(container, "libs/B4/store/publicservorg/Contract.js");
            AddResource(container, "libs/B4/store/publicservorg/ContractTempGraph.js");
            AddResource(container, "libs/B4/store/publicservorg/Municipality.js");
            AddResource(container, "libs/B4/store/publicservorg/NavigationMenu.js");
            AddResource(container, "libs/B4/store/publicservorg/PublicOrgServiceQualityLevel.js");
            AddResource(container, "libs/B4/store/publicservorg/RealityObjectInContract.js");
            AddResource(container, "libs/B4/store/publicservorg/RealObjPublicServiceOrgService.js");
            AddResource(container, "libs/B4/store/publicservorg/RealtyObject.js");
            AddResource(container, "libs/B4/store/publicservorg/RealtyObjForSelect.js");
            AddResource(container, "libs/B4/store/publicservorg/RealtyObjForSelected.js");
            AddResource(container, "libs/B4/store/publicservorg/contractpart/IndividualOwnerContract.js");
            AddResource(container, "libs/B4/store/publicservorg/contractpart/JurPersonOwnerContract.js");
            AddResource(container, "libs/B4/store/publicservorg/contractpart/OfferContract.js");
            AddResource(container, "libs/B4/store/publicservorg/contractpart/RsoAndServicePerformerContract.js");
            AddResource(container, "libs/B4/store/realityobj/ForPassport.js");
            AddResource(container, "libs/B4/store/realityobj/PublicServiceOrg.js");
            AddResource(container, "libs/B4/store/View/ViewRealityObject.js");
            AddResource(container, "libs/B4/view/button/Sign.js");
            AddResource(container, "libs/B4/view/Control/GkhButtonPrint.js");
            AddResource(container, "libs/B4/view/Control/GkhCustomColumn.js");
            AddResource(container, "libs/B4/view/Control/GkhWorkModeGrid.js");
            AddResource(container, "libs/B4/view/Control/NavigationPanel.js");
            AddResource(container, "libs/B4/view/Control/YMap.js");
            AddResource(container, "libs/B4/view/desktop/portlet/Map.js");
            AddResource(container, "libs/B4/view/dict/Municipality/EditWindow.js");
            AddResource(container, "libs/B4/view/dict/Municipality/Grid.js");
            AddResource(container, "libs/B4/view/dict/publicservice/Grid.js");
            AddResource(container, "libs/B4/view/dict/typecustomer/Grid.js");
            AddResource(container, "libs/B4/view/importfrom1468rf/Form.js");
            AddResource(container, "libs/B4/view/passport/AttributeEditor.js");
            AddResource(container, "libs/B4/view/passport/AttributeTreeGrid.js");
            AddResource(container, "libs/B4/view/passport/CopyPassportWindow.js");
            AddResource(container, "libs/B4/view/passport/Editor.js");
            AddResource(container, "libs/B4/view/passport/NotCreatedGrid.js");
            AddResource(container, "libs/B4/view/passport/PartTreeGrid.js");
            AddResource(container, "libs/B4/view/passport/StructEditor.js");
            AddResource(container, "libs/B4/view/passport/StructGrid.js");
            AddResource(container, "libs/B4/view/passport/StructImportWindow.js");
            AddResource(container, "libs/B4/view/passport/house/Grid.js");
            AddResource(container, "libs/B4/view/passport/house/Panel.js");
            AddResource(container, "libs/B4/view/passport/house/info/CombinedPassportGrid.js");
            AddResource(container, "libs/B4/view/passport/house/info/Grid.js");
            AddResource(container, "libs/B4/view/passport/house/info/PassportPanel.js");
            AddResource(container, "libs/B4/view/passport/oki/Grid.js");
            AddResource(container, "libs/B4/view/passport/oki/Panel.js");
            AddResource(container, "libs/B4/view/passport/oki/info/CombinedPassportGrid.js");
            AddResource(container, "libs/B4/view/passport/oki/info/Grid.js");
            AddResource(container, "libs/B4/view/passport/oki/info/PassportPanel.js");
            AddResource(container, "libs/B4/view/passport/struct/BaseAttribute.js");
            AddResource(container, "libs/B4/view/passport/struct/GroupedAttribute.js");
            AddResource(container, "libs/B4/view/passport/struct/GroupedComplexAttribute.js");
            AddResource(container, "libs/B4/view/passport/struct/GroupedWithValueAttribute.js");
            AddResource(container, "libs/B4/view/passport/struct/SimpleAttribute.js");
            AddResource(container, "libs/B4/view/providerpassport/myhouse/EditWindow.js");
            AddResource(container, "libs/B4/view/providerpassport/myhouse/Grid.js");
            AddResource(container, "libs/B4/view/providerpassport/myoki/EditWindow.js");
            AddResource(container, "libs/B4/view/providerpassport/myoki/Grid.js");
            AddResource(container, "libs/B4/view/publicservorg/AddWindow.js");
            AddResource(container, "libs/B4/view/publicservorg/ContractEditWindow.js");
            AddResource(container, "libs/B4/view/publicservorg/ContractGrid.js");
            AddResource(container, "libs/B4/view/publicservorg/ContractMainInfo.js");
            AddResource(container, "libs/B4/view/publicservorg/ContractServiceEditWindow.js");
            AddResource(container, "libs/B4/view/publicservorg/ContractServiceGrid.js");
            AddResource(container, "libs/B4/view/publicservorg/ContractServiceMainInfo.js");
            AddResource(container, "libs/B4/view/publicservorg/ContractServicePlanVolume.js");
            AddResource(container, "libs/B4/view/publicservorg/ContractStop.js");
            AddResource(container, "libs/B4/view/publicservorg/ContractTempGraph.js");
            AddResource(container, "libs/B4/view/publicservorg/ContractTempGraphGrid.js");
            AddResource(container, "libs/B4/view/publicservorg/ContractTimingInformation.js");
            AddResource(container, "libs/B4/view/publicservorg/EditPanel.js");
            AddResource(container, "libs/B4/view/publicservorg/Grid.js");
            AddResource(container, "libs/B4/view/publicservorg/MunicipalityGrid.js");
            AddResource(container, "libs/B4/view/publicservorg/NavigationPanel.js");
            AddResource(container, "libs/B4/view/publicservorg/RealtyObjectGrid.js");
            AddResource(container, "libs/B4/view/publicservorg/RealtyObjectInContractGrid.js");
            AddResource(container, "libs/B4/view/publicservorg/contractpart/MainPanel.js");
            AddResource(container, "libs/B4/view/publicservorg/contractqualitylevel/EditWindow.js");
            AddResource(container, "libs/B4/view/publicservorg/contractqualitylevel/Grid.js");
            AddResource(container, "libs/B4/view/publicservorg/contractqualitylevel/Panel.js");
            AddResource(container, "libs/B4/view/realityobj/PublicServiceOrgGrid.js");
            AddResource(container, "libs/jQuery/jquery-1.9.1.js");
            AddResource(container, "libs/jQuery/jquery-1.9.1.min.js");
            AddResource(container, "libs/jQuery/jquery-1.9.1.min.js.map");

            AddResource(container, "resources/Report.mrt");

            AddResource(container, "content/css/gkh1468.css");
            AddResource(container, "content/img/housePassport.png");
            AddResource(container, "content/img/housePassportProv.png");
            AddResource(container, "content/img/okiPaspport.png");
            AddResource(container, "content/img/okiPassportProv.png");
            AddResource(container, "content/img/onepasptransfer.png");
            AddResource(container, "content/img/publicServOrg.png");

            AddResource(container, "Views/shared/B4.1ExtJs.Layout.cshtml");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh1468.dll/Bars.Gkh1468.{0}", path.Replace("/", ".")));
        }
    }
}
