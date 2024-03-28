Ext.define('B4.view.outdoor.image.ImageGrid', {
    extend: 'B4.ux.grid.Panel',
    closable: true,
    alias: 'widget.outdoorimagegrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.enums.OutdoorImagesGroup'
    ],

    title: 'Фото-архив',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.outdoor.Image');

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateImage',
                    text: 'Дата изображения',
                    format: 'd.m.Y',
                    flex: 0.5,
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: {
                        xtype: 'textfield'
                    },
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'ImagesGroup',
                    flex: 1,
                    text: 'Группа',
                    enumName: 'B4.enums.OutdoorImagesGroup',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Period',
                    flex: 1,
                    text: 'Период',
                    filter: {
                        xtype: 'textfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WorkCr',
                    flex: 1,
                    text: 'Вид работы',
                    filter: {
                        xtype: 'textfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Описание',
                    filter: {
                        xtype: 'textfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    flex: 0.5,
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
            viewConfig: { loadMask: true },
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