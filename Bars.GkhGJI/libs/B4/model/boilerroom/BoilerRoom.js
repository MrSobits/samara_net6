Ext.define('B4.model.boilerroom.BoilerRoom', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BoilerRoom'
    },
    fields: [
        { name: 'Address' },
        { name: 'Municipality' },
        { name: 'IsActive' },
        { name: 'IsRunning' }
    ]
});