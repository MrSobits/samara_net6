Ext.define('B4.view.paysize.UpdateRetPreviewGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.updateretpreviewgrid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.paysize.RetPreview'
    ],

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.paysize.RetPreview');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    text: 'Адрес',
                    dataIndex: 'Address',
                    flex: 3
                },
                {
                    text: 'Старая классификация дома',
                    dataIndex: 'ExistEstateTypes',
                    flex: 2
                },
                {
                    text: 'Новая классификация дома',
                    dataIndex: 'NewEstateTypes',
                    flex: 2
                },
                {
                    text: 'Старый тариф',
                    dataIndex: 'ExistRate',
                    flex: 1
                },
                {
                    text: 'Новый тариф',
                    dataIndex: 'NewRate',
                    flex: 1
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'pagingtoolbar',
                    store: store,
                    pageSize: 5,
                    dock: 'bottom',
                    displayInfo: true
                }
            ]
        });

        me.callParent(arguments);
    }
});