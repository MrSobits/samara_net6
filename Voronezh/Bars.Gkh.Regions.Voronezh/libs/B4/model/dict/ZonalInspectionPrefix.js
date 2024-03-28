Ext.define('B4.model.dict.ZonalInspectionPrefix', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ZonalInspectionPrefix'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActCheckPrefix' },
        { name: 'ProtocolPrefix' },
        { name: 'PrescriptionPrefix' },
        { name: 'ZonalInspection' }
    ]
});