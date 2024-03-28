Ext.define('B4.model.eds.EDSInspection', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'EDSInspection'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent' },
        { name: 'ContragentINN' },
        { name: 'InspectionNumber' },
        { name: 'InspectionDate' },
        { name: 'NotOpened' },
        { name: 'TypeBase' },
        { name: 'Disposals' },
        { name: 'inspectors' },
        { name: 'BaseDoc' },
        { name: 'DocNumber' },
        { name: 'InspectionGji' },
        { name: 'ContragentName' }
    ]
});