Ext.define('B4.view.dict.paymentsizecr.MuPaySizeGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.mupaysizecrpanel',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',        
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.dict.PaymentSizeMuRecord'
    ],

    store: 'dict.PaymentSizeMuRecord',
    
    title: 'Муниципальные образования',

    initComponent: function () {
        var me = this;
        
        Ext.applyIf(me, {
            columnLines: true,
            store: this.store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' }
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