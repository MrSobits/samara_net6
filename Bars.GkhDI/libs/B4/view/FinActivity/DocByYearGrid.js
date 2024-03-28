Ext.define('B4.view.finactivity.DocByYearGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.findocbyyeargrid',
    store: 'finactivity.DocByYear',
    itemId: 'finActivityDocByYearGrid',
    title: 'Документы',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit'
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
                    dataIndex: 'TypeDocByYearDi',
                    flex: 1,
                    text: 'Тип документа',
                    renderer: function (val) { return B4.enums.TypeDocByYearDi.displayRenderer(val); },
                    hidden: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Year',
                    flex: 1,
                    text: 'Год',
                    groupable: false
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    flex: 1,
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    groupable: false
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
            features: [{
                ftype: 'grouping',
                groupHeaderTpl: '{name}'
            }],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});