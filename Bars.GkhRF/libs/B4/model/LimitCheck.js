Ext.define('B4.model.LimitCheck', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.TypeProgramRequest'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'LimitCheck'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'TypeProgram', defaultValue: 10 },
        { name: 'FinSources' }
    ]
});