Ext.define('B4.view.supplyresourceorg.ContractGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.supplyresorgcontractgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Договора с жилыми домами',
    store: 'supplyresourceorg.Contract',
    
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    text: 'Жилой дом',
                    flex: 2
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    text: 'Дата начала',
                    format: 'd.m.Y',
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    text: 'Дата окончания',
                    format: 'd.m.Y',
                    flex: 1
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                                { xtype: 'b4addbutton' },
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