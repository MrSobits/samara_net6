Ext.define('B4.view.regop.personal_account.PersonalAccountGrid',
{
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.selection.CheckboxModel',
        'B4.ux.grid.column.Enum',
        'B4.form.GridStateColumn',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.regop.personal_account.BasePersonalAccount',
        'B4.enums.regop.PersonalAccountOwnerType',
        'B4.enums.AccountFilterHasCharges',
        'B4.enums.AccountRegistryMode',
        'B4.enums.YesNo',
        'B4.store.regop.ChargePeriod',
        'B4.store.CashPaymentCenter',
        'B4.ux.grid.filter.YesNo',
        'B4.store.dict.Municipality',
        'B4.view.Control.GkhButtonImport',
        'B4.store.regop.personal_account.CrFundFormationDecisionFilterType',
        'B4.store.regop.personal_account.CrOwnerFilterType',
        'Ext.ux.grid.FilterBar',
        'B4.store.dict.PrivilegedCategory'
    ],

    title: 'Реестр лицевых счетов',
    alias: 'widget.paccountgrid',

    closable: true,
    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.personal_account.BasePersonalAccount'),
            numberFilter = { 
                xtype: 'numberfield',
                allowDecimals: false,
                hideTrigger: true,
                operand: CondExpr.operands.eq 
            };

        Ext.applyIf(me,
        {
            store: store,
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),
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
                    text: 'Номер',
                    minWidth: 100,
                    maxWidth: 120,
                    dataIndex: 'PersonalAccountNum',
                    filter: {
                        xtype: 'textfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'ЕНС',
                    minWidth: 100,
                    maxWidth: 120,
                    dataIndex: 'UnifiedAccountNumber',
                    filter: {
                        xtype: 'textfield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    text: 'Адрес',
                    dataIndex: 'RoomAddress',
                    flex: 1,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Settlement',
                    width: 160,
                    itemId: 'SettlementColumn',
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListSettlementWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 160,
                    text: 'Муниципальный район',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                {
                    text: 'ФИО/Наименование абонента',
                    flex: 1,
                    dataIndex: 'AccountOwner',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    text: 'Номер ЛС во внешних системах',
                    minWidth: 100,
                    maxWidth: 120,
                    dataIndex: 'PersAccNumExternalSystems',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    minWidth: 130,
                    maxWidth: 140,
                    menuText: 'Статус',
                    text: 'Статус',
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function(field, store, options) {
                                options.params.typeId = 'gkh_regop_personal_account';
                            },
                            storeloaded: {
                                fn: function(field) {
                                    field.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                    processEvent: function(type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.regop.PersonalAccountOwnerType',
                    filter: true,
                    text: 'Тип абонента',
                    dataIndex: 'OwnerType',
                    minWidth: 100,
                    maxWidth: 110
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNo',
                    filter: true,
                    text: 'Электронная квитанция',
                    dataIndex: 'DigitalReceipt',
                    minWidth: 100,
                    maxWidth: 110
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'HaveEmail',
                    text: 'Наличие эл.почты',
                    minWidth: 100,
                    maxWidth: 110,
                    trueText: 'Да',
                    falseText: 'Нет',
                    filter: { xtype: 'b4dgridfilteryesno', operator: 'eq' }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'IsNotDebtor',
                    text: 'Не считать должником',
                    minWidth: 100,
                    maxWidth: 110,
                    trueText: 'Да',
                    falseText: 'Нет',
                    filter: { xtype: 'b4dgridfilteryesno', operator: 'eq' }
                },
                {
                    text: 'Входящее сальдо',
                    tooltip: 'Входящее сальдо',
                    sortable: true,
                    dataIndex: 'SaldoIn',
                    flex: 1,
                    filter: numberFilter,
                    hidden: true
                },
                {
                    text: 'Начислено с учетом пени',
                    tooltip: 'Начислено с учетом пени',
                    sortable: true,
                    dataIndex: 'CreditedWithPenalty',
                    flex: 1,
                    filter: numberFilter,
                    hidden: true
                },
                {
                    text: 'Оплачено с учетом пени',
                    tooltip: 'Оплачено с учетом пени',
                    sortable: true,
                    dataIndex: 'PaidWithPenalty',
                    flex: 1,
                    filter: numberFilter,
                    hidden: true
                },
                {
                    text: 'Перерасчет с учетом пени',
                    tooltip: 'Перерасчет с учетом пени',
                    sortable: true,
                    dataIndex: 'RecalculationWithPenalty',
                    flex: 1,
                    filter: numberFilter,
                    hidden: true
                },
                {
                    text: 'Исходящее сальдо',
                    tooltip: 'Исходящее сальдо',
                    sortable: true,
                    dataIndex: 'SaldoOut',
                    flex: 1,
                    filter: numberFilter,
                    hidden: true
                },
                {
                    text: 'Дата открытия',
                    xtype: 'datecolumn',
                    minWidth: 100,
                    maxWidth: 110,
                    dataIndex: 'OpenDate',
                    filter: { xtype: 'datefield' },
                    renderer: function(value) {
                        if (value.indexOf('0001-01-01T') > -1) {
                            return '';
                        }

                        return Ext.util.Format.date(value, 'd.m.Y');
                    }
                },
                {
                    text: 'Долевая площадь помещения',
                    tooltip: 'Долевая площадь помещения',
                    sortable: true,
                    dataIndex: 'RealArea',
                    flex: 1,
                    filter: numberFilter,
                },
                {
                    text: 'Дата закрытия',
                    xtype: 'datecolumn',
                    minWidth: 100,
                    maxWidth: 110,
                    dataIndex: 'CloseDate',
                    filter: { xtype: 'datefield' },
                    renderer: function(value) {
                        if (!value || value.indexOf('0001-01-01T') > -1) {
                            return '';
                        }

                        return Ext.util.Format.date(value, 'd.m.Y');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    flex: 0.1,
                    dataIndex: 'HasCharges',
                    tooltip: 'Наличие начислений за текущий месяц',
                    text: 'Наличие начислений за текущий месяц',
                    renderer: function(val) {
                        return val ? 'Да' : 'Нет';
                    },
                    sortable: false,
                    filter: {
                        xtype: 'b4dgridfilteryesno',
                        operator: 'eq'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    flex: 0.1,
                    dataIndex: 'AccuralByOwnersDecision',
                    tooltip: 'Проводится начисление в соответствии с решением собственников',
                    text: 'Проводится начисление в соответствии с решением собственников',
                    sortable: false,
                    renderer: function(val) {
                        return val ? 'Да' : 'Нет';
                    }
                },
                {
                    xtype: 'gridcolumn',
                    flex: 0.1,
                    dataIndex: 'PrivilegedCategory',
                    itemId: 'PrivilegedCategory',
                    text: 'Наличие льготной категории',
                    tooltip: 'Наличие льготной категории',
                    renderer: function(val) {
                        return val ? 'Да' : 'Нет';
                    },
                    hidden: true,
                    filter: {
                        xtype: 'b4dgridfilteryesno',
                        operator: 'eq'
                    }
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    itemId: 'toptoolbar',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            title: 'Действия',
                            defaults: {
                                margin: 2
                            },
                            items: [
                                {
                                    text: 'Расчет',
                                    action: 'Charge',
                                    iconCls: 'icon-calculator'
                                },
                                {
                                    text: 'Обновить реестр',
                                    action: 'UpdateCache',
                                    iconCls: 'icon-arrow-refresh'
                                },
                                {
                                    xtype: 'button',
                                    name: 'accountoperation',
                                    text: 'Другие операции',
                                    iconCls: 'icon-cog-go',
                                    menu: []
                                },
                                {
                                    xtype: 'button',
                                    text: 'Выгрузка',
                                    iconCls: 'icon-package-go',
                                    menu: {
                                        items: [
                                            {
                                                text: 'Предпросмотр документов на оплату',
                                                action: 'PaymentDocumentsPreview',
                                                iconCls: 'icon-page-white-acrobat'
                                            },
                                            {
                                                text: 'Документы на оплату',
                                                action: 'GetPaymentDocuments',
                                                iconCls: 'icon-page-white-text'
                                            },
                                            {
                                                text: 'Документы на оплату (по частичному реестру)',
                                                action: 'GetPartialPaymentDocuments',
                                                iconCls: 'icon-page-white-text'
                                            },
                                            {
                                                iconCls: 'icon-page-excel',
                                                text: 'Экспорт',
                                                action: 'ExportToExcel'
                                            },
                                            {
                                                text: 'Выгрузка документов',
                                                action: 'GetZeroPaymentDocs',
                                                iconCls: 'icon-page-white-put'
                                            },
                                            {
                                                text: 'Выгрузка информации для ВЦКП',
                                                action: 'ExportToVtscp',
                                                iconCls: 'icon-page-white-put'
                                            },
                                            {
                                                text: 'Выгрузка начислений пени',
                                                action: 'ExportPenalty',
                                                iconCls: 'icon-page-white-put'
                                            },
                                            {
                                                text: 'Выгрузка начислений пени Excel',
                                                action: 'ExportPenaltyExcel',
                                                iconCls: 'icon-page-excel'
                                            },
                                            {
                                                text: 'Выгрузка информации по ЛС',
                                                action: 'ExportPersonalAccounts',
                                                iconCls: 'icon-page-white-put'
                                            },
                                            {
                                                iconCls: 'icon-page-excel',
                                                text: 'Выгрузка Сальдо',
                                                action: 'ExportSaldo'
                                            },
                                            '-',
                                            {
                                                text: 'Выгрузить начисления',
                                                action: 'expcalculation',
                                                menu: []
                                            }
                                        ]
                                    }
                                },
                                {
                                    xtype: 'gkhbuttonimport',
                                    itemId: 'btnImport'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Удалить ЛС',
                                    action: 'RemovePersonalAccounts',
                                    iconCls: 'icon-cross'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            name: 'ModeGroup',
                            title: 'Режимы работы',
                            defaults: {
                                margin: 2
                            },
                            items: [
                                {
                                    xtype: 'radiogroup',
                                    name: 'Mode',
                                    vertical: true,
                                    columns: 1,
                                    defaults: {
                                        name: 'Mode'
                                    },
                                    items: [
                                        {
                                            boxLabel: 'Текущий режим',
                                            inputValue: B4.enums.AccountRegistryMode.Common,
                                            checked: true,
                                            listeners: {
                                                render: {
                                                    fn: function(field) {
                                                        Ext.create('Ext.tip.ToolTip',
                                                        {
                                                            target: this.getEl().getAttribute('id'),
                                                            trackMouse: true,
                                                            width: 300,
                                                            html: 'Отображение актуальных данных на текущий период'
                                                        });
                                                    }
                                                }
                                            }
                                        },
                                        {
                                            boxLabel: 'Режим расчета',
                                            inputValue: B4.enums.AccountRegistryMode.Calc,
                                            listeners: {
                                                render: {
                                                    fn: function(field) {
                                                        Ext.create('Ext.tip.ToolTip',
                                                        {
                                                            target: this.getEl().getAttribute('id'),
                                                            trackMouse: true,
                                                            width: 300,
                                                            html:
                                                                'Отображение ЛС, по которым будет произведено начисление в открытом периоде'
                                                        });
                                                    }
                                                }
                                            }
                                        },
                                        {
                                            boxLabel: 'Формирование квитанций',
                                            inputValue: B4.enums.AccountRegistryMode.PayDoc,
                                            listeners: {
                                                render: {
                                                    fn: function(field) {
                                                        Ext.create('Ext.tip.ToolTip',
                                                        {
                                                            target: this.getEl().getAttribute('id'),
                                                            trackMouse: true,
                                                            width: 300,
                                                            html: 'Отображение данных, актуальных на выбранный период'
                                                        });
                                                    }
                                                }
                                            }
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            title: 'Фильтры',
                            flex: 1,
                            defaults: {
                                margin: 2
                            },
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    flex: 1,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 175
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.regop.ChargePeriod',
                                            textProperty: 'Name',
                                            editable: false,
                                            allowBlank: false,
                                            windowContainerSelector: '#' + me.getId(),
                                            windowCfg: {
                                                modal: true
                                            },
                                            trigger2Cls: '',
                                            columns: [
                                                {
                                                    text: 'Наименование',
                                                    dataIndex: 'Name',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                },
                                                {
                                                    text: 'Дата открытия',
                                                    xtype: 'datecolumn',
                                                    format: 'd.m.Y',
                                                    dataIndex: 'StartDate',
                                                    flex: 1,
                                                    filter: { xtype: 'datefield' }
                                                },
                                                {
                                                    text: 'Дата закрытия',
                                                    xtype: 'datecolumn',
                                                    format: 'd.m.Y',
                                                    dataIndex: 'EndDate',
                                                    flex: 1,
                                                    filter: { xtype: 'datefield' }
                                                },
                                                {
                                                    text: 'Состояние',
                                                    dataIndex: 'IsClosed',
                                                    flex: 1,
                                                    renderer: function(value) {
                                                        return value ? 'Закрыт' : 'Открыт';
                                                    }
                                                }
                                            ],
                                            name: 'ChargePeriod',
                                            labelAlign: 'right',
                                            fieldLabel: 'Период'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.regop.personal_account.CrFundFormationDecisionFilterType',
                                            selectionMode: 'MULTI',
                                            windowCfg: { modal: true },
                                            textProperty: 'Name',
                                            labelAlign: 'right',
                                            fieldLabel: 'Способ формирования фонда КР',
                                            editable: false,
                                            columns: [
                                                {
                                                    dataIndex: 'Name',
                                                    flex: 1
                                                }
                                            ],
                                            name: 'crFoundType'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.regop.personal_account.CrOwnerFilterType',
                                            selectionMode: 'MULTI',
                                            windowCfg: { modal: true },
                                            textProperty: 'Name',
                                            labelAlign: 'right',
                                            fieldLabel: 'Тип абонента',
                                            editable: false,
                                            columns: [
                                                {
                                                    dataIndex: 'Name',
                                                    flex: 1
                                                }
                                            ],
                                            name: 'crOwnerType'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'DeliveryAgent',
                                            fieldLabel: 'Агент доставки',
                                            store: 'B4.store.DeliveryAgent',
                                            selectionMode: 'MULTI',
                                            windowCfg: { modal: true },
                                            windowContainerSelector: '#' + me.getId(),
                                            textProperty: 'Name',
                                            labelAlign: 'right',
                                            editable: false,
                                            onSelectAll: function() {
                                                var me = this;

                                                me.setValue('All');
                                                me.updateDisplayedText('Выбраны все');
                                                me.selectWindow.hide();
                                            },
                                            columns: [
                                                {
                                                    text: 'Контрагент',
                                                    dataIndex: 'Name',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'OwnershipType',
                                            fieldLabel: 'Тип собственности',
                                            selectionMode: "MULTI",
                                            store: B4.enums.RoomOwnershipType.getStore(),
                                            textProperty: 'Display',
                                            labelAlign: 'right',
                                            editable: false,
                                            windowContainerSelector: '#' + me.getId(),
                                            windowCfg: {
                                                modal: true
                                            },
                                            listeners: {
                                                windowcreated: function (control, window) {
                                                    var selectAll = window.down('[text=Выбрать все]');
                                                    if (selectAll) {
                                                        selectAll.hide();
                                                    }
                                                }
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    flex: 1,
                                    defaults: {
                                        labelWidth: 175
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.PrivilegedCategory',
                                            selectionMode: 'MULTI',
                                            name: 'PrivilegedCategory',
                                            labelWidth: 210,
                                            labelAlign: 'right',
                                            editable: false,
                                            fieldLabel: 'Льготная категория',
                                            windowCfg: {
                                                modal: true
                                            },
                                            onSelectAll: function() {
                                                var me = this;
                                                me.setValue('All');
                                                me.updateDisplayedText('Выбраны все');
                                                me.selectWindow.hide();
                                            },
                                            columns: [
                                                {
                                                    xtype: 'gridcolumn',
                                                    dataIndex: 'Code',
                                                    flex: 2,
                                                    text: 'Код',
                                                    filter: {
                                                        xtype: 'textfield',
                                                        maxLength: 300
                                                    }
                                                },
                                                {
                                                    xtype: 'gridcolumn',
                                                    dataIndex: 'Name',
                                                    flex: 6,
                                                    text: 'Наименование',
                                                    filter: {
                                                        xtype: 'textfield',
                                                        maxLength: 300
                                                    }
                                                },
                                                {
                                                    xtype: 'gridcolumn',
                                                    dataIndex: 'Percent',
                                                    flex: 3,
                                                    text: 'Процент льготы',
                                                    filter: {
                                                        xtype: 'numberfield',
                                                        hideTrigger: true,
                                                        keyNavEnabled: false,
                                                        mouseWheelEnabled: false,
                                                        minValue: 0,
                                                        maxValue: 100,
                                                        operand: CondExpr.operands.eq
                                                    }
                                                },
                                                {
                                                    xtype: 'datecolumn',
                                                    dataIndex: 'DateFrom',
                                                    flex: 3,
                                                    text: 'Действует с',
                                                    format: 'd.m.Y',
                                                    filter: {
                                                        xtype: 'datefield',
                                                        operand: CondExpr.operands.eq
                                                    }
                                                },
                                                {
                                                    xtype: 'datecolumn',
                                                    dataIndex: 'DateTo',
                                                    flex: 3,
                                                    text: 'Действует по',
                                                    format: 'd.m.Y',
                                                    filter: {
                                                        xtype: 'datefield',
                                                        operand: CondExpr.operands.eq
                                                    }
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.CashPaymentCenter',
                                            textProperty: 'Name',
                                            labelAlign: 'right',
                                            editable: false,
                                            emptyText: 'Все РКЦ',
                                            windowContainerSelector: '#' + me.getId(),
                                            windowCfg: {
                                                modal: true
                                            },
                                            columns: [
                                                {
                                                    text: 'Наименование',
                                                    dataIndex: 'Name',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                }
                                            ],
                                            labelWidth: 210,
                                            name: 'CashPaymentCenter',
                                            fieldLabel: 'Расчетно-кассовый центр'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.PersAccGroup',
                                            selectionMode: 'MULTI',
                                            windowCfg: { modal: true },
                                            windowContainerSelector: '#' + me.getId(),
                                            textProperty: 'Name',
                                            labelWidth: 210,
                                            labelAlign: 'right',
                                            fieldLabel: 'Группы лицевых счетов',
                                            editable: false,
                                            onSelectAll: function() {
                                                var me = this;

                                                me.setValue('All');
                                                me.updateDisplayedText('Выбраны все');
                                                me.selectWindow.hide();
                                            },

                                            columns: [
                                                {
                                                    text: 'Наименование',
                                                    dataIndex: 'Name',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                },
                                                {
                                                    xtype: 'b4enumcolumn',
                                                    enumName: 'B4.enums.YesNo',
                                                    dataIndex: 'IsSystem',
                                                    text: 'Системная',
                                                    filter: true,
                                                    sortable: false
                                                }
                                            ],
                                            name: 'persAccGroup'
                                        },
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'HasCharges',
                                            fieldLabel: 'Наличие начислений/долга/переплаты',
                                            selectionMode: "MULTI",
                                            labelWidth: 210,
                                            store: B4.enums.AccountFilterHasCharges.getStore(),
                                            textProperty: 'Display',
                                            labelAlign: 'right',
                                            editable: false,
                                            windowContainerSelector: '#' + me.getId(),
                                            windowCfg: {
                                                modal: true
                                            },
                                            listeners: {
                                                windowcreated: function(control, window) {
                                                    var selectAll = window.down('[text=Выбрать все]');
                                                    if (selectAll) {
                                                        selectAll.hide();
                                                    }
                                                }
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'vbox',
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            boxLabel: 'Показать все записи',
                                            fieldStyle: 'vertical-align: middle;',
                                            style: 'font-size: 11px !important;',
                                            margin: '-2 0 0 10',
                                            action: 'ShowAll',
                                            width: 130,
                                            name: 'CheckShowAll'
                                        },
                                        {
                                            xtype: 'checkbox',
                                            boxLabel: 'Показать суммы',
                                            fieldStyle: 'vertical-align: middle;',
                                            style: 'font-size: 11px !important;',
                                            margin: '-2 0 0 10',
                                            action: 'ShowSums',
                                            width: 130,
                                            name: 'CheckShowSums'
                                        },
                                        {
                                            xtype: 'tbfill'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Перейти ко всем ЛС',
                                            iconCls: 'icon-accept',
                                            itemId: 'btnClearAllFilters'
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
                    store: store,
                    view: me,
                    dock: 'bottom',
                    listeners: {
                        beforechange: function() {
                            //при смене страницы очищаем выбранные ЛС
                            this.view.getSelectionModel().clearSelections();
                        }
                    }
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                // отключили
                //{
                //    ptype: 'filterbar',
                //    renderHidden: false,
                //    showShowHideButton: false,
                //    showClearAllButton: false
                //}
            ],
            viewConfig: {
                loadMask: true
            }

        });

        me.callParent(arguments);
    }
});