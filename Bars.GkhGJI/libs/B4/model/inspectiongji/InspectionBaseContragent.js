Ext.define('B4.model.inspectiongji.InspectionBaseContragent', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectionBaseContragent'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'MunicipalityName' },
        { name: 'JuridicalAddress' },
        { name: 'Name' },
        { name: 'Inn' }
    ]
});