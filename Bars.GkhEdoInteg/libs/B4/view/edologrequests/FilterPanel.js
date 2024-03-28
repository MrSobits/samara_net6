Ext.define('B4.view.edologrequests.FilterPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.requestsFilterPanel',

    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    itemId: 'requestsFilterPanel',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.ux.button.Update'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    width: 650,
                    layout: {
                        pack: 'start',
                        type: 'hbox'
                    },
                    defaults: {
                        anchor: '100%',
                        format: 'd.m.Y',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            labelWidth: 130,
                            fieldLabel: 'Дата запроса с',
                            width: 290,
                            itemId: 'dfDateStart',
                            value: new Date(new Date().getFullYear(), 0, 1)
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 50,
                            fieldLabel: 'по',
                            width: 210,
                            itemId: 'dfDateEnd',
                            value: new Date()
                        },
                        {
                            width: 10,
                            xtype: 'component'
                        },
                        {
                            width: 100,
                            itemId: 'updateGrid',
                            xtype: 'button',
                            text: 'Обновить',
                            tooltip: 'Обновить',
                            iconCls: 'icon-arrow-refresh'
                        }
                            
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});