Ext.define('B4.view.manorglicense.ManOrgRequestRPGUGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.manorglicenserequestrpgugrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.RequestRPGUState',
        'B4.enums.RequestRPGUType',
        'B4.store.manorglicense.ManOrgRequestRPGU'
    ],

    title: 'Запросы РПГУ',

    closable: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.manorglicense.ManOrgRequestRPGU');

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
                    dataIndex: 'RequestRPGUState',
                    text: 'Статус',
                    enumName: 'B4.enums.RequestRPGUState',
                    flex: 0.5
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'RequestRPGUType',
                    text: 'Тип запроса',
                    enumName: 'B4.enums.RequestRPGUType',
                    flex: 0.5
                },      
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ObjectCreateDate',
                    flex: 1,
                    text: 'Дата запроса',
                    format: 'd.m.Y'
                },               
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    flex: 1,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
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