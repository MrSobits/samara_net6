Ext.define('B4.model.constructionobject.Smr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'constructobjmonitoringsmr',
        timeout: 60000
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ConstructionObject', defaultValue: null },
        { name: 'State', defaultValue: null }
    ]
});