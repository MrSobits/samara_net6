Ext.define('B4.model.appealcits.AppealCitsExecutant', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsExecutant'
    },
    fields: [
        { name: 'AppealCits', defaultValue: null },
        { name: 'Executant', defaultValue: null },
        { name: 'Author', defaultValue: null },
        { name: 'Controller', defaultValue: null },
        { name: 'OrderDate' },
        { name: 'PerformanceDate' },
        { name: 'ExecutantZji' },
        { name: 'IsResponsible', defaultValue: false },
        { name: 'Description' },
        { name: 'State', defaultValue: null },
        { name: 'Resolution', defaultValue: null }
    ]
});