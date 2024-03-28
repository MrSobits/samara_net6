Ext.define('B4.store.dict.MunicipalityTree', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.dict.MunicipalityTree'],
    model: 'B4.model.dict.MunicipalityTree',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: false,
            storeId: 'municipalityTreeStore',
            proxy: {
                type: 'ajax',
                url: B4.Url.action('/Municipality/ListTree'),
                reader: {
                    type: 'json'
                },
                root: {
                    text: 'root',
                    expanded: false
                }
            }
        }, cfg)]);
    }
});
