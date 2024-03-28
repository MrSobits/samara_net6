Ext.define('B4.view.realityobj.ResOrgGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realobjresorggrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Поставщики коммунальных услуг',
    store: 'realityobj.ResOrg',
    closable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ResourceOrg',
                    flex: 1,
                    text: 'Наименование'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    width: 100,
                    text: 'Дата начала',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    width: 100,
                    text: 'Дата окончания',
                    format: 'd.m.Y'
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
                            columns: 1,
                            items: [
                                { xtype: 'b4updatebutton' }
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