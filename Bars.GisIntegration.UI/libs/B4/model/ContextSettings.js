Ext.define('B4.model.ContextSettings',
    {
        extend: 'B4.base.Model',
        proxy: {
                type: 'b4proxy',
                controllerName: 'ContextSettings',
                listAction: 'GetStorableSettings'
            },
        fields: [
                { name: 'Id', useNull: true },
                { name: 'FileStorageName' },
                { name: 'Context' }
            ]
    });