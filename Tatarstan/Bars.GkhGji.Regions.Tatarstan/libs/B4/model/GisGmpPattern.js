Ext.define('B4.model.GisGmpPattern', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'GisGmpPattern'
    },
    fields: [
        { name: 'Municipality' },
        { name: 'PatternCode' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});