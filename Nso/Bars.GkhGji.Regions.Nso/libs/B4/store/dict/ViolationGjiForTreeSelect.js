Ext.define('B4.store.dict.ViolationGjiForTreeSelect', {
    extend: 'Ext.data.TreeStore',
    fields: [
      { name: 'Id', useNull: true },
      { name: 'Name' },
      { name: 'Code' },
      { name: 'ViolationGjiId'},
      { name: 'IsViolation', type: 'boolean' }
    ],
    constructor: function (cfg) {
        var me = this;
        cfg = cfg || {};
        me.callParent([Ext.apply({
            autoLoad: false,
            proxy: {
                type: 'ajax',
                url: B4.Url.action('/FeatureViolGji/GetTreeForViolations'),
                reader: {
                    type: 'json'
                },
                root: {
                    text: 'root',
                    expanded: false
                },
                timeout: 1000000
            },
            listeners: {
                load: function (scope, node, records) {
                    var cascade = function(items) {
                        if (items.length) {
                            Ext.each(items, function (rec, i) {
                                if (rec.get('IsViolation')) {
                                    rec.set('checked', false);
                                    rec.set('leaf', true);
                                }
                                cascade(rec.childNodes);
                            });
                        }
                    };
                    cascade(records);
                }
            }
        }, cfg)]);
    }
});
