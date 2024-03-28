Ext.define('B4.view.objectcr.performedworkact.PhotoGrid', {
    extend: 'B4.ux.grid.Panel',
    
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.PerfWorkActPhotoType',
        'B4.form.ComboBox'
    ],

    title: 'Фотографии акта',
    alias: 'widget.perfworkactphotogrid',
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.objectcr.performedworkact.PerformedWorkActPhoto');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'Название',
                    flex: 1,
                    filter: { xtype: 'textfield' } 
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Photo',
                    flex: 1,
                    text: 'Фото',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.PerfWorkActPhotoType',
                    dataIndex: 'PhotoType',
                    text: 'Тип фото',
                    flex: 1,
                    filter: true,
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});