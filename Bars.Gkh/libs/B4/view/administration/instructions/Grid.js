Ext.define('B4.view.administration.instructions.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.instructionsGrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.administration.instruction.Instructions'
    ],
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.administration.instruction.Instructions');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'actioncolumn',
                    width: 20,
                    itemId: 'instructionDlColumn',
                    icon: 'content/img/icons/disk_download.png',
                    handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                        var param = { id: rec.get('File').Id };

                        var urlParams = Ext.urlEncode(param);

                        var newUrl = Ext.urlAppend('/FileUpload/Download?' + urlParams, '_dc=' + (new Date().getTime()));
                        newUrl = B4.Url.action(newUrl);

                        Ext.DomHelper.append(document.body, {
                            tag: 'iframe',
                            id: 'downloadIframe',
                            frameBorder: 0,
                            width: 0,
                            height: 0,
                            css: 'display:none;visibility:hidden;height:0px;',
                            src: newUrl
                        });
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DisplayName',
                    flex: 1,
                    text: 'Отображаемое имя'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Code',
                    flex: 1,
                    text: 'Код'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    flex: 1,
                    text: 'Название файла',
                    renderer: function (val) {
                        if(val)
                            return val.Name + '.' + val.Extention;
                        return "";
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