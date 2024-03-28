Ext.define('B4.view.import.chesimport.ComparedAddressGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete'
    ],

    alias: 'widget.chesimportcomparedaddressgrid',
    columnLines: true,
    title: 'Адреса',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.AddressMatch');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExternalAddress',
                    flex: 1,
                    text: 'Адрес из файла',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес из системы',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
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
                    view: me,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});
