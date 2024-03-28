Ext.define('B4.view.regop.personal_account.banrecalc.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.paccountbanrecalcgrid',

    title: 'Запрет перерасчета',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.regop.personal_account.BanRecalc',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit'
    ],

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.BanRecalc');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    format: 'F Y',
                    flex: 1,
                    text: 'Период начала действия'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    format: 'F Y',
                    flex: 1,
                    text: 'Период окончания действия'
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.BanRecalcType',
                    header: 'Тип операции',
                    dataIndex: 'Type',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Reason',
                    flex: 1,
                    text: 'Причина',
                    editor: {
                        xtype: 'textfield',
                        maxLength: 300
                    }
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
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});

