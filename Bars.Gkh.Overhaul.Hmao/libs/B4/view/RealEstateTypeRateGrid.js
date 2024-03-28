Ext.define('B4.view.RealEstateTypeRateGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realestatetyperategrid',

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
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'TotalArea',
                    flex: 1,
                    text: 'Площадь помещений, кв.м',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'ReasonableRate',
                    flex: 1,
                    text: 'Экономически обоснованный тариф, руб./кв.м.',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'SociallyAcceptableRate',
                    flex: 1,
                    text: 'Социально допустимый (установочный) тариф, руб./кв.м',
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        keyNavEnabled: false,
                        mouseWheelEnabled: false,
                        decimalSeparator: ','
                    },
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    dataIndex: 'RateDeficit',
                    flex: 1,
                    text: 'Дефицит тарифа, руб./кв.м.',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
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