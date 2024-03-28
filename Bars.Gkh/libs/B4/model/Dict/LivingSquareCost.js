Ext.define('B4.model.dict.LivingSquareCost', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'LivingSquareCost'
    },
    fields: [
        { name: 'Id' },
        { name: 'Municipality' },
        { name: 'Cost' },
        { name: 'Year' }
    ]
});