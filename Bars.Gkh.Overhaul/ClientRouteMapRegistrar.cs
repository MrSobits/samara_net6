namespace Bars.Gkh.Overhaul
{
    using B4;

    public class ClientRouteMapRegistrar : IClientRouteMapRegistrar
    {
        public void RegisterRoutes(ClientRouteMap map)
        {
            map.AddRoute(new ClientRoute("workprice", "B4.controller.dict.WorkPrice", requiredPermission: "Gkh.Dictionaries.WorkPrice.View"));
            map.AddRoute(new ClientRoute("job", "B4.controller.dict.Job", requiredPermission: "Ovrhl.Dictionaries.Job.View"));
            map.AddRoute(new ClientRoute("basisoverhauldockind", "B4.controller.dict.BasisOverhaulDocKind", requiredPermission: "Ovrhl.Dictionaries.BasisOverhaulDocKind.View"));
            map.AddRoute(new ClientRoute("grouptype", "B4.controller.dict.GroupType", requiredPermission: "Ovrhl.Dictionaries.GroupType.View"));
            map.AddRoute(new ClientRoute("commonestateobject", "B4.controller.CommonEstateObject", requiredPermission: "Ovrhl.Dictionaries.CommonEstateObject.View"));
            map.AddRoute(new ClientRoute("commonestateobj_create", "B4.controller.CommonEstateObject", "create", requiredPermission: "Ovrhl.Dictionaries.CommonEstateObject.Create"));
            map.AddRoute(new ClientRoute("commonestateobj_edit/{id}", "B4.controller.CommonEstateObject", "edit", requiredPermission: "Ovrhl.Dictionaries.CommonEstateObject.Edit"));
            map.AddRoute(new ClientRoute("realestatetyperate", "B4.controller.RealEstateTypeRate", requiredPermission: "Ovrhl.RealEstateTypeRate.View"));

            map.AddRoute(new ClientRoute("paymentsizecr", "B4.controller.dict.PaymentSizeCr", requiredPermission: "Ovrhl.Dictionaries.PaymentSizeCr.View"));
            map.AddRoute(new ClientRoute("commonrealityobjimport", "B4.controller.import.CommonRealityObjectImport", requiredPermission: "Import.CommonRealtyObjectImport.View"));

            map.AddRoute(new ClientRoute("paysize", "B4.controller.Paysize"));
            map.AddRoute(new ClientRoute("paysize_edit/{id}", "B4.controller.paysize.Edit"));
            map.AddRoute(new ClientRoute("overhaultogasu", "B4.controller.export.OverhaulToGasu", requiredPermission: "Administration.ExportOverhaulToGasu.View"));
        }
    }
}