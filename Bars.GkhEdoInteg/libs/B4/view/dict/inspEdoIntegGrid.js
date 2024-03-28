Ext.define('B4.view.dict.inspEdoIntegGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.view.Control.GkhIntField',
        
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Сопоставление инспекторов',
    store: 'dict.InspectorEdoInteg',
    alias: 'widget.inspEdoIntegGrid',
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Fio',
                    flex: 1,
                    text: 'Инспектор'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CodeEdo',
                    flex: 1,
                    text: 'Код ЭДО',
                    editor: {
                        xtype: 'gkhintfield',
                        allowNegative: false,
                        minValue: 0
                    }
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4savebutton'
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