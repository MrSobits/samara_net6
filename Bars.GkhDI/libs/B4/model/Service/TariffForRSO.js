Ext.define('B4.model.service.TariffForRso', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TariffForRso'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'BaseService', defaultValue: null },
        { name: 'DateStart', defaultValue: null },
        { name: 'DateEnd', defaultValue: null },
        { name: 'NumberNormativeLegalAct' },
        { name: 'DateNormativeLegalAct', defaultValue: null },
        { name: 'OrganizationSetTariff', defaultValue: null },
        { name: 'Cost', defaultValue: null }
    ]
});