Ext.define('B4.model.disposal.DisposalDocConfirm', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalDocConfirm'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DocumentName'},
        { name: 'Disposal'}
    ]
});