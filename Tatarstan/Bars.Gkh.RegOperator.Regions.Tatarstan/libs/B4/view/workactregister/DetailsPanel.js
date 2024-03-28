Ext.define('B4.view.workactregister.DetailsPanel', {
    extend: 'B4.form.Window',
    alias: 'widget.workactregisterdetailspanel',

    title: 'Сводные данные по принятым актам выполненных работ',
    closable: true,
    bodyStyle: Gkh.bodyStyle,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 900,
    height: 600,
    bodyPadding: 5,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.grid.feature.Summary'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.workactregister.Details');

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox'
                    },
                    defaults: {
                        labelAlign: 'left',
                        labelWidth: 175
                    },
                    items: [
                        {
                            xtype: 'displayfield',
                            fieldLabel: 'Программа КР',
                            name: 'Program'
                        },
                        {
                            xtype: 'displayfield',
                            fieldLabel: 'Муниципальное образование',
                            name: 'Municipality'
                        },
                        {
                            xtype: 'displayfield',
                            fieldLabel: 'Жилой дом',
                            name: 'Address'
                        }
                    ]
                },
                {
                    xtype: 'gridpanel',
                    cls: 'x-large-head',
                    store: store,
                    flex: 1,
                    plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
                    features: [{
                        ftype: 'b4_summary'
                    }],
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'WorkName',
                            text: 'Вид работы',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'PlanVolume',
                            text: 'Плановый объем',
                            align: 'right',
                            summaryType: 'sum',
                            renderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : 0;
                            },
                            summaryRenderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : 0;
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'PlanSum',
                            text: 'Плановая сумма',
                            align: 'right',
                            renderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : 0;
                            },
                            summaryType: 'sum',
                            summaryRenderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : 0;
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ActVolume',
                            text: 'Объем по принятым актам',
                            align: 'right',
                            summaryType: 'sum',
                            renderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : 0;
                            },
                            summaryRenderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : 0;
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ActSum',
                            text: 'Сумма по принятым актам',
                            align: 'right',
                            renderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : 0;
                            },
                            summaryType: 'sum',
                            summaryRenderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : 0;
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'CompleteVolumePercent',
                            text: 'Процент выполненного объема',
                            align: 'right',
                            renderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : 0;
                            },
                            summaryType: 'sum',
                            summaryRenderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : 0;
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'UsedResourcesPercent',
                            text: 'Процент использованных средств',
                            align: 'right',
                            renderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : 0;
                            },
                            summaryType: 'sum',
                            summaryRenderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : 0;
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'TypeWorkLimit',
                            text: 'Лимит по виду работы',
                            align: 'right',
                            renderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : 0;
                            },
                            summaryType: 'sum',
                            summaryRenderer: function (val) {
                                return val ? Ext.util.Format.currency(val) : 0;
                            }
                        }
                    ],
                    dockedItems: [
                        {
                            xtype: 'b4pagingtoolbar',
                            displayInfo: true,
                            store: store,
                            dock: 'bottom'
                        }
                    ],
                    viewConfig: {
                        loadMask: true
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    text: 'Закрыть',
                                    listeners: {
                                        click: function () {
                                            this.up('workactregisterdetailspanel').close();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});