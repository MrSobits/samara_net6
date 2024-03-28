Ext.define('B4.view.manorglicense.LicenseExtensionGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.manorglicenseextensiongrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeManOrgTypeDocLicense',
        'B4.store.manorglicense.LicenseExtension'
    ],

    title: 'Документы о продлении действия лицензии',
    
    closable: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.manorglicense.LicenseExtension');

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
                    dataIndex: 'DocType',
                    flex: 2,
                    text: 'Наименование документа',
                    renderer: function (val) {
                        if (val) {
                            return B4.enums.TypeManOrgTypeDocLicense.displayRenderer(val);
                        }
                        return "";
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocNumber',
                    flex: 1,
                    text: 'Номер'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocDate',
                    text: 'Дата',
                    format: 'd.m.Y',
                    width: 100
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