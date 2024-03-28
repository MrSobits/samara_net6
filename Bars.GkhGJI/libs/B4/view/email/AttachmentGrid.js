Ext.define('B4.view.email.AttachmentGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.form.ComboBox',
        'B4.ux.grid.selection.CheckboxModel'
    ], 
    store: 'email.EmailGjiAttachment',
    itemId: 'emailGjiAttachmentGrid',
    alias: 'widget.emailgjiattachmentgrid',

    initComponent: function () {
        var me = this;
        
        Ext.applyIf(me, {
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AttachmentFile',
                    text: 'Имя файла',
                    flex: 1,
                    renderer: function (v) {
                        return v.Name;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AttachmentFile',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
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
                            columns: 3,
                            items: [
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});