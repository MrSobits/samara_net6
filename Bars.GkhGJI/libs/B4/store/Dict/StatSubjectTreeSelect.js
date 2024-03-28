/**
* тристор тематик/подтематик/характеристик обращения
*/
Ext.define('B4.store.dict.StatSubjectTreeSelect', {
    extend: 'Ext.data.TreeStore',
    constructor: function(cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: false,
            storeId: 'StatSubjectTreeStore',
            proxy: {
                type: 'ajax',
                url: B4.Url.action('/StatSubjectGji/ListTree'),
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