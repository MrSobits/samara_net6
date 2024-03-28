/**
* тристор выбора конструктивных элементов
*/
Ext.define('B4.store.realityobj.StructuralElementTree', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.realityobj.StructuralElementTree'],
    model: 'B4.model.realityobj.StructuralElementTree',
    autoLoad: false,
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: B4.Url.action('/CommonEstateObject/ListTree'),
                extraParams: {
                    realityObjectId: cfg.realityObjectId
                },
                reader: {
                    type: 'json'
                },
                root: {
                    text: 'root',
                    expanded: true
                }
            }
        }, cfg)]);
    }
});