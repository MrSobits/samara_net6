Ext.define('B4.model.actcheck.DocRequestActionRequestInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DocRequestActionRequestInfo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DocRequestAction' },
        { name: 'RequestInfoType' },
        { name: 'Name' },
        { name: 'File' }
    ]
});