Ext.define('B4.model.basejurperson.Contragent', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BaseJurPersonContragent'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'InspectionGji', defaultValue: null },
        { name: 'Contragent', defaultValue: null },
        { name: 'ContragentId' },
        { name: 'MunicipalityName' },
        { name: 'JuridicalAddress' },
        { name: 'Name' },
        { name: 'Inn' },
    ]
});