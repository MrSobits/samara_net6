Ext.define('B4.view.appealcits.ContragentGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.appealcitscontragentgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',

        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    itemId: 'appealCitsContragentGrid',
    store: 'rapidresponsesystem.Appeal',
    title: 'Организации',
    closable: false,
    autoScroll: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    text: 'Наименование организации',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'actioncolumn',
                    name: 'createAppeal',
                    align: 'center',
                    width: 200,
                    text: 'Сформировать обращение в СОПР',
                    icon: B4.Url.content('content/img/icons/arrow_right.png'),
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var me = this,
                            scope = me.origScope;
                        scope.fireEvent('rowaction', scope, 'createAppeal', rec);
                    },
                    scope: me
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
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});