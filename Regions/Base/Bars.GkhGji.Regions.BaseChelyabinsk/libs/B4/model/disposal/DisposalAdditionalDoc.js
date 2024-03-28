Ext.define('B4.model.disposal.DisposalAdditionalDoc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalAdditionalDoc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DocumentName'},
        { name: 'Disposal'}
    ]
});