Ext.define('B4.model.controldate.MunicipalityLimitDate', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlDateMunicipalityLimitDate'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ControlDate' },
        { name: 'Municipality' },
        { name: 'LimitDate' }
    ]
});