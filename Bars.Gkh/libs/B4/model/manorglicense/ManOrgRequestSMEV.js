Ext.define('B4.model.manorglicense.ManOrgRequestSMEV', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgRequestSMEV'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'LicRequest' },
        { name: 'ObjectCreateDate' },
        { name: 'Date' },
        { name: 'RequestSMEVType', defaultValue: 0},
        { name: 'SMEVRequestState', defaultValue: 0 },
        { name: 'Inspector' },
        { name: 'RequestId' }
    ]
});