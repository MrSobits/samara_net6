Ext.define('B4.store.regop.realty.RealtyPlanSubsidyAccountOperation', {
    extend: 'B4.base.Store',
    autoLoad: false,
    fields: ['Id', 'DateString', 'FederalStandardFee', 'Tariff', 'Area', 'Days', 'Sum'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectSubsidyAccount',
        listAction: 'GetPlanSubsidyOperations'
    }
});