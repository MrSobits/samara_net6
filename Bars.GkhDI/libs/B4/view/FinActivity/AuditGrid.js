Ext.define('B4.view.finactivity.AuditGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.finauditgrid',
    store: 'finactivity.Audit',
    itemId: 'finActivityAuditGrid',
    title: 'Аудиторские проверки',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        
        'B4.enums.TypeAuditStateDi'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    flex: 1,
                    text: 'Год'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeAuditStateDi',
                    flex: 1,
                    text: 'Состояние проверки',
                    renderer: function (val) { return B4.enums.TypeAuditStateDi.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Да</a>') : 'Нет';
                    }
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
                }
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});