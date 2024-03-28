Ext.define('B4.view.dict.work.FinSourceChecker', {
    extend: 'Ext.form.FieldSet',
    alias: 'widget.finsourcechecker',
    requires: ['B4.enums.TypeFinSource'],

    layout: {
        type: 'vbox',
        align: 'left'
    },

    border: false,

    initComponent: function () {
        var me = this,
            items = [{
                xtype: 'hiddenfield',
                name: 'FinSources'
            }],
            store = B4.enums.TypeFinSource.getStore();

        store.each(function (rec) {
            items.push(
                {
                    xtype: 'checkbox',
                    boxLabel: rec.get('Display'),
                    recValue: rec.get('Value'),
                    listeners: {
                        change: function (ch, newVal) {
                            var companion = ch.nextSibling('hiddenfield[recValue="' + ch.recValue + '"]');
                            if (companion) {
                                if (newVal) {
                                    companion.setValue(ch.recValue);
                                } else {
                                    companion.setValue(null);
                                }
                            }
                        },
                        render: function (ch) {
                            var finSources = (ch.up('finsourcechecker').down('hiddenfield[name="FinSources"]').getValue()).split(',');
                            var companion = ch.nextSibling('hiddenfield[recValue="' + ch.recValue + '"]');
                            if (companion) {
                                if (Ext.Array.contains(finSources, ch.recValue+"")) {
                                    ch.setValue(true);
                                    companion.setValue(ch.recValue);
                                }
                            }
                        }
                    }
                },
                {
                    xtype: 'hiddenfield',
                    name: 'FinSource',
                    recValue: rec.get('Value')
                });
        });

        Ext.apply(me, {
            items: items
        });

        me.callParent(arguments);
    }
});