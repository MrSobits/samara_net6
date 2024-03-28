Ext.define('B4.model.InfoAboutReductionPayment', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InfoAboutReductionPayment'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BaseService', defaultValue: null },
        { name: 'BaseServiceName' },
        { name: 'TypeGroupServiceDi', defaultValue: 10 },
        { name: 'DisclosureInfoRealityObj', defaultValue: null },
        { name: 'ReasonReduction' },
        { name: 'RecalculationSum', defaultValue: null },
        { name: 'OrderDate', defaultValue: null },
        { name: 'OrderNum' },
        { name: 'Description' }
    ]
});