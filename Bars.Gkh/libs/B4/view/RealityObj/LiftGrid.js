Ext.define('B4.view.realityobj.LiftGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.realityobjectliftgrid',

    requires: [
        'B4.store.realityobj.Lift',

        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging',

        'B4.form.ComboBox',

        'B4.enums.LiftAvailabilityDevices'
    ],

    title: 'Лифтовое оборудование',
    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.realityobj.Lift');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PorchNum',
                    flex: 1,
                    text: 'Номер подъезда',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LiftNum',
                    flex: 1,
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
                    flex: 1,
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
                    flex: 1,
                    text: 'Регистрационный номер',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Capacity',
                    flex: 1,
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
                    flex: 1,
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
                    flex: 1,
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
                    flex: 1,
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
                    flex: 1,
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
                    flex: 1,
                    text: 'Период замены',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'YearInstallation',
                    flex: 1,
                    text: 'Год установки',
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
                    dataIndex: 'YearExploitation',
                    flex: 1,
                    text: 'Год ввода в эксплуатацию',
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
                    flex: 1,
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
                    flex: 1,
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
                    flex: 1,
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
                    flex: 1,
                    text: 'Привод дверей кабины',
                    hidden: true,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeLiftMashineRoom',
                    flex: 1,
                    text: 'Расположение машинного помещения',
                    hidden: true,
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AvailabilityDevices',
                    flex: 1,
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
                    flex: 1,
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
                    flex: 1,
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
                    flex: 1,
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
                    flex: 1,
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
                    flex: 1,
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
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