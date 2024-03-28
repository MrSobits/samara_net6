Ext.define('B4.view.efficiencyrating.analitics.ConstructorPanel', {
    extend: 'Ext.panel.Panel',
    requires: [
        'B4.ux.Highcharts',
        'B4.ux.Highcharts.SplineSerie',
        'B4.ux.Highcharts.AreaSplineSerie',
        'B4.ux.Highcharts.PieSerie',
        'B4.ux.Highcharts.ColumnSerie',

        'B4.form.SelectField',
        'B4.form.ComboBox',

        'B4.store.dict.EfficiencyRatingPeriod',
        'B4.store.dict.Municipality',

        'B4.view.efficiencyrating.analitics.GraphGrid'
    ],

    title: 'Конструктор',
    alias: 'widget.efanaliticsconstructorpanel',

    layout: { type: 'vbox', align: 'stretch' },
    bodyStyle: Gkh.bodyStyle,
    closable: true,
    border: false,
    bodyPadding: '0 5px 0 5px',

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    items: [
                        {
                            xtype: 'fieldset',
                            title: 'Основные параметры',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'container',
                                        layout: 'form',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            defaults: {
                                                labelWidth: 120,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Name',
                                                    fieldLabel: 'Название графика',
                                                    allowBlank: false
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    fieldLabel: 'Период',
                                                    textProperty: 'Name',
                                                    name: 'Periods',
                                                    store: 'B4.store.dict.EfficiencyRatingPeriod',
                                                    allowBlank: false,
                                                    editable: false,
                                                    selectionMode: 'MULTI',
                                                    listeners: { windowcreated: me.onWindowCreated },
                                                    columns: [
                                                        {
                                                            xtype: 'gridcolumn',
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
                                                            width: 200,
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
                                                            width: 200,
                                                            text: 'Дата окончания',
                                                            filter: {
                                                                xtype: 'datefield',
                                                                operand: CondExpr.operands.eq
                                                            }
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'b4combobox',
                                                    name: 'AnaliticsLevel',
                                                    fieldLabel: 'Детализация',
                                                    allowBlank: false,
                                                    editable: false,
                                                    items: B4.enums.efficiencyrating.AnaliticsLevel.getItems()
                                                },
                                                {
                                                    xtype: 'b4combobox',
                                                    name: 'ViewParam',
                                                    fieldLabel: 'Отображать по',
                                                    allowBlank: false,
                                                    editable: false,
                                                    items: B4.enums.efficiencyrating.ViewParam.getItems()
                                                }
                                            ]
                                        },
                                        {
                                            defaults: {
                                                labelWidth: 200,
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4combobox',
                                                    name: 'Category',
                                                    fieldLabel: 'Категория',
                                                    allowBlank: false,
                                                    editable: false,
                                                    items: B4.enums.efficiencyrating.Category.getItems()
                                                },
                                                {
                                                    xtype: 'b4combobox',
                                                    name: 'FactorCode',
                                                    fieldLabel: 'Фактор',
                                                    fields: ['Name', 'Code'],
                                                    url: '/ManagingOrganizationEfficiencyRating/GetFactors',
                                                    valueField: 'Code',
                                                    displayField: 'Name',
                                                    editable: false
                                                },
                                                {
                                                    xtype: 'b4selectfield',
                                                    name: 'Municipalities',
                                                    fieldLabel: 'Муниципальные районы',
                                                    selectionMode: 'MULTI',
                                                    idProperty: 'Id',
                                                    textProperty: 'Name',
                                                    store: 'B4.store.dict.Municipality',
                                                    listeners: { windowcreated: me.onWindowCreated },
                                                    editable: false,
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
                                                    textProperty: 'ManagingOrganization',
                                                    name: 'ManagingOrganizations',
                                                    fieldLabel: 'Управляющие организации',
                                                    selectionMode: 'MULTI',
                                                    store: 'B4.store.efficiencyrating.ManagingOrganizationGrid',
                                                    editable: false,
                                                    disabled: true,
                                                    listeners: {
                                                        windowcreated: me.onWindowCreated,
                                                        beforeload: function (sf, operation, store) {
                                                            var municipalities = sf.up().down('b4selectfield[name=Municipalities]').getValue();
                                                            operation.params.municipalityIds = Ext.encode(municipalities);
                                                        }
                                                    },
                                                    columns: [
                                                        {
                                                            dataIndex: 'ManagingOrganization',
                                                            flex: 1,
                                                            text: 'Наименование',
                                                            filter: {
                                                                xtype: 'textfield',
                                                                maxLength: 255
                                                            }
                                                        },
                                                        {
                                                            dataIndex: 'Inn',
                                                            flex: 1,
                                                            text: 'ИНН',
                                                            filter: {
                                                                xtype: 'textfield',
                                                                maxLength: 255
                                                            }
                                                        },
                                                        {
                                                            dataIndex: 'Kpp',
                                                            flex: 1,
                                                            text: 'КПП',
                                                            filter: {
                                                                xtype: 'textfield',
                                                                maxLength: 255
                                                            }
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    name: 'GraphContainer',
                    flex: 1,
                    layout: {type: 'vbox', align: 'stretch' },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'container',
                                    name: 'ChartButtons',
                                    defaults: {
                                        margin: '0 3px 0 0',
                                        scale: 'large'
                                    },
                                    items: [
                                        {
                                            xtype: 'button',
                                            icon: B4.Url.content('content/img/charts/barchart.png'),
                                            chart: {
                                                type: 'column'
                                            }
                                        },
                                        {
                                            xtype: 'button',
                                            icon: B4.Url.content('content/img/charts/logarithmicchart.png'),
                                            chart: {
                                                type: 'areaspline'
                                            }
                                        },
                                        {
                                            xtype: 'button',
                                            icon: B4.Url.content('content/img/charts/linediagram.png'),
                                            chart: {
                                                type: 'spline'
                                            }
                                        },
                                        {
                                            xtype: 'button',
                                            icon: B4.Url.content('content/img/charts/piediagram.png'),
                                            chart: {
                                                type: 'pie'
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'tbfill'
                                },
                                {
                                    xtype: 'container',
                                    defaults: {
                                        scale: 'large'
                                    },
                                    items: [
                                        {
                                            xtype: 'button',
                                            text: 'Построить график',
                                            actionName: 'buildgraph',
                                            margin: '0 5px 0 0'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Сохранить график',
                                            actionName: 'savegraph'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            name: 'Highcharts',
                            margin: '10px 0 5px 0',
                            flex: 1,
                            layout: { type: 'hbox', align: 'stretch' },
                            items: []
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    onWindowCreated: function (control, window) {
        var selectAll = window.down('[text=Выбрать все]');
        if (selectAll) {
            selectAll.hide();
        }
    }
});