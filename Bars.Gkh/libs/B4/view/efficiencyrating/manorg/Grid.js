Ext.define('B4.view.efficiencyrating.manorg.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.dict.EfficiencyRatingPeriod',
        'B4.model.dict.EfficiencyRatingPeriod'
    ],

    title: 'Рейтинг эффективности управляющих организаций',
    alias: 'widget.efficiencyratingManorgGrid',
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.efficiencyrating.ManagingOrganizationGrid');

        Ext.applyIf(me,
        {
            store: store,
            columnLines: true,
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
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
                        valueField: 'Id',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    },
                    renderer: function(val) {
                        return val ? val.Name : null;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ManagingOrganization',
                    flex: 1,
                    text: 'Управляющая организация',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inn',
                    width: 150,
                    text: 'ИНН',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Kpp',
                    width: 150,
                    text: 'КПП',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Ogrn',
                    width: 150,
                    text: 'ОГРН',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Rating',
                    width: 150,
                    sortable: false,
                    text: 'Показатель эффективности',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : 0;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Dynamics',
                    width: 150,
                    sortable: false,
                    text: 'Динамика',
                    renderer: function (val) {
                        return val ? Ext.util.Format.currency(val) : 0;
                    }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'container',
                            layout: 'vbox',
                            margin: '10px, 0',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    items: [
                                        {
                                            xtype: 'b4updatebutton',
                                            margin: '0 10px 5px 10px'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Рассчитать показатели',
                                            actionName: 'calcvalues',
                                            margin: '0 10px 5px 0'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'tbfill',
                                    height: 27
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'vbox',
                            margin: '10px, 0',
                            defaults: {
                                width: 400,
                                labelWidth: 130,
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Municipality',
                                    fieldLabel: 'Муниципальные районы',
                                    editable: false,
                                    selectionMode: 'MULTI',
                                    idProperty: 'Id',
                                    textProperty: 'Name',
                                    store: 'B4.store.dict.Municipality',
                                    columns: [
                                        {
                                            dataIndex: 'Name',
                                            flex: 1,
                                            text: 'Наименование',
                                            filter: {
                                                xtype: 'textfield',
                                                maxLength: 255
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'EfficiencyRatingPeriod',
                                    fieldLabel: 'Период',
                                    editable: false,
                                    idProperty: 'Id',
                                    textProperty: 'Name',
                                    allowBlank: false,
                                    store: 'B4.store.dict.EfficiencyRatingPeriod',
                                    columns: [
                                        {
                                            dataIndex: 'Name',
                                            flex: 1,
                                            text: 'Период',
                                            filter: {
                                                xtype: 'textfield',
                                                maxLength: 255
                                            }
                                        },
                                        {
                                            xtype: 'datecolumn',
                                            format: 'd.m.Y',
                                            dataIndex: 'DateStart',
                                            width: 100,
                                            text: 'Дата начала',
                                            filter: {
                                                xtype: 'datefield',
                                                operand: CondExpr.operands.eq
                                            }
                                        },
                                        {
                                            xtype: 'datecolumn',
                                            format: 'd.m.Y',
                                            dataIndex: 'DateEnd',
                                            width: 100,
                                            text: 'Дата окончания',
                                            filter: {
                                                xtype: 'datefield',
                                                operand: CondExpr.operands.eq
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
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});