Ext.define('B4.view.transferctr.RequestInfoPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.requestinfopanel',

    requires: [
        'B4.ux.grid.column.Enum',
        'B4.form.ComboBox',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.dict.ProgramCrType',
        'B4.store.dict.ProgramCr',
        'B4.store.ObjectCr',
        'B4.store.objectcr.TypeWorkCr',
        'B4.store.objectcr.ObjectCrBuilder',
        'B4.enums.TypeFinanceGroup',
        'B4.store.dict.FinanceSource'
    ],

    autoScroll: true,

    initComponent: function () {
        var me = this,
            vboxLayout = {
                type: 'vbox',
                align: 'stretch'
            };

        Ext.applyIf(me, {
            defaults: {
                padding: 3
            },
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'numberfield',
                                    name: 'DocumentNumPp',
                                    itemId: 'tfDocumentNumPp',
                                    fieldLabel: 'Номер ПП',
                                    minValue: 1,
                                    hideTrigger: true
                                },
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y',
                                    name: 'DateFromPp',
                                    itemId: 'dfDateFromPp',
                                    fieldLabel: 'Дата ПП'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNum',
                                    itemId: 'tfDocumentNum',
                                    fieldLabel: 'Номер заявки',
                                    maxLength: 50
                                },
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y',
                                    name: 'DateFrom',
                                    itemId: 'dfDateFrom',
                                    fieldLabel: 'Дата заявки',
                                    allowBlank: false
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            fieldLabel: 'Тип программы',
                            name: 'ProgramCrType',
                            store: 'B4.store.dict.ProgramCrType',
                            allowBlank: false,
                            editable: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1 },
                                { text: 'Код', dataIndex: 'Code', flex: 1 }
                            ],
                            windowContainerSelector: '#requestTransferCtrEditWindow'
                        },
                        {
                            xtype: 'b4selectfield',
                            fieldLabel: 'Программа капремонта',
                            name: 'ProgramCr',
                            itemId: 'sfProgramCr',
                            store: 'B4.store.dict.ProgramCr',
                            allowBlank: false,
                            editable: false,
                            columns: [
                                { dataIndex: 'Name', flex: 1, text: 'Наименование', filter: { xtype: 'textfield' } },
                                {
                                    dataIndex: 'TypeProgramCr', flex: 1, text: 'Тип',
                                    renderer: function (val) {
                                        return B4.enums.TypeProgramCr.displayRenderer(val);
                                    },
                                    filter: {
                                        xtype: 'b4combobox',
                                        items: B4.enums.TypeProgramCr.getItemsWithEmpty([null, '-']),
                                        editable: false,
                                        operand: CondExpr.operands.eq,
                                        valueField: 'Value',
                                        displayField: 'Display'
                                    }
                                }
                            ],
                            windowContainerSelector: '#requestTransferCtrEditWindow'
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'ObjectCr',
                            itemId: 'sfObjectCr',
                            fieldLabel: 'Объект КР',
                            store: 'B4.store.ObjectCr',
                            textProperty: 'RealityObjName',
                            editable: false,
                            readOnly: true,
                            allowBlank: false,
                            columns: [
                                {
                                    text: 'Муниципальное образование',
                                    dataIndex: 'Municipality',
                                    flex: 1,
                                    filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListWithoutPaging'
                                    }
                                },
                                {
                                    text: 'Адрес дома',
                                    dataIndex: 'RealityObjName',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'TypeWorkCr',
                            textProperty: 'WorkName',
                            fieldLabel: 'Вид работы',
                            store: 'B4.store.objectcr.TypeWorkCr',
                            allowBlank: false,
                            editable: false,
                            columns: [
                                { text: 'Наименование работы', dataIndex: 'WorkName', flex: 1 },
                                { text: 'Разрез финансирования', dataIndex: 'FinanceSourceName', flex: 1 }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'FinSource',
                            fieldLabel: 'Разрез финансирования',
                            store: 'B4.store.dict.FinanceSource',
                            editable: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1 },
                                { text: 'Код', dataIndex: 'Code', flex: 1 },
                                { xtype: 'b4enumcolumn', text: 'Группа финансирования', dataIndex: 'TypeFinanceGroup', flex: 1, enumName: 'B4.enums.TypeFinanceGroup' }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    layout: vboxLayout,
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    title: 'Информация о подрядной организации',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            itemId: 'sfBuilder',
                            name: 'Builder',
                            fieldLabel: 'Подрядная организация',
                            textProperty: 'ContragentName',
                            windowContainerSelector: '#requestTransferCtrEditWindow',
                            store: 'B4.store.objectcr.ObjectCrBuilder',
                            readOnly: true,
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            bodyPadding: 3,
                            padding: '0 0 5 0',
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1,
                                readOnly: true
                            },
                            items: [
                                {
                                    name: 'ContragentInn',
                                    itemId: 'tfInn',
                                    fieldLabel: 'ИНН'
                                },
                                {
                                    name: 'ContragentKpp',
                                    itemId: 'tfKpp',
                                    fieldLabel: 'КПП'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1,
                                readOnly: true
                            },
                            items: [
                                {
                                    name: 'ContragentPhone',
                                    itemId: 'tfPhone',
                                    fieldLabel: 'Контактный телефон'
                                },
                                {
                                    name: 'SettlementAccount',
                                    itemId: 'tfSettlementAccount',
                                    fieldLabel: 'Расчетный счет'
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            itemId: 'sfContragentBank',
                            name: 'ContragentBank',
                            fieldLabel: 'Наименование банка',
                            windowContainerSelector: '#requestTransferCtrEditWindow',
                            store: 'B4.store.contragent.Bank',
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            defaults: {
                                xtype: 'textfield',
                                labelWidth: 150,
                                labelAlign: 'right',
                                readOnly: true,
                                flex: 1
                            },
                            items: [
                                {
                                    name: 'CorrAccount',
                                    itemId: 'tfCorrAccount',
                                    fieldLabel: 'Кор. счет'
                                },
                                {
                                    name: 'Bik',
                                    itemId: 'tfBik',
                                    fieldLabel: 'БИК'
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Contract',
                            itemId: 'sfContract',
                            fieldLabel: 'Договор подряда',
                            allowBlank: false,
                            editable: false,
                            store: 'B4.store.objectcr.BuildContract',
                            textProperty: 'Text',
                            windowContainerSelector: '#requestTransferCtrEditWindow',
                            columns: [
                                {
                                    text: 'Номер',
                                    dataIndex: 'DocumentNum',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    xtype: 'datecolumn',
                                    format: 'd.m.Y',
                                    text: 'Дата',
                                    dataIndex: 'DocumentDateFrom',
                                    flex: 1,
                                    renderer: function(value) {
                                        if (value === '0001-01-01T00:00:00') {
                                            return '';
                                        }

                                        return Ext.util.Format.date(value, 'd.m.Y');
                                    },
                                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    fieldSetType: 'Regop',
                    layout: vboxLayout,
                    defaults: {
                        xtype: 'b4selectfield',
                        labelWidth: 150,
                        labelAlign: 'right',
                        allowBlank: false,
                        editable: false
                    },
                    title: 'Информация о плательщике',
                    items: [
                        {
                            name: 'RegOperator',
                            textProperty: 'Contragent',
                            fieldLabel: 'Региональный оператор',
                            store: 'B4.store.RegOperator',
                            columns: [
                                { text: 'Наименование', dataIndex: 'Contragent', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            name: 'RegopCalcAccount',
                            textProperty: 'AccountNumber',
                            fieldLabel: 'Счет',
                            store: 'B4.store.calcaccount.Regop',
                            columns: [
                                { text: 'Номер счета', dataIndex: 'ContragentAccountNumber', flex: 1 },
                                { text: 'Кредитная организация', dataIndex: 'ContragentCreditOrg', flex: 1 }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: vboxLayout,
                    defaults: {
                        labelWidth: 151,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Исполнитель',
                            name: 'Perfomer',
                            allowBlank: false
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'File',
                            fieldLabel: 'Файл',
                            possibleFileExtensions: 'pdf,doc,docx,xls,xlsx'
                        },
                        {
                            xtype: 'textfield',
                            name: 'Comment',
                            fieldLabel: 'Комментарий',
                            maxLength: 1000
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});
