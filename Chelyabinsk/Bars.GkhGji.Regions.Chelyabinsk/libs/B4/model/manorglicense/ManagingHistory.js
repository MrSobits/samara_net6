Ext.define('B4.model.manorglicense.ManagingHistory', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManagingHistory',
        listAction: 'GetListWithRO',
        timeout: 100000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'Address' },
        { name: 'StartDate' },
        { name: 'StartReason' },
        { name: 'EndDate' },
        { name: 'EndReason' },
        { name: 'NewManOrg' }
    ]
});