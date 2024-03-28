/* Квалификационный документ: Дубликат, Переоформление и т.п. */
Ext.define('B4.view.person.QualificationDocumentGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.qualificationdocumentgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.form.FileField',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging'
    ],

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.person.QualificationDocument');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scome: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Number',
                    width: 100,
                    text: 'Номер бланка'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StatementNumber',
                    width: 100,
                    text: 'Номер заявления'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'IssuedDate',
                    text: 'Дата',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Document',
                    width: 150,
                    text: 'Файл заявления',
                    editor: { xtype: 'b4filefield' },
                    renderer: function (val) {
                        return val ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + val.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Note',
                    text: 'Комментарий',
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