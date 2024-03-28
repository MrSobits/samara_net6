Ext.define('B4.view.EmsedListPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    storeName: null,
    title: 'Отправка данных в ЕМСЭД',
    alias: 'widget.emsedListPanel',
    layout: {
        type: 'hbox',
        align: 'stretch'
    },

    requires: [
        'B4.view.appealcits.SelectionGrid',
        'B4.view.EmsedGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                xtype: 'panel',
                layout: 'fit',
                border: false
            },
            items: [
                {
                    flex: 1,
                    padding: '0 5 0 0',
                    items: [
                        {
                            xtype: 'appealCitsSelectGrid'
                        }
                    ]
                },
                {
                    flex: 2,
                    items: [
                        {
                            xtype: 'emsedGrid'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});