Ext.define('B4.view.claimwork.lawsuit.CourtGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.claimworklawsuitcourtgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.claimwork.lawsuit.Court',
        'B4.enums.LawsuitCourtType',
        'B4.enums.PretensionType'
    ],

    title: 'Обжалование',
    
    closable: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.claimwork.lawsuit.Court');

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
                    dataIndex: 'DocNumber',
                    text: 'Номер претензии'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocDate',
                    text: 'Дата претензии',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LawsuitCourtType',
                    flex: 1,
                    text: 'Суд',
                    renderer: function (val) {
                        if (val) {
                            return B4.enums.LawsuitCourtType.displayRenderer(val);
                        }
                        return "";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PretensionType',
                    text: 'Вид жалобы',
                    flex: 1,
                    renderer: function (val) {
                        if (val) {
                            return B4.enums.PretensionType.displayRenderer(val);
                        }
                        return "";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PretensionResult',
                    text: 'Результат рассмотрения',
                    flex: 1.5
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    width: 100,
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
                            items: [
                                { xtype: 'b4addbutton' }
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