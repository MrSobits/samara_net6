Ext.define('B4.view.specialaccount.RowGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Информация по спец счетам1',
    store: 'specialaccount.SpecialAccountRow',
    alias: 'widget.specialaccountrowgrid',
    closable:false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me,
        {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    flex: 1.3,
                    editable: false,
                    height: 230,
                    text: 'Полное <br />наименование <br />владельца <br />специального <br />счета '
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentInn',
                    flex: 0.7,
                    height: 230,
                    text: 'ИНН'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1.2,
                    height: 230,
                    text: 'Адрес МКД'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccuracyArea',
                    flex: 0.5,
                    allowBlank: false,
                    height: 230,
                    text: 'Общая <br />площадь <br />МКД (кв.м.)',
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SpecialAccountNum',
                    flex: 1,
                    allowBlank: false,
                    height: 230,
                    text: 'Номер <br />специального <br />счета',
                    editor: {
                        xtype: 'textfield'
                    },                    
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'StartDate',
                    flex: 0.7,
                    allowBlank: false,
                    height: 230,
                    text: 'Дата начала <br />формирования <br />ФКР',
                    editor: {
                        xtype: 'datefield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Tariff',
                    flex: 0.7,
                    text: 'Размер взноса <br /> на КР',
                    height: 230,
                    allowBlank: false,
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Accured',
                    flex: 0.7,
                    text: 'Начислено <br />взносов <br />на КР за <br />отчетный <br />период',
                    height: 230,
                    allowBlank: false,
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccuredTotal',
                    flex: 0.7,
                    height: 230,
                    allowBlank: false,
                    text: 'Начислено <br />взносов <br />на КР с момента <br />формирования <br />ФКР',
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncomingTotal',
                    flex: 0.7,
                    height: 230,
                    text: 'Оплачено <br />взносов <br />с момента <br />формирования <br />ФКР',
                    allowBlank: false,
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Incoming',
                    flex: 0.7,
                    allowBlank: false,
                    height: 230,
                    text: 'Оплачено <br />взносов <br />на КР в <br />отчетном <br />периоде',
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AmountDebtForPeriod',
                    flex: 0.7,
                    height: 230,
                    text: 'Задолженность <br />по оплате за <br />отчетный <br />период',
                    allowBlank: false,
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AmmountDebt',
                    flex: 0.7,
                    height: 230,
                    text: 'Задолженность <br />по оплате <br />с момента <br />формирования <br />ФКР',
                    allowBlank: false,
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Transfer',
                    flex: 0.7,
                    height: 230,
                    text: 'Израсходовано <br />за отчетный <br />период',
                    allowBlank: false,
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TransferTotal',
                    flex: 0.7,
                    height: 230,
                    text: 'Израсходовано <br />с момента <br />формирования <br />ФКР',
                    allowBlank: false,
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Ballance',
                    flex: 0.7,
                    height: 230,
                    text: 'Размер остатка <br />средств на <br />спецсчете',
                    allowBlank: false,
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contracts',
                    flex: 0.7,
                    hheight: 230,
                    text: 'Сведения <br />о заключении <br />договоров <br />займа <br />на проведение <br />КР',
                    editor: {
                        xtype: 'textfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AmountDebtCredit',
                    flex: 0.7,
                    hheight: 230,
                    text: 'Сумма <br />задолженности <br />по договорам <br />займа',
                    editor: {
                        xtype: 'textfield'
                    },
                },
                
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Perscent',
                    flex: 0.7,
                    height: 230,
                    text: '%',
                    hidden: true,
                },

                
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
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
                                 { xtype: 'b4savebutton' }
                             ]
                         },
                        //{
                        //    xtype: 'buttongroup',
                        //    columns: 3,
                        //    items: [                             
                        //        {
                        //            xtype: 'b4addbutton'
                        //        }                              
                        //    ]
                        //},
                        // {
                        //    xtype: 'buttongroup',
                        //    columns: 3,
                        //    items: [                             
                        //        {
                        //            xtype: 'b4updatebutton'
                        //        }                              
                        //    ]
                        //}
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
//        //
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