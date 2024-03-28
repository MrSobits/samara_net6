Ext.define('B4.view.objectcr.qualification.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.qualgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.objectcr.Qualification'
    ],

    title: 'Участники отбора',
    store: 'objectcr.Qualification',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.objectcr.Qualification');

        Ext.util.Format.thousandSeparator = ' ';

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'actioncolumn',
                    width: 20,
                    itemId: 'qualToBuilderColumn',
                    icon: 'content/img/icons/bullet_magnify.png'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'BuilderName',
                    flex: 1,
                    text: 'Подрядчик'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    flex: 1,
                    text: 'Сумма',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Rating',
                    flex: 1,
                    text: 'Рейтинг'
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});