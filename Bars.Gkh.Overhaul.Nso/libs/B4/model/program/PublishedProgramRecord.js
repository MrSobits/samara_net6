Ext.define('B4.model.program.PublishedProgramRecord', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'PublishedProgramRecord'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality' },
        { name: 'RealityObject' },
        { name: 'CommonEstateobject' },
        { name: 'IndexNumber' },
        { name: 'Sum' },
        { name: 'PublishedYear' }
    ]
});