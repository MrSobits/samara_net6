Ext.define('B4.view.suspenseaccount.RealtyAccDistributionPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.realtyaccdistribpanel',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.ux.button.Delete',
        'B4.ux.grid.Panel',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Delete',
        'B4.form.EnumCombo',
        'B4.enums.SuspenseAccountDistributionParametersView',
        'B4.store.regop.personal_account.CrFundFormationDecisionFilterType',
        'B4.store.dict.ProgramCr',
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

        if (!me.rightGridToolbar || !me.rightGridToolbar.length) {
            me.rightGridToolbar = [];
            me.rightGridToolbar.push({
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
                    xtype: 'b4selectfield',
                    name: 'ProgramCr',
                    fieldLabel: 'Программа КР',
                    labelWidth: 115,
                    labelAlign: 'right',
                    margin: 5,
                    store: 'B4.store.dict.ProgramCr',
                    columns: [
                        {
                            header: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            header: 'Код',
                            dataIndex: 'Code',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        },
                        {
                            header: 'Период',
                            dataIndex: 'PeriodName',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObjectOriginator',
                    fieldLabel: 'Адрес дома',
                    labelWidth: 115,
                    labelAlign: 'right',
                    margin: 5,
                    hidden: true,
                    editable: false,
                    textProperty: 'Address',
                    store: 'B4.store.RealityObject',
                    columns: [
                        {
                            header: 'Муниципальный район',
                            dataIndex: 'Municipality',
                            flex: 0.7,
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
                            header: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
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
                                beforerender: function(field) {
                                    B4.Ajax.request({
                                        url: B4.Url.action('GetStartDateOfFirstPeriod', 'ChargePeriod')
                                    }).next(function(response) {
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
                            flex: 1.4,
                            title: "Выбрать записи",
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
                                                    name: 'crFundTypes',
                                                    onSelectAll: function () {
                                                        var me = this;
                                                        
                                                        me.setValue('All');
                                                        me.updateDisplayedText('Выбраны все');
                                                        me.selectWindow.hide();
                                                    }
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    name: 'accountNum',
                                                    textProperty: 'BankAccountNumber',
                                                    fieldLabel: 'Р/С получателя',
                                                    idProperty: 'RoId',
                                                    selectionMode: 'MULTI',
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
                                                            timeout: 60000 * 5
                                                        }
                                                    }),
                                                    onSelectAll: function () {
                                                        var me = this;

                                                        me.setValue('All');
                                                        me.updateDisplayedText('Выбраны все');
                                                        me.selectWindow.hide();
                                                    }
                                                },
                                                {
                                                    xtype: 'checkboxfield',
                                                    name: 'distributeUsingFilters',
                                                    boxLabel: 'Выбрать все с учетом фильтров'
                                                }
                                            ]
                                        }
                                    ].concat(me.leftGridToolbar)
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
                            dockedItems: me.rightGridToolbar
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