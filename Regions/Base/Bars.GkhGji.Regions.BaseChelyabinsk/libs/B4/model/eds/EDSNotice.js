Ext.define('B4.model.eds.EDSNotice', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'EDSScript',
        listAction: 'ListEDSNotice'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'SignedFile', defaultValue: null },
        { name: 'DocumentDate' },
        { name: 'Name' },
        { name: 'TypeAnnex', defaultValue: 0 },
    ]
});