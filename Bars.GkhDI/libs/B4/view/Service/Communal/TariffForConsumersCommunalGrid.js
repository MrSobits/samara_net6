Ext.define('B4.view.service.communal.TariffForConsumersCommunalGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.tariffforconsumcommungrid',
    requires: [
        'B4.view.Control.GkhDecimalField',

        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.TariffIsSetForDi'
    ],

    itemId: 'tariffForConsumersCommunalGrid',
    store: 'service.TariffForConsumers',
    title: 'Тарифы для потребителей',
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'DateStart',
                    width: 125,
                    text: 'Дата начала действия',
                    editor: 'datefield'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'DateEnd',
                    width: 125,
                    text: 'Дата окончания действия',
                    editor: 'datefield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TariffIsSetFor',
                    flex: 2,
                    text: 'Тариф установлен',
                    renderer: function (val) { return B4.enums.TariffIsSetForDi.displayRenderer(val); },
                    editor: {
                        xtype: 'combobox', editable: false,
                        store: B4.enums.TariffIsSetForDi.getStore(),
                        displayField: 'Display',
                        valueField: 'Value'
                    }
                },
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
                                    itemId: 'tariffForConsumersSaveButton'
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