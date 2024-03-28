Ext.define('B4.view.resolution.FizGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
         'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    alias: 'widget.resolutionFizGrid',
    title: 'Реквизиты физического лица',
    store: 'resolution.Fiz',
    itemId: 'resolutionFizGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PhysicalPersonDocType',
                    flex: 1,
                    text: 'Документ'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentSerial',
                    flex: 1,
                    text: 'Серия'
                },
               {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    text: 'Номер'
               },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PayerCode',
                    flex: 1,
                    text: 'Код плательщика'
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