Ext.define('B4.view.realityobj.LiftRegisterGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.realityobjectliftregistergrid',

    requires: [
        'B4.store.realityobj.LiftRegister',

        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging',      
        'B4.form.ComboBox',
        'B4.form.GridStateColumn',
        'B4.enums.LiftAvailabilityDevices'
    ],

    title: 'Реестр лифтов',
    enableColumnHide: true,

    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.realityobj.LiftRegister');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    menuText: 'Статус',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gkh_real_obj';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CodeErc',
                    text: 'Код МКД в РП',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    text: 'Муниципальное образование',
                    flex: 1,
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
                    xtype: 'gridcolumn',
                    dataIndex: 'Settlement',
                    text: 'Населенный пункт',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 2,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PorchNum',
                    flex: 0.5,
                    text: 'Номер подъезда',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LiftNum',
                    flex: 0.5,
                    text: 'Номер лифта',
                    hidden: true,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ComissioningDate',
                    format: 'd.m.Y',
                    flex: 0.5,
                    text: 'Дата ввода в эксплуатацию',
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq,
                        format: 'd.m.Y'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RegNum',
                    flex: 0.5,
                    text: 'Регистрационный номер',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Capacity',
                    flex: 0.5,
                    text: 'Грузоподъемность',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        if (val) {
                            return Ext.util.Format.currency(val);
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StopCount',
                    flex: 0.5,
                    text: 'Количество остановок',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeLift',
                    flex: 1,
                    text: 'Тип лифта',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Cost',
                    flex: 0.5,
                    text: 'Стоимость работ по замене/ремонту',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        if (val) {
                            return Ext.util.Format.currency(val);
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CostEstimate',
                    flex: 0.5,
                    text: 'Стоимость оценки',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val) {
                        if (val) {
                            return Ext.util.Format.currency(val);
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'YearEstimate',
                    flex: 0.5,
                    text: 'Год проведения экспертной диагностики',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ReplacementPeriod',
                    flex: 0.5,
                    text: 'Период замены',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'YearInstallation',
                    flex: 0.5,
                    text: 'Год установки',                   
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'YearExploitation',
                    flex: 0.5,
                    text: 'Год ввода в эксплуатацию',                 
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ModelLift',
                    flex: 1,
                    text: 'Модель лифта',
                    hidden: true,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    flex: 1,
                    text: 'Обслуживающая организация',
                    hidden: true,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SpeedRise',
                    flex: 1,
                    text: 'Скорость подьема',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeLiftShaft',
                    flex: 1,
                    text: 'Шахта лифта',
                    hidden: true,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DepthLiftShaft',
                    flex: 0.5,
                    text: 'Глубина шахты',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'WidthLiftShaft',
                    flex: 0.5,
                    text: 'Ширина шахты',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'HeightLiftShaft',
                    flex: 0.5,
                    text: 'Высота шахты',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeLiftDriveDoors',
                    flex: 0.5,
                    text: 'Привод дверей кабины',
                    hidden: true,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeLiftMashineRoom',
                    flex: 0.5,
                    text: 'Расположение машинного помещения',
                    hidden: true,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AvailabilityDevices',
                    flex: 0.5,
                    text: 'Наличие устройства для автом. опускания',
                    hidden: true,
                    renderer: function (val) {
                        return B4.enums.LiftAvailabilityDevices.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.LiftAvailabilityDevices.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'LifeTime',
                    flex: 0.5,
                    text: 'Срок эксплуатации',
                    format: 'd.m.Y',
                    hidden: true,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DecommissioningDate',
                    flex: 0.5,
                    text: 'Дата вывода из эксплуатации',
                    format: 'd.m.Y',
                    hidden: true,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'PlanDecommissioningDate',
                    flex: 0.5,
                    text: 'Плановая дата вывода из эксплуатации',
                    format: 'd.m.Y',
                    hidden: true,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'YearPlannedReplacement',
                    flex: 0.5,
                    text: 'Плановый год замены',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberOfStoreys',
                    flex: 0.5,
                    text: 'Этажность',
                    hidden: true,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnerLift',
                    flex: 1,
                    text: 'Владелец лифтового оборудования',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CabinLift',
                    flex: 1,
                    text: 'Лифт (кабина)',
                    hidden: true,
                    filter: {
                        xtype: 'textfield'
                    }
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
                            items: [
                                { xtype: 'b4updatebutton' },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
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