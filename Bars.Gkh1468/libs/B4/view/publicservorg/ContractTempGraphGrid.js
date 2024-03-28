Ext.define('B4.view.publicservorg.ContractTempGraphGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.publicservorgcontracttempgraphgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    height: 400,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.publicservorg.ContractTempGraph');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OutdoorAirTemp',
                    text: 'Температура наружного воздуха',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield'
                    },
                    editor: {
                        type: 'numberfield',
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CoolantTempSupplyPipeline',
                    text: 'Температура теплоносителя в подающем трубопроводе',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield'
                    },
                    editor: {
                        type: 'numberfield',
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CoolantTempReturnPipeline',
                    text: 'Температура теплоносителя в обратном трубопроводе',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield'
                    },
                    editor: {
                        type: 'numberfield',
                        allowBlank: false
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
                                    name: 'saveGrid',
                                    iconCls: 'icon-accept',
                                    text: 'Сохранить'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});