Ext.define('B4.view.suspenseaccount.PersAccDistributionPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.suspaccpersaccdistribpanel',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.ux.button.Delete',
        'B4.ux.grid.Panel',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Enum',
        'B4.form.EnumCombo',
        'B4.enums.SuspenseAccountDistributionParametersView',
        'B4.enums.DistributeOn',
        'B4.store.regop.report.PaymentDocumentSnapshot',
        'B4.store.regop.personal_account.CrFundFormationDecisionFilterType',
        'B4.form.SelectField',
        'B4.Ajax'
    ],

    titleGrid: 'Выбор элементов',
    titleGridSelect: 'Элементы для выбора',
    titleGridSelected: 'Выбранные элементы',
    storeSelect: null,
    storeSelected: null,
    columnsGridSelect: [],
    columnsGridSelected: [],
    selModelMode: 'MULTI',

    bodyStyle: Gkh.bodyStyle,
    closable: true,
    autoScroll: true,
    title: 'Распределение',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    closeAction: 'destroy',

    leftGridToolbar: [],
    rightGridToolbar: [],

    initComponent: function () {
        var me = this,
            isExistDelColumn = false;

        me.addEvents(
            'selectedgridrowactionhandler'
        );

        if (!me.columnsGridSelected) {
            me.columnsGridSelected = [];
        }

        Ext.Array.each(me.columnsGridSelected, function (value) {
            if (value.xtype == 'b4deletecolumn') {
                isExistDelColumn = true;
            }
        });

        if (!isExistDelColumn) {
            me.columnsGridSelected.push({
                xtype: 'b4deletecolumn',
                handler: function (gridView, rowIndex, colIndex, el, e, rec) {
                    me.fireEvent('selectedgridrowactionhandler', 'delete', rec);
                }
            });
        }

        Ext.apply(me, {
            items: [
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Вид распределения',
                    labelWidth: 115,
                    margin: 5,
                    labelAlign: 'right',
                    store: B4.enums.SuspenseAccountDistributionParametersView.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'DistributionView',
                    value: 10
                },
                {
                    xtype: 'b4enumcombo',
                    fieldLabel: 'Распределить по',
                    labelWidth: 115,
                    margin: 5,
                    labelAlign: 'right',
                    enumName: 'B4.enums.DistributeOn',
                    value: B4.enums.DistributeOn.ChargesPenalties,
                    name: 'DistributeOn',
                    width: 310,
                    hidden: true
                },
                {
                    xtype: 'b4selectfield',
                    name: 'ChargePeriod',
                    fieldLabel: 'Период',
                    labelWidth: 115,
                    labelAlign: 'right',
                    margin: 5,
                    allowBlank: false,
                    store: 'B4.store.regop.ChargePeriod',
                    hidden: true,
                    columns: [
                        {
                            header: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'StartDate',
                            header: 'Дата начала',
                            format: 'd.m.Y',
                            flex: 1,
                            filter: {
                                xtype: 'datefield',
                                format: 'd.m.Y',
                                operand: CondExpr.operands.eq
                            }
                        },
                        {
                            xtype: 'datecolumn',
                            format: 'd.m.Y',
                            header: 'Дата окончания',
                            dataIndex: 'EndDate',
                            flex: 1,
                            filter: {
                                xtype: 'datefield',
                                format: 'd.m.Y',
                                operand: CondExpr.operands.eq
                            }
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'Snapshot',
                    fieldLabel: 'Документ',
                    labelWidth: 115,
                    labelAlign: 'right',
                    margin: 5,
                    store: 'B4.store.regop.report.PaymentDocumentSnapshot',
                    hidden: true,
                    disabled: true,
                    allowBlank: false,
                    textProperty: 'Payer',
                    columns: [
                        {
                            header: 'Плательщик',
                            dataIndex: 'Payer',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            header: 'Номер документа',
                            dataIndex: 'DocNumber',
                            flex: 1,
                            filter: { xtype: 'textfield' }
						},
						{
							header: 'Тип документа',
							dataIndex: 'PaymentDocumentType',
							flex: 1,
							filter: { xtype: 'textfield' }
						},
						{
							header: 'Сумма',
							dataIndex: 'TotalCharge' ,
							flex: 1
						},
						{
							header: 'Р/С получателя платежа',
							dataIndex: 'PaymentReceiverAccount' ,
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            xtype: 'datecolumn',
                            header: 'Дата документа',
                            dataIndex: 'DocDate',
                            flex: 1,
                            filter: {
                                xtype: 'datefield',
                                format: 'd.m.Y',
                                operand: CondExpr.operands.eq
                            }
                        }
                    ],
                    setValue: function (data) {
                        var cmp = this,
                            oldValue = cmp.getValue(),
                            isValid = cmp.getErrors() != '';

                        cmp.value = data;
                        cmp.updateDisplayedText(data);

                        cmp.fireEvent('validitychange', cmp, isValid);
                        if (data != oldValue) {
                            cmp.fireEvent('change', cmp, data, oldValue);
                        }
                        cmp.validate();
                        return cmp;
                    }
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        width: 250,
                        margin: 5,
                        labelWidth: 115,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'PeriodStartDate',
                            fieldLabel: 'Дата начала периода',
                            format: 'd.m.Y',
                            hidden: true,
                            listeners: {
                                beforerender: function (field) {
                                    B4.Ajax.request({
                                        url: B4.Url.action('GetStartDateOfFirstPeriod', 'ChargePeriod')
                                    }).next(function (response) {
                                        if (!response) {
                                            Ext.Msg.alert('Ошибка', 'При получении даты начала первого периода произошла ошибка');
                                            return;
                                        }
                                        var startDate = Ext.decode(response.responseText);
                                        field.setValue(startDate);
                                    }).error(function () {
                                        Ext.Msg.alert('Ошибка', 'При получении даты начала первого периода произошла ошибка');
                                    });
                                }
                            }
                        },
                        {
                            xtype: 'datefield',
                            name: 'PeriodEndDate',
                            fieldLabel: 'Дата окончания периода',
                            format: 'd.m.Y',
                            hidden: true,
                            value: new Date()
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    border: false,
                    flex: 1,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'b4grid',
                            name: 'selectGrid',
                            type: 'multiSelectGrid',
                            style: 'border-top: solid #99bce8 1px; border-right: solid #99bce8 1px;',
                            flex: 2.5,
                            title: 'Выбрать записи',
                            enableColumnHide: true,
                            border: false,
                            store: me.storeSelect,
                            selModel: Ext.create('Ext.selection.CheckboxModel', { mode: me.selModelMode }),
                            columns: me.columnsGridSelect,
                            plugins: [],
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
                                            columns: 1,
                                            items: [
                                                {
                                                    xtype: 'b4updatebutton'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'buttongroup',
                                            columns: 1,
                                            flex: 1,
                                            name: 'jurfilters',
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelWidth: 175,
                                                labelAlign: 'left'
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4selectfield',
                                                    name: 'legalOwner',
                                                    fieldLabel: 'Юридическое лицо',
                                                    jurFilterMustHide: true,
                                                    columns: [
                                                        { header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield', hideTrigger: true, operand: CondExpr.operands.icontains } },
                                                        { header: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield', hideTrigger: true, operand: CondExpr.operands.icontains } },
                                                        { header: 'КПП', dataIndex: 'Kpp', flex: 1, filter: { xtype: 'textfield', hideTrigger: true, operand: CondExpr.operands.icontains } }
                                                    ],
                                                    store: Ext.create('B4.base.Store', {
                                                        autoLoad: false,
                                                        fields: [
                                                            { name: 'Id' },
                                                            { name: 'Name' },
                                                            { name: 'Inn' },
                                                            { name: 'Kpp' }
                                                        ],
                                                        storeId: 'legalOwnerPersAccStore',
                                                        proxy: {
                                                            type: 'b4proxy',
                                                            controllerName: 'PersonalAccountOwner',
                                                            listAction: 'ListLegalOwners'                                                           
                                                        }
                                                    })
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    name: 'accountNum',
                                                    textProperty: 'BankAccountNumber',
                                                    disabled: true,
                                                    fieldLabel: 'Р/С получателя',
                                                    idProperty: 'RoId',
                                                    jurFilterMustHide: true,
                                                    columns: [
                                                        { header: 'Номер', dataIndex: 'BankAccountNumber', flex: 1, filter: { xtype: 'textfield' } },
                                                        { header: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } }
                                                    ],
                                                    store: Ext.create('B4.base.Store', {
                                                        autoLoad: false,
                                                        fields: [
                                                            { name: 'RoId' },
                                                            { name: 'BankAccountNumber' },
                                                            { name: 'Address' }
                                                        ],
                                                        proxy: {
                                                            type: 'b4proxy',
                                                            controllerName: 'RealityObjectPaymentAccount',
                                                            listAction: 'ListByPersonalAccountOwner',
                                                            timeout: 9999999
                                                        }
                                                    }),
                                                    listeners: {
                                                        beforeload: function (fld, opts) {
                                                            var panel = fld.up('suspaccpersaccdistribpanel'),
                                                                ownerId = panel.down('b4selectfield[name="legalOwner"]').getValue();

                                                            opts.params.ownerId = ownerId;
                                                        }
                                                    }
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    store: 'B4.store.regop.personal_account.CrFundFormationDecisionFilterType',
                                                    selectionMode: "MULTI",
                                                    windowCfg: { modal: true },
                                                    textProperty: 'Name',
                                                    fieldLabel: 'Способ формирования фонда КР',
                                                    editable: false,
                                                    columns: [
                                                        {
                                                            dataIndex: 'Name',
                                                            flex: 1
                                                        }
                                                    ],
                                                    name: 'crFoundType',
                                                    onSelectAll: function () {
                                                        var me = this;

                                                        me.setValue(null);
                                                        me.updateDisplayedText('Выбраны все');
                                                        me.selectWindow.hide();
                                                    }
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    allowBlank: true,
                                                    fieldLabel: 'Номер ИП/ИД/ИЛ',
                                                    name: 'rospNumber'
                                                },
                                                {
                                                    xtype: 'checkboxfield',
                                                    name: 'distributeUsingFilters',
                                                    boxLabel: 'Выбрать все с учетом фильтров'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4pagingtoolbar',
                                    displayInfo: true,
                                    store: me.storeSelect,
                                    dock: 'bottom'
                                }
                            ]
                        },
                        {
                            xtype: 'b4grid',
                            flex: 1,
                            style: 'border-top: solid #99bce8 1px;',
                            type: 'multiSelectedGrid',
                            name: 'selectedGrid',
                            border: false,
                            title: me.titleGridSelected,
                            store: me.storeSelected,
                            columns: me.columnsGridSelected,
                            listeners: {
                                afterrender: function () {
                                    var store = this.getStore();
                                    if (store) {
                                        store.pageSize = store.getCount();
                                    }
                                }
                            },
                            dockedItems: [
                                {
                                    xtype: 'toolbar',
                                    dock: 'top',
                                    items: [
                                        {
                                            xtype: 'tbfill'
                                        },
                                        {
                                            xtype: 'buttongroup',
                                            items: [

                                                {
                                                    xtype: 'b4deletebutton',
                                                    text: 'Удалить все'
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Продолжить',
                                    action: 'NextStep'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});