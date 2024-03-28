Ext.define('B4.view.program.PublicationGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.publicationproggrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.view.program.PublicProgramPagingToolbar',
        
        'B4.store.program.Publication',
        'B4.form.ComboBox',
        
        'B4.grid.feature.Summary',
        'B4.form.SelectField',
        'B4.store.dict.Municipality',
        'B4.store.dict.municipality.ByParam'
    ],

    title: 'Опубликованные программы',
    //closable: true,

    features: [{
        ftype: 'b4_summary'
    }],
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.program.Publication', {
                autoLoad: false
            });

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndexNumber',
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq,
                        allowDecimals: false
                    }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'Municipality',
                //    width: 160,
                //    text: 'Муниципальное образование',
                //    filter: {
                //        xtype: 'b4combobox',
                //        operand: CondExpr.operands.eq,
                //        storeAutoLoad: false,
                //        hideLabel: true,
                //        editable: false,
                //        valueField: 'Name',
                //        emptyItem: { Name: '-' },
                //        url: '/Municipality/ListWithoutPaging'
                //    },
                //    summaryRenderer: function() {
                //        return Ext.String.format('Итого:');
                //    }
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateobject',
                    flex: 1,
                    text: 'ООИ',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Сумма',
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 150,
                    renderer: function(value) {
                        return Ext.util.Format.currency(value);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function(value) {
                        return Ext.util.Format.currency(value);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PublishedYear',
                    text: 'Год публикации',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 120
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
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            flex: 1,
                            items: [
                                {
                                    xtype: 'buttongroup',
                                    items: [
                                        {
                                            xtype: 'button',
                                            itemId: 'btnState',
                                            iconCls: 'icon-accept',
                                            text: 'Статус',
                                            menu: []
                                        },
                                        {
                                            xtype: 'b4updatebutton'
                                        },
                                        {
                                            xtype: 'button',
                                            iconCls: 'icon-table-go',
                                            text: 'Экспорт',
                                            textAlign: 'left',
                                            itemId: 'btnExport'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Подписать',
                                            iconCls: 'icon-accept',
                                            actionName: 'sign'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Удалить опубликованную программу',
                                            iconCls: 'icon-accept',
                                            actionName: 'delete'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    flex:1,
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'combobox',
                                            store: Ext.create('B4.store.dict.municipality.ByParam', {remoteFilter: false}),
                                            name: 'Municipality',
                                            fieldLabel: 'Муниципальное образование',
                                            labelWidth: 175,
                                            width: 425,
                                            margin: '5 0',
                                            labelAlign: 'right',
                                            valueField: 'Id',
                                            displayField: 'Name',
                                            listeners: {
                                                change: function (cmp, newValue) {
                                                    if (cmp.store.isLoading()) {
                                                        return;
                                                    }

                                                    cmp.store.clearFilter();
                                                    if (!Ext.isEmpty(newValue)) {
                                                        cmp.store.filter({
                                                            property: 'Name',
                                                            anyMatch: true,
                                                            exactMatch: false,
                                                            caseSensitive: false,
                                                            value: newValue
                                                        });
                                                    }
                                                }
                                            }
                                        },
                                        {
                                            xtype: 'datefield',
                                            labelAlign: 'right',
                                            labelWidth: 300,
                                            margin: '5 0',
                                            maxWidth: 1000,
                                            format: 'd.m.Y',
                                            name: 'PublishDate',
                                            fieldLabel: 'Дата опубликования',
                                            readOnly: true
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'publicprogrampagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
                ,
                {
                    xtype: 'toolbar',
                    dock: 'bottom',
                    layout: {
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'fieldset',
                            defaults: {
                                labelWidth: 180,
                                anchor: '100%',
                                labelAlign: 'right'
                            },
                            title: 'Текущее МО : Все МО',
                            items: [
                                {
                                    xtype: 'fieldset',
                                    title: 'Количество домов',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '-15 0 0 0',
                                            border: false,
                                            width: 1200,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                readOnly: true
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 140,
                                                    fieldLabel: 'Основная программа',
                                                    width: 190,
                                                    itemId: 'tfMainProg'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 10,
                                                    fieldLabel: ' ',
                                                    width: 60,
                                                    itemId: 'tfAllMainProg'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 110,
                                                    fieldLabel: 'Подпрограмма',
                                                    width: 160,
                                                    itemId: 'tfSubProg'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 10,
                                                    fieldLabel: ' ',
                                                    width: 60,
                                                    itemId: 'tfAllSubProg'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 100,
                                                    fieldLabel: 'Всего',
                                                    width: 150,
                                                    itemId: 'tfHousesCount'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 10,
                                                    fieldLabel: ' ',
                                                    width: 60,
                                                    itemId: 'tfAllHousesCount'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Количество ООИ',
                                    items: [
                                        {
                                            xtype: 'container',
                                            border: false,
                                            width: 1200,
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                readOnly: true
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 140,
                                                    fieldLabel: 'Лифтовое хозяйство',
                                                    width: 190,
                                                    itemId: 'tfLift'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 10,
                                                    fieldLabel: ' ',
                                                    width: 60,
                                                    itemId: 'tfAllLift'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 110,
                                                    fieldLabel: 'Водоотведение',
                                                    width: 160,
                                                    itemId: 'tfWaterO'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 10,
                                                    fieldLabel: ' ',
                                                    width: 60,
                                                    itemId: 'tfAllWaterO'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 100,
                                                    fieldLabel: 'Водоснабжение',
                                                    width: 150,
                                                    itemId: 'tfWaterS'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 10,
                                                    fieldLabel: ' ',
                                                    width: 60,
                                                    itemId: 'tfAllWaterS'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 70,
                                                    fieldLabel: 'Крыша',
                                                    width: 120,
                                                    itemId: 'tfRoof'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 10,
                                                    fieldLabel: ' ',
                                                    width: 60,
                                                    itemId: 'tfAllRoof'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 120,
                                                    fieldLabel: 'Газоснабжение',
                                                    width: 170,
                                                    itemId: 'tfGas'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 10,
                                                    fieldLabel: ' ',
                                                    width: 60,
                                                    itemId: 'tfAllGas'
                                                }

                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            width: 1200,
                                            margin: '10 0 0 0',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                readOnly: true
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 140,
                                                    fieldLabel: 'Подвальные помещения',
                                                    width: 190,
                                                    itemId: 'tfPodv'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 10,
                                                    fieldLabel: ' ',
                                                    width: 60,
                                                    itemId: 'tfAllPodv'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 110,
                                                    fieldLabel: 'Теплоснабжение',
                                                    width: 160,
                                                    itemId: 'tfWarm'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 10,
                                                    fieldLabel: ' ',
                                                    width: 60,
                                                    itemId: 'tfAllWarm'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 100,
                                                    fieldLabel: 'Фундамент',
                                                    width: 150,
                                                    itemId: 'tfFundam'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 10,
                                                    fieldLabel: ' ',
                                                    width: 60,
                                                    itemId: 'tfAllFundam'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 70,
                                                    fieldLabel: 'Фасад',
                                                    width: 120,
                                                    itemId: 'tfFas'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 10,
                                                    fieldLabel: ' ',
                                                    width: 60,
                                                    itemId: 'tfAllFas'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 120,
                                                    fieldLabel: 'Электроснабжение',
                                                    width: 170,
                                                    itemId: 'tfElect'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    labelWidth: 10,
                                                    fieldLabel: ' ',
                                                    width: 60,
                                                    itemId: 'tfAllElect'
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
        });

        me.callParent(arguments);
    }
});