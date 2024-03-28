Ext.define('B4.view.realestatetype.RealEstateTypeRateNotLivingAreaGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realestatetyperatenotlivingareagrid',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Save',

        'Ext.grid.plugin.CellEditing',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    title: 'Тариф по типам домов',
    store: 'RealEstateTypeRate',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            columns: [
                {
                    dataIndex: 'RealEstateTypeName',
                    flex: 1,
                    text: 'Тип дома'
                },
                {
                    dataIndex: 'NeedForFunding',
                    flex: 1,
                    text: 'Потребность в финансировании, руб',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    text: 'Площадь помещений, кв.м',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'TotalArea',
                            text: 'Жилая площадь'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'TotalNotLivingArea',
                            text: 'Нежилая площадь'
                        }
                    ]
                },
                {
                    text: 'Экономически обоснованный тариф, руб./кв.м.',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ReasonableRate',
                            text: 'Жилая площадь'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ReasonableRateNotLivingArea',
                            text: 'Нежилая площадь'
                        }
                    ]
                },
                {
                    text: 'Социально допустимый (установочный) тариф, руб./кв.м',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'SociallyAcceptableRate',
                            text: 'Жилая площадь',
                            editor: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                decimalSeparator: ','
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'SociallyAcceptableRateNotLivingArea',
                            text: 'Нежилая площадь',
                            editor: {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                decimalSeparator: ','
                            }
                        }
                    ]
                },
                {
                    text: 'Дефицит тарифа, руб./кв.м.',
                    flex: 1,
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    },
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'RateDeficit',
                            text: 'Жилая площадь'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'RateDeficitNotLivingArea',
                            text: 'Нежилая площадь'
                        }
                    ]
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            features: [{
                ftype: 'grouping',
                groupHeaderTpl: '{name}'
            }],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    itemId: 'recalcButton',
                                    text: 'Рассчитать показатели',
                                    tooltip: 'Рассчитать показатели',
                                    iconCls: 'icon-calculator'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });
        me.callParent(arguments);
    }
});