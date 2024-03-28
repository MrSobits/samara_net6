Ext.define('B4.store.dict.MunicipalitySelectTree', {
    extend: 'Ext.data.TreeStore',
    requires: ['B4.model.dict.MunicipalityTree'],
    model: 'B4.model.dict.MunicipalityTree',
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: false,
            storeId: 'MunicipalitySelectTree',
            proxy: {
                type: 'ajax',
                url: B4.Url.action('/Municipality/ListSelectTree'),
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
