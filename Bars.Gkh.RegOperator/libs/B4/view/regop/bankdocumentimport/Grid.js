Ext.define('B4.view.regop.bankdocumentimport.Grid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.form.EnumCombo',
        'B4.enums.BankDocumentImportStatus',
        'B4.enums.PersonalAccountDeterminationState',
        'B4.enums.PaymentConfirmationState',
        'B4.enums.BankDocumentImportCheckState',
        'B4.form.ComboBox',
        'B4.view.Control.GkhButtonImport'
    ],

    title: 'Реестр оплат платежных агентов',

    alias: 'widget.bankdocumentimportgrid',

    closable: true,
    enableColumnHide: true,
    store: 'regop.BankDocumentImport',

    initComponent: function () {
        var me = this,
        selModel = Ext.create('Ext.selection.CheckboxModel', {
            mode: 'MULTI'
        });

        Ext.applyIf(me, {
            selModel: selModel,
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
                    format: 'd.m.Y',
                    text: 'Дата операции',
                    dataIndex: 'ImportDate',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    text: 'Дата сводного реестра',
                    dataIndex: 'DocumentDate',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    text: 'Дата подтверждения',
                    dataIndex: 'AcceptDate',
                    flex: 1,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Номер сводного реестра',
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Сумма по реестру (руб.)',
                    dataIndex: 'ImportedSum',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Код платежного агента',
                    dataIndex: 'PaymentAgentCode',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Наименование платежного агента',
                    dataIndex: 'PaymentAgentName',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Банковская выписка',
                    dataIndex: 'BankStatement',
                    flex: 1,
                    filter: { xtype: 'textfield' } 
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.PersonalAccountDeterminationState',
                    dataIndex: 'PersonalAccountDeterminationState',
                    flex: 1,
                    filter: true,
                    header: 'Статус определения ЛС'
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.PaymentConfirmationState',
                    dataIndex: 'PaymentConfirmationState',
                    flex: 1,
                    filter: true,
                    header: 'Статус подтверждения оплат'
                },
                {
                    text: 'Наименование файла',
                    dataIndex: 'FileName',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    text: 'Количество оплат по ЛС',
                    dataIndex: 'PACount',
                    flex: 1,
                    sortable: false
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.BankDocumentImportCheckState',
                    dataIndex: 'CheckState',
                    flex: 1,
                    filter: true,
                    header: 'Проверка пройдена'
                },
                {
                    text: 'Тип импорта',
                    dataIndex: 'ImportType',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
                    }
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
                            columns: 3,
                            items: [
                                { xtype: 'gkhbuttonimport' },
                                {
                                    xtype: 'button',
                                    action: 'Check',
                                    tooltip: 'По нажатию произойдет проверка выбранного реестра',
                                    text: 'Проверить',
                                    textAlign: 'left',
                                    iconCls: 'icon-database-refresh',
                                    width: 160
                                },
                                {
                                    xtype: 'button',
                                    action: 'Delete',
                                    text: 'Удалить',
                                    iconCls: 'icon-delete'
                                },
                                {
                                    xtype: 'button',
                                    action: 'Accept',
                                    tooltip: 'По нажатию произойдет подтверждение начислений выбранной записи',
                                    text: 'Подтвердить',
                                    iconCls: 'icon-tick'
                                },
                                {
                                    xtype: 'button',
                                    action: 'Cancel',
                                    tooltip: 'По нажатию произойдет отмена подтверждения оплат выбранной записи',
                                    text: 'Отменить подтверждение',
                                    iconCls: 'icon-decline',
                                    width: 160
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            title: 'Фильтры',
                            defaults: {
                                xtype: 'container',
                                layout: {
                                    type: 'hbox',
                                    align: 'stretch'
                                },
                                margin: 2
                            },
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            boxLabel: 'Показать подтвержденные',
                                            fieldStyle: 'vertical-align: middle;',
                                            style: 'font-size: 11px !important;',
                                            margin: '-2 0 0 10',
                                            name: 'CheckShowConfirmed'
                                        },
                                        {
                                            xtype: 'checkbox',
                                            boxLabel: 'Показать удаленные',
                                            fieldStyle: 'vertical-align: middle;',
                                            style: 'font-size: 11px !important;',
                                            margin: '-2 0 0 10',
                                            name: 'CheckShowDeleted'
                                        }
                                    ]
                                },
                                {
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            boxLabel: 'Показать только реестры с неопределенными Р/С',
                                            fieldStyle: 'vertical-align: middle;',
                                            style: 'font-size: 11px !important;',
                                            margin: '-2 0 0 10',
                                            name: 'CheckShowRegisters',
                                            listeners: {
                                                afterRender: function (obj) {
                                                    try {
                                                        Ext.QuickTips.register({
                                                            target: obj.getEl(),
                                                            title: '',
                                                            text: '<span style="">' + 'При установке галочки будут отображены реестры с оплатами, по которым не определен номер лицевого или расчетного счета в системе' + '</span>',
                                                            enabled: true,
                                                            trackMouse: true
                                                        });
                                                    } catch (e) { }
                                                }
                                            }
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            title: 'Поиск',
                            defaults: {
                                xtype: 'container',
                                layout: {
                                    type: 'hbox',
                                    align: 'stretch'
                                },
                                margin: 2
                            },
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'PersonalAccountNumber',
                                            fieldLabel: 'Номер ЛС',
                                            labelWidth: 75,
                                            labelAlign: 'right',
                                            width: 200,
                                            listeners: {
                                                specialkey: function (field, e) {
                                                    if (e.getKey() == e.ENTER) {
                                                        this.fireEvent('enterclickevent', field);
                                                    }
                                                }
                                            }
                                        },
                                        {
                                            xtype: 'button',
                                            margin: '0 0 0 5px',
                                            action: 'Search',
                                            tooltip: 'При нажатии кнопки поиск будут учтены основные фильтры, а так же два доплнительных "Номер ЛС" и "Дата оплаты", если они заполнены',
                                            text: 'Поиск',
                                            iconCls: 'icon-page-white-magnify'
                                        }
                                    ]
                                },
                                {
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'PaymentDate',
                                            format: 'd.m.Y',
                                            fieldLabel: 'Дата оплаты',
                                            width: 200,
                                            labelAlign: 'right',
                                            labelWidth: 75,
                                            listeners: {
                                                specialkey: function (field, e) {
                                                    if (e.getKey() == e.ENTER) {
                                                        this.fireEvent('enterclickevent', field);
                                                    }
                                                }
                                            }
                                        }
                                    ]
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
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);
    }
});