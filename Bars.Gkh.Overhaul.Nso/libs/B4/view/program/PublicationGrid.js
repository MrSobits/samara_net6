Ext.define('B4.view.program.PublicationGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.publicationproggrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.grid.toolbar.Paging',
        
        'B4.store.program.Publication',
        'B4.form.ComboBox',
        
        'B4.grid.feature.Summary',
        'B4.form.SelectField',
        'B4.store.dict.Municipality'
    ],

    title: 'Опубликованные программы',
    closable: true,

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
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 160,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    },
                    summaryRenderer: function () {
                        return Ext.String.format('Итого:');
                    }
                },
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
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    },
                    summaryType: 'sum',
                    summaryRenderer: function (value) {
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
                                    iconCls: 'icon-bin-empty',
                                    name: 'btnDeletePublishedProgram'
                                },
                                {
                                    xtype: 'datefield',
                                    labelAlign: 'right',
                                    labelWidth: 125,
                                    name: 'PublishDate',
                                    format: 'd.m.Y',
                                    fieldLabel: 'Дата опубликования',
                                    readOnly: true
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