Ext.define('B4.view.service.housing.TariffForConsumersHousingGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.tariffforconsumhousinggrid',
    requires: [
        'B4.view.Control.GkhDecimalField',

        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.TypeOrganSetTariffDi'
    ],

    itemId: 'tariffForConsumersHousingGrid',
    store: 'service.TariffForConsumersHousing',
    title: 'Тарифы',
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Cost',
                    flex: 1,
                    text: 'Тариф (руб.)',
                    editor: 'gkhdecimalfield',
                    renderer: function (val) {
                        if (!Ext.isEmpty(val) && val.toString().indexOf('.') != -1) {
                            return val.toString().replace('.', ',');
                        }
                        return val;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeOrganSetTariffDi',
                    flex: 1,
                    text: 'Тип организации, установившей тариф',
                    renderer: function (val) { return B4.enums.TypeOrganSetTariffDi.displayRenderer(val); },
                    editor: {
                        xtype: 'combobox', editable: false,
                        store: B4.enums.TypeOrganSetTariffDi.getStore(),
                        displayField: 'Display',
                        valueField: 'Value'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    format: 'd.m.Y',
                    width: 140,
                    text: 'Дата начала действия тарифа',
                    editor: 'datefield'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    format: 'd.m.Y',
                    width: 140,
                    text: 'Дата окончания действия тарифа',
                    editor: 'datefield'
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
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
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сохранить',
                                    tooltip: 'Сохранить',
                                    iconCls: 'icon-accept',
                                    itemId: 'tariffForConsumersHousingSaveButton'
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