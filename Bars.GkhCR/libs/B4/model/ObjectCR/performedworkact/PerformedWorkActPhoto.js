Ext.define('B4.model.objectcr.performedworkact.PerformedWorkActPhoto', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PerformedWorkActPhoto'
    },
    fields: [
        { name: 'Id' },
        { name: 'PerformedWorkAct' },
        { name: 'Photo' },
        { name: 'Name' },
        { name: 'Discription' },
        { name: 'PhotoType' }
    ]
});