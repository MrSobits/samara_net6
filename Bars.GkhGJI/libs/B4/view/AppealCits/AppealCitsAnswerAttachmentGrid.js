Ext.define('B4.view.appealcits.AppealCitsAnswerAttachmentGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [        
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete'
    ],

    alias: 'widget.appealcitsanswerattachmentgrid',
    title: 'Вложения',
    columnLines: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.appealcits.AppealCitsAnswerAttachment');

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    text: 'Наименование',
                    dataIndex: 'Name',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Описание',
                    dataIndex: 'Description',
                    flex: 2,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FileInfo',
                    width: 100,
                    text: 'Файл',
                    renderer: function (value) {
                        return value && ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + value.Id) + '" target="_blank" style="color: black">Скачать</a>');
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
                    view: me,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});