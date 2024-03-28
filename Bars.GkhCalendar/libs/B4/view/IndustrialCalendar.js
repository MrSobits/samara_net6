Ext.define('B4.view.IndustrialCalendar', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.industrialcalendar',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    title: 'Календарь',
    bodyStyle: Gkh.bodyStyle,
    closable: true,

    requires: [
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    padding: 5,
                    layout: {
                        type: 'hbox',
                        align: 'center'
                    },
                    items: [
                        {
                            xtype: 'button',
                            itemId: 'prevBtn',
                            text: '<',
                            width: 45
                        },
                        {
                            xtype: 'panel',
                            layout: 'card',
                            activeItem: 0,
                            flex: 1,
                            border: false,
                            itemId: 'headerPanel',
                            items: [
                                {
                                    xtype: 'panel',
                                    bodyStyle: Gkh.bodyStyle,
                                    itemId: 'datePanel',
                                    layout: {
                                        type: 'hbox',
                                        align: 'center',
                                        pack: 'center'
                                    },
                                    border: false,
                                    defaults: {
                                        margin: '0 3'
                                    },
                                    items: [
                                        {
                                            xtype: 'label',
                                            itemId: 'monthLabel',
                                            style: {
                                                fontWeight: 'bold',
                                                fontSize: '20px'
                                            }
                                        },
                                        {
                                            xtype: 'label',
                                            itemId: 'yearLabel',
                                            style: {
                                                fontWeight: 'bold',
                                                fontSize: '20px'
                                            }
                                        },
                                        {
                                            xtype: 'button',
                                            iconCls: 'icon-pencil',
                                            action: 'SelectDate'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'panel',
                                    itemId: 'changeDatePanel',
                                    layout: {
                                        type: 'hbox',
                                        align: 'center',
                                        pack: 'center'
                                    },
                                    bodyStyle: Gkh.bodyStyle,
                                    border: false,
                                    defaults: {
                                        margin: '2 3 1 3'
                                    },
                                    items: [
                                        {
                                            xtype: 'combo',
                                            fieldLabel: 'Месяц',
                                            displayField: 'name',
                                            valueField: 'number',
                                            store: 'B4.store.Month',
                                            queryMode: 'local',
                                            labelWidth: 40,
                                            itemId: 'monthField',
                                            editable: false
                                        },
                                        {
                                            xtype: 'numberfield',
                                            fieldLabel: 'Год',
                                            minValue: 1900,
                                            maxValue: 2100,
                                            labelWidth: 25,
                                            itemId: 'yearField',
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'button',
                                            iconCls: 'icon-accept',
                                            action: 'AcceptDate'
                                        },
                                        {
                                            xtype: 'button',
                                            iconCls: 'icon-cancel',
                                            action: 'CancelDateChange'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'button',
                            itemId: 'nextBtn',
                            text: '>',
                            width: 45
                        }
                    ]
                },
                {
                    border: false,
                    padding: 5,
                    bodyStyle: Gkh.bodyStyle,
                    layout: {
                        type: 'hbox',
                        align: 'center',
                        pack: 'center'
                    },
                    defaults: {
                        flex: 1,
                        xtype: 'label',
                        style: {
                            fontWeight: 'bold',
                            fontSize: '15px',
                            textAlign: 'center'
                        }
                    },
                    items: [
                        { text: 'Пн' },
                        { text: 'Вт' },
                        { text: 'Ср' },
                        { text: 'Чт' },
                        { text: 'Пт' },
                        { text: 'Сб' },
                        { text: 'Вс' }
                    ]
                },
                {
                    xtype: 'panel',
                    flex: 1,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    itemId: 'vboxpanel',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: []
                }
            ]
        });

        me.callParent(arguments);
    }
});