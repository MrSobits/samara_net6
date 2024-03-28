Ext.define('B4.view.license.DisclosureInfoGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit', 
        
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.DisclosureInfoLicense'
    ],

    title: 'Лицензии',
    alias: 'widget.disinfolicensegrid',
    closable: false,
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.DisclosureInfoLicense');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LicenseNumber',
                    text: 'Номер Лицензии',
                    filter: { xtype: 'textfield' },
                    width: 150
                
                },                
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateReceived',
                    format: 'd.m.Y',
                    text: 'Дата получения',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    },
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LicenseOrg',
                    text: 'Орган выдавший лицензию',
                    filter: { xtype: 'textfield' },
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});