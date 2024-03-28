Ext.define('B4.model.mkdlicrequest.HeadInspector', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'MKDLicRequestHeadInspector'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Fio' },
        { name: 'Position' },
        { name: 'Phone' },
        { name: 'Email' }
    ]
});