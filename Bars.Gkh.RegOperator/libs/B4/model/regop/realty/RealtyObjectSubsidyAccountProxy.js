Ext.define('B4.model.regop.realty.RealtyObjectSubsidyAccountProxy', {
    extend: 'B4.base.Model',
    fields: [
        { name: 'Id' },
        { name: 'AccountNum' },
        { name: 'DateOpen' },
        { name: 'LastOperationDate' },
        { name: 'PlanOperTotal' },
        { name: 'FactOperTotal' },
        { name: 'CurrentBalance' }
    ],

    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectAccount',
        readAction: 'GetSubsidyAccount'
    }
});