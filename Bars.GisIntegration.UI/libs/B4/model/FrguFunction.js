Ext.define('B4.model.FrguFunction', {
        extend: 'B4.base.Model',
        proxy: {
            type: 'b4proxy',
            controllerName: 'FrguFunction'
        },
        fields: [
            { name: 'Id', useNull: true },
            { name: 'Name' },
            { name: 'FrguId' },
            { name: 'Guid' }
        ]
});