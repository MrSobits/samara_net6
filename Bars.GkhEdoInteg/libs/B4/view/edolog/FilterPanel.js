Ext.define('B4.view.edolog.FilterPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.appealcitsEdoLogFilterPanel',

    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    itemId: 'appealcitsEdoLogFilterPanel',
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
                            fieldLabel: 'Дата создания с',
                            width: 290,
                            itemId: 'dfDateCreateStart',
                            value: new Date(new Date().getFullYear(), 0, 1)
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 50,
                            fieldLabel: 'по',
                            width: 210,
                            itemId: 'dfDateCreateEnd',
                            value: new Date()
                        }
                    ]
                },
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
                            fieldLabel: 'Дата актуальности с',
                            width: 290,
                            itemId: 'dfDateActualStart',
                            value: new Date(new Date().getFullYear(), 0, 1)
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 50,
                            fieldLabel: 'по',
                            width: 210,
                            itemId: 'dfDateActualEnd',
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