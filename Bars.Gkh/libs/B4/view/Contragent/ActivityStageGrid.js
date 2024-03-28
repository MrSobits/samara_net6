Ext.define('B4.view.contragent.ActivityStageGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.Control.GkhFileColumn',
        'B4.enums.ActivityStageType'
    ],

    title: 'Стадия деятельности',

    alias: 'widget.activitystagegrid',
    maxHeight: 300,
    minHeight: 300,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.contragent.ActivityStage');

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата начала'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    format: 'd.m.Y',
                    flex: 1,
                    text: 'Дата окончания'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ActivityStageType',
                    flex: 1,
                    text: 'Стадия',
                    renderer: function (val) {
                        return B4.enums.ActivityStageType.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Описание'
                },
                {
                    xtype: 'gkhfilecolumn',
                    dataIndex: 'Document',
                    flex: 1
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
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