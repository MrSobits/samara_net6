Ext.define('B4.model.actcheck.Annex', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckAnnex'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActCheck', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'File', defaultValue: null },
        { name: 'SendFileToErknm', defaultValue: 10 }
    ]
});