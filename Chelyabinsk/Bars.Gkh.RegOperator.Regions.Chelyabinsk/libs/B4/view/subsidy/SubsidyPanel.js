Ext.define('B4.view.subsidy.SubsidyPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.subsidypanel',

    requires: [
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.subsidy.SubsidyRecordGrid',
        'B4.store.version.ProgramVersion'
    ],

    minWidth: 750,
    width: 750,
    autoScroll: true,
    bodyPadding: 5,
    closable: false,
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    title: 'Субсидирование',
    trackResetOnLoad: true,
    frame: true,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            flex: 1,
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    items: [
                                        {
                                            xtype: 'b4savebutton'
                                        },
                                        {
                                            xtype: 'tbseparator'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Рассчитать собираемость',
                                            action: 'CalcOwnerCollection',
                                            iconCls: 'icon-table-go',
                                            hidden: true
                                        },
                                        {
                                            xtype: 'tbseparator'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Рассчитать баланс',
                                            action: 'CalcBalance',
                                            iconCls: 'icon-table-go'
                                        },
                                        {
                                            xtype: 'tbseparator'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Рассчитать показатели',
                                            action: 'CalcValues',
                                            iconCls: 'icon-table-go',
                                            hidden: true
                                        },
                                        {
                                            xtype: 'tbseparator'
                                        },
                                        {
                                            xtype: 'tbseparator'
                                        },
                                        {
                                            xtype: 'button',
                                            iconCls: 'icon-table-go',
                                            text: 'Экспорт',
                                            textAlign: 'left',
                                            action: 'Export'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    flex:1,
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'component',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'label',
                                            name: 'DateCalcOwner',
                                            width: 250,
                                            padding: '5 0 0 0',
                                            text: ''
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
                {
                    region:'center',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'subsidyrecordgrid',
                            flex: 1,
                            border: 0
                        }
                    ]
                }
            ]
        });
        
        me.callParent(arguments);
    }
});