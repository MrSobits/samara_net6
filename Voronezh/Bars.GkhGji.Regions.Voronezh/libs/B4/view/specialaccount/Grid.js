Ext.define('B4.view.specialaccount.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.enums.YearEnums',
        'B4.enums.MonthEnums',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.view.button.Sign'
    ],

    title: 'Реестр отчетов по спецсчетам',
    store: 'specialaccount.SpecialAccountReport',
    alias: 'widget.specialaccountreportgrid',
    closable: true,

    initComponent: function () {
        var me = this;
        var currMonthEnums = B4.enums.MonthEnums.getItemsWithEmpty([null, '-']);
                var newMonthEnums = [];
                Ext.iterate(currMonthEnums, function (val, key) {
                    newMonthEnums.push(val);
                });

                var currYearEnums = B4.enums.YearEnums.getItemsWithEmpty([null, '-']);
                var newYearEnums = [];
                Ext.iterate(currYearEnums, function (val, key) {
                    newYearEnums.push(val);
                });
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    flex: 1,
                    text: 'Контрагент',
                    filter: { xtype: 'textfield' },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inn',
                    flex: 0.5,
                    text: 'ИНН контрагента',
                    filter: { xtype: 'textfield' },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MonthEnums',
                    width: 150,
                    text: 'Отчетный месяц',
                    renderer: function (value) {
                        return B4.enums.MonthEnums.displayRenderer(value);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: newMonthEnums,
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'YearEnums',
                    width: 150,
                    text: 'Отчетный год',
                    renderer: function (value) {
                        return B4.enums.YearEnums.displayRenderer(value);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: newYearEnums,
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sertificate',
                    flex: 1,
                    text: 'Подпись',
                    filter: { xtype: 'textfield' }

                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SignedXMLFile',
                    width: 100,
                    text: 'Подписанный файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileTransport/GetFileFromPublicServer?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
                }, 
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'SignedXMLFile',
                //    width: 100,
                //    text: 'Подписанный файл',
                //    renderer: function (v) {
                //        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                //    }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Author',
                    flex: 0.5,
                    text: 'Автор',
                    filter: { xtype: 'textfield' }
                },
                //{
                //    xtype: 'actioncolumn',
                //    text: 'Перейти к паспорту',
                //    action: 'openpassport',
                //    width: 174,
                //    items: [{
                //        tooltip: 'Перейти к паспорту',
                //        iconCls: 'icon-fill-button',
                //        icon: 'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAKgAAAAWCAYAAABUi9exAAAAGXRFWHRTb2Z0d2FyZQBBZG9iZSBJbWFnZVJlYWR5ccllPAAAAyJpVFh0WE1MOmNvbS5hZG9iZS54bXAAAAAAADw/eHBhY2tldCBiZWdpbj0i77u/IiBpZD0iVzVNME1wQ2VoaUh6cmVTek5UY3prYzlkIj8+IDx4OnhtcG1ldGEgeG1sbnM6eD0iYWRvYmU6bnM6bWV0YS8iIHg6eG1wdGs9IkFkb2JlIFhNUCBDb3JlIDUuMy1jMDExIDY2LjE0NTY2MSwgMjAxMi8wMi8wNi0xNDo1NjoyNyAgICAgICAgIj4gPHJkZjpSREYgeG1sbnM6cmRmPSJodHRwOi8vd3d3LnczLm9yZy8xOTk5LzAyLzIyLXJkZi1zeW50YXgtbnMjIj4gPHJkZjpEZXNjcmlwdGlvbiByZGY6YWJvdXQ9IiIgeG1sbnM6eG1wPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiIHhtcDpDcmVhdG9yVG9vbD0iQWRvYmUgUGhvdG9zaG9wIENTNiAoV2luZG93cykiIHhtcE1NOkluc3RhbmNlSUQ9InhtcC5paWQ6NjRCRTAyNkUxMzFGMTFFM0E1OEE4MzMwREIzQkFCRDUiIHhtcE1NOkRvY3VtZW50SUQ9InhtcC5kaWQ6NjRCRTAyNkYxMzFGMTFFM0E1OEE4MzMwREIzQkFCRDUiPiA8eG1wTU06RGVyaXZlZEZyb20gc3RSZWY6aW5zdGFuY2VJRD0ieG1wLmlpZDo2NEJFMDI2QzEzMUYxMUUzQTU4QTgzMzBEQjNCQUJENSIgc3RSZWY6ZG9jdW1lbnRJRD0ieG1wLmRpZDo2NEJFMDI2RDEzMUYxMUUzQTU4QTgzMzBEQjNCQUJENSIvPiA8L3JkZjpEZXNjcmlwdGlvbj4gPC9yZGY6UkRGPiA8L3g6eG1wbWV0YT4gPD94cGFja2V0IGVuZD0iciI/PvsEoDoAAAX6SURBVHja7FtNTBtH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'
                //    }]
                //},
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'ObjectEditDate',
                    flex: 0.5,
                    allowBlank: false,
                    text: 'Системная дата',
                    editor: {
                        xtype: 'datefield'
                    },
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'DateAccept',
                    flex: 0.5,
                    allowBlank: false,
                    text: 'Дата сдачи',
                    editor: {
                        xtype: 'datefield'
                    },
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'b4signbutton',
                                    disabled: true
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



//Ext.define('B4.view.specialaccount.Grid', {
//    extend: 'B4.ux.grid.Panel',
//    requires: [
//        'B4.ux.button.Add',
//        'B4.ux.button.Update',

//        'B4.ux.grid.column.Delete',
//        'B4.ux.grid.column.Edit',

//        'B4.ux.grid.plugin.HeaderFilters',
//        'B4.ux.grid.toolbar.Paging'
//    ],

//    title: 'Отделения судебных tjygjyghkj',
//    store: 'specialaccount.SpecialAccountReport',
//    alias: 'widget.specialAccountGrid',
//    closable: true,

//    initComponent: function () {
//        var me = this;

//        Ext.applyIf(me, {
//            columnLines: true,
//            columns: [
//                 {
//                    xtype: 'gridcolumn',
//                    dataIndex: 'Sertificate',
//                    flex: 1,
//                    text: 'Подпись',
//                    filter: { xtype: 'textfield' }

//                }
//            ],
//            plugins: [
//                Ext.create('B4.ux.grid.plugin.HeaderFilters')
//            ],
//            viewConfig: {
//                loadMask: true
//            },
//            dockedItems: [
//                {
//                    xtype: 'toolbar',
//                    dock: 'top',
//                    items: [
//                        {
//                            xtype: 'buttongroup',
//                            columns: 3,
//                            items: [
//                                {
//                                    xtype: 'b4addbutton'
//                                },
//                                {
//                                    xtype: 'b4updatebutton'
//                                }
//                            ]
//                        }
//                    ]
//                },
//                {
//                    xtype: 'b4pagingtoolbar',
//                    displayInfo: true,
//                    store: this.store,
//                    dock: 'bottom'
//                }
//            ]
//        });

//        me.callParent(arguments);
//    }
//});


//Ext.define('B4.view.specialaccount.Grid', {
//    extend: 'B4.ux.grid.Panel',
//    requires: [
//        'B4.ux.grid.column.Delete',
//        //'B4.form.GridStateColumn',
//        'B4.ux.grid.column.Edit',
//        'B4.ux.grid.plugin.HeaderFilters',
//        'B4.ux.grid.toolbar.Paging',
//        'B4.enums.YearEnums',
//        'B4.enums.MonthEnums',
//        //'B4.store.CreditOrg',
//        //'B4.enums.TypeBase',
//        //'B4.view.button.Sign'
//        'B4.ux.button.Close',
//        'B4.ux.button.Save',
//    ],

//    title: 'Отчет по спецсчетам',
//    store: 'specialaccount.SpecialAccountReport',
//    //itemId: 'specialAccountGrid',
//    alias: 'widget.specialAccountGrid',
//    closable: true,
//    enableColumnHide: true,

//    initComponent: function () {
//        var me = this;

//        //var currMonthEnums = B4.enums.MonthEnums.getItemsWithEmpty([null, '-']);
//        //var newMonthEnums = [];
//        //Ext.iterate(currMonthEnums, function (val, key) {
//        //    newMonthEnums.push(val);
//        //});

//        //var currYearEnums = B4.enums.YearEnums.getItemsWithEmpty([null, '-']);
//        //var newYearEnums = [];
//        //Ext.iterate(currYearEnums, function (val, key) {
//        //    newYearEnums.push(val);
//        //});
//        //debugger;
//        Ext.applyIf(me, {
//            columnLines: true,
//            columns: [
//                //{
//                //    xtype: 'b4editcolumn',
//                //    scope: me
//                //},
//                //{
//                //    xtype: 'gridcolumn',
//                //    dataIndex: 'Contragent',
//                //    flex: 1,
//                //    text: 'Контрагент',
//                //    filter: { xtype: 'textfield' },
//                //    renderer: function (val, meta, rec) {
//                //        return renderer(val, meta, rec);
//                //    }
//                //},
//                //{
//                //    xtype: 'gridcolumn',
//                //    dataIndex: 'MonthEnums',
//                //    width: 150,
//                //    text: 'Отчетный месяц',
//                //    renderer: function (val, meta, rec) {
//                //        val = renderer(val, meta, rec);
//                //        return B4.enums.MonthEnums.displayRenderer(val);
//                //    },
//                //    filter: {
//                //        xtype: 'b4combobox',
//                //        items: newMonthEnums,
//                //        editable: false,
//                //        operand: CondExpr.operands.eq,
//                //        valueField: 'Value',
//                //        displayField: 'Display'
//                //    }
//                //},
//                //{
//                //    xtype: 'gridcolumn',
//                //    dataIndex: 'YearEnums',
//                //    width: 150,
//                //    text: 'Отчетный месяц',
//                //    renderer: function (val, meta, rec) {
//                //        val = renderer(val, meta, rec);
//                //        return B4.enums.YearEnums.displayRenderer(val);
//                //    },
//                //    filter: {
//                //        xtype: 'b4combobox',
//                //        items: newYearEnums,
//                //        editable: false,
//                //        operand: CondExpr.operands.eq,
//                //        valueField: 'Value',
//                //        displayField: 'Display'
//                //    }
//                //},
//                {
//                    xtype: 'gridcolumn',
//                    dataIndex: 'Sertificate',
//                    flex: 1,
//                    text: 'Подпись',
//                    filter: { xtype: 'textfield' }
                    
//                },
//                //{
//                //    xtype: 'gridcolumn',
//                //    dataIndex: 'SignedXMLFile',
//                //    width: 100,
//                //    text: 'Подписанный файл',
//                //    renderer: function (v) {
//                //        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
//                //    }
//                //},
//                //{
//                //    xtype: 'b4deletecolumn',
//                //    scope: me
//                //}
//            ],
//            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
//            viewConfig: {
//                loadMask: true
//            },
//            dockedItems: [
//                {
//                    xtype: 'toolbar',
//                    dock: 'top',
//                    items: [
//                        {
//                            xtype: 'buttongroup',
//                            columns: 1,
//                            items: [
//                                {
//                                    xtype: 'b4updatebutton'
//                                },
//                                //{
//                                //    xtype: 'b4addbutton',
//                                //    text: 'Добавить новый'
//                                //},
//                                //{
//                                //    xtype: 'b4signbutton',
//                                //    //disabled: true
//                                //}
//                            ]
//                        }
//                    ]
//                },
//                {
//                    xtype: 'b4pagingtoolbar',
//                    displayInfo: true,
//                    store: this.store,
//                    dock: 'bottom'
//                }
//            ]
//        });

//        me.callParent(arguments);
//    }
//});