Ext.define('B4.view.manorglicense.ManOrgRequestSMEVGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.manorglicenserequestsmevgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.RequestSMEVType',
        'B4.enums.SMEVRequestState',
        'B4.store.manorglicense.ManOrgRequestSMEV'
    ],

    title: 'Запросы СМЭВ',

    closable: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.manorglicense.ManOrgRequestSMEV');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },              
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'RequestSMEVType',
                    text: 'Тип запроса',
                    enumName: 'B4.enums.RequestSMEVType',
                    flex: 0.5
                },    
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspector',
                    text: 'Автор',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'Date',
                    flex: 1,
                    text: 'Дата запроса',
                    format: 'd.m.Y'
                },               
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'SMEVRequestState',
                    text: 'Статус',
                    enumName: 'B4.enums.SMEVRequestState',
                    flex: 0.5
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});