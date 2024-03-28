Ext.define('B4.view.regop.bankstatement.Grid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'Ext.selection.CheckboxModel',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.form.GridStateColumn',
        'B4.store.BankAccountStatement',
        'B4.store.regop.BankStatementAccountNumber',
        'B4.view.Control.GkhButtonImport',
        'B4.enums.DistributionState',
        'B4.enums.MoneyDirection',
        'B4.ux.grid.filter.YesNo',
        'B4.enums.YesNo'
    ],

    closable: true,
    title: 'Банковские операции',
    enableColumnHide: true,

    alias: 'widget.rbankstatementgrid',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.BankAccountStatement'),
            yesNoRenderer = function (val) {
                return val ? 'Да' : 'Нет';
            };

        Ext.apply(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    text: 'Идентификатор',
                    dataIndex: 'Id',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: false,
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата операции',
                    dataIndex: 'OperationDate',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата документа',
                    dataIndex: 'DocumentDate',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата поступления/списания',
                    dataIndex: 'DateReceipt',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function(val) {
                        return val > new Date(2000) ? Ext.Date.format(val, 'd.m.Y') : "";
                    },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата распределения',
                    dataIndex: 'DistributionDate',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield',
                        format: 'd.m.Y',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function(val) {
                        return val > new Date(2000) ? Ext.Date.format(val, 'd.m.Y') : "";
                    },
                    flex: 1
                },
                {
                    text: 'Номер документа',
                    dataIndex: 'DocumentNum',
                    filter: {
                        xtype: 'textfield'
                    },
                    flex: 1
                },
                {
                    xtype: 'b4enumcolumn',
                    text: 'Приход/Расход',
                    dataIndex: 'MoneyDirection',
                    enumName: 'B4.enums.MoneyDirection',
                    filter: true
                },
                {
                    text: 'Плательщик',
                    dataIndex: 'PayerFull',
                    filter: {
                        xtype: 'textfield'
                    },
                    flex: 1
                },
                {
                    text: 'Р/с плательщика',
                    dataIndex: 'PayerAccountNum',
                    filter: {
                        xtype: 'textfield'
                    },
                    flex: 1
                },
                {
                    text: 'Наименование плательщика',
                    dataIndex: 'PayerName',
                    filter: {
                        xtype: 'textfield'
                    },
                    flex: 1
                },
                {
                    text: 'ИНН плательщика',
                    dataIndex: 'PayerInn',
                    filter: {
                        xtype: 'textfield'
                    },
                    flex: 1
                },
                {
                    text: 'Р/с получателя',
                    dataIndex: 'RecipientAccountNum',
                    filter: {
                        xtype: 'textfield'
                    },
                    flex: 1
                },
                {
                    text: 'Наименование получателя',
                    dataIndex: 'RecipientName',
                    filter: {
                        xtype: 'textfield'
                    },
                    flex: 1
                },
                {
                    text: 'Сумма',
                    dataIndex: 'Sum',
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : null;
                    },
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        decimalSeparator: ',',
                        operand: CondExpr.operands.eq
                    },
                    width: 60
                },
                {
                    text: 'Остаток',
                    dataIndex: 'RemainSum',
                    renderer: function(val) {
                        return val ? Ext.util.Format.currency(val) : null;
                    },
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        decimalSeparator: ',',
                        operand: CondExpr.operands.eq
                    },
                    width: 60
                },
                {
                    text: 'Назначение платежа',
                    dataIndex: 'PaymentDetails',
                    filter: {
                        xtype: 'textfield'
                    },
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsROSP',
                    width: 100,
                    text: 'Взыскано РОСП',
                    renderer: yesNoRenderer,
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    text: 'Связанные документы',
                    dataIndex: 'LinkedDocuments',
                    filter: {
                        xtype: 'textfield'
                    },
                    flex: 1
                },
                {
                    xtype: 'actioncolumn',
                    dataIndex: 'DistributeState',
                    width: 210,
                    text: 'Статус',
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.DistributionState.getItemsWithEmpty([null, 'Все']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    },
                    //renderer не использовать, тк он подменяется 
                    defaultRenderer: function (value) {
                        return '<div style="float: left;">' + (value ? B4.enums.DistributionState.displayRenderer(value) : '') +
                            "</div><img data-qtip='История изменений' style='display:block; float: right;' src='content/img/icons/book_open.png'>";
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    text: 'Распределение возможно',
                    dataIndex: 'IsDistributable',
                    enumName: 'B4.enums.YesNo',
                    filter: true,
                    width: 130
                },
                {
                    text: 'Наименование файла',
                    dataIndex: 'FileName',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    text: 'Файл',
                    dataIndex: 'File',
                    renderer: function(v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    },
                    flex: 1
                },
                {
                    text: 'Документ',
                    dataIndex: 'Document',
                    renderer: function(v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    },
                    flex: 1
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            title: 'Действия',
                            columns: 4,
                            defaults: {
                                margin: '8 1 5 1'
                            },
                            items: [
                                {
                                    xtype: 'gkhbuttonimport'
                                },
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Распределить',
                                    action: 'distribute',
                                    iconCls: 'icon-money-add'
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function() {
                                            store.load();
                                        }
                                    }
                                },
                                {
                                    xtype: 'button',
                                    name: 'operation',
                                    text: 'Другие операции',
                                    iconCls: 'icon-cog-go',
                                    menu: [
                                        {
                                            xtype: 'menuitem',
                                            text: 'Указать возможность распределения',
                                            action: 'setdistributable'
                                        },
                                        {
                                            xtype: 'menuitem',
                                            text: 'Привязать к документу',
                                            action: 'link',
                                            actionName: 'add'
                                        },
                                        {
                                            xtype: 'menuitem',
                                            text: 'Изменить назначение платежа',
                                            action: 'changepaymentdetails'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'button',
                                    text: 'Удалить',
                                    action: 'delete',
                                    iconCls: 'icon-money-delete'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Отменить распределение',
                                    action: 'cancel',
                                    iconCls: 'icon-cross'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Выгрузка',
                                    iconCls: 'icon-package-go',
                                    menu: {
                                        items: [
                                            {
                                                iconCls: 'icon-page-excel',
                                                text: 'Экспорт',
                                                action: 'ExportToExcel'
                                            }
                                        ]
                                    }
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            title: 'Фильтры',
                            columns: 3,
                            defaults: {
                                margin: '5 1 5 7',
                                labelAlign: 'right',
                                width: 180
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.regop.BankStatementAccountNumber',
                                    selectionMode: 'MULTI',
                                    windowCfg: { modal: true },
                                    textProperty: 'AccountNumber',
                                    width: 480,
                                    colspan: 2,
                                    labelWidth: 110,
                                    labelAlign: 'right',
                                    fieldLabel: 'Расчетные счета',
                                    editable: false,
                                    onSelectAll: function() {
                                        var me = this;

                                        me.setValue('All');
                                        me.updateDisplayedText('Выбраны все');
                                        me.selectWindow.hide();
                                    },
                                    columns: [
                                        {
                                            text: 'Р/С получателя',
                                            dataIndex: 'AccountNumber',
                                            flex: 1,
                                            filter: { xtype: 'textfield' }
                                        }
                                    ],
                                    name: 'bsAccountNumber'
                                },
                                {
                                    xtype: 'checkbox',
                                    boxLabel: 'Показать удаленные',
                                    name: 'ShowDeleted'
                                },
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'Дата поступления с',
                                    format: 'd.m.Y',
                                    itemId:'dfDateFrom',
                                    width: 270,
                                    labelWidth: 110,
                                    name: 'DateReceiptFrom'
                                },
                                {
                                    xtype: 'datefield',
                                    fieldLabel: 'по',
                                   format: 'd.m.Y',
                                    labelWidth: 15,
                                    width: 200,
                                    name: 'DateReceiptBy'

                                },
                                {
                                    xtype: 'checkbox',
                                    boxLabel: 'Показать распределенные',
                                    name: 'ShowDistributed'
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
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }

        });
        me.callParent(arguments);
    }
});