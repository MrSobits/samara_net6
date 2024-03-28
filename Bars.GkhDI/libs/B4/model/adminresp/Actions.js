Ext.define('B4.model.adminresp.Actions', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'Actions'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AdminResp', defaultValue: null },
        { name: 'Action' }
    ]
});