Ext.define('B4.model.finactivity.RealityObjCommunalService', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FinActivityRealityObjCommunalService'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfoRealityObj', defaultValue: null },
        { name: 'TypeServiceDi', defaultValue: 10 },
        { name: 'PaidOwner', defaultValue: null },
        { name: 'DebtOwner', defaultValue: null },
        { name: 'PaidByIndicator', defaultValue: null },
        { name: 'PaidByAccount', defaultValue: null }
    ]
});
