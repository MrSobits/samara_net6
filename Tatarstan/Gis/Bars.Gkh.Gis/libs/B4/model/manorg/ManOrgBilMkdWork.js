Ext.define('B4.model.manorg.ManOrgBilMkdWork', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgBilMkdWork'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'MkdWork', defaultValue: null },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'Description' }
    ]
});