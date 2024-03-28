Ext.define('B4.view.documentsgjiregister.DecisionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.GridStateColumn',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo',
        'B4.form.ComboBox',
        
        'B4.enums.TypeBase',
        'B4.enums.TypeAgreementProsecutor',
    ],

    title: 'Решения',
    store: 'view.DecisionGji',
    itemId: 'docsGjiRegisterDecisionGrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        //поскольку требуется чтобы в енуме Тип Основания небыло постановления прокуратуры
        //то получаем енум и формируем из его item-ов новый енум но без постановления прокуратуры
        var currTypeBase = B4.enums.TypeBase.getItemsWithEmpty([null, '-']);
        var newTypeBase = [];

        Ext.iterate(currTypeBase, function (val, key) {
            if (key != 6)
                newTypeBase.push(val);
        });
        
        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Id',
                    flex: 2,
                    text: 'Id',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    menuText: 'Статус',
                    text: 'Статус',
                    width: 125,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_document_decision';
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
                    dataIndex: 'TypeBase',
                    width: 160,
                    text: 'Основание проверки',
                    renderer: function (val) {
                        if (val != 60)
                            return B4.enums.TypeBase.displayRenderer(val);
                        return '';
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: newTypeBase,
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectorNames',
                    flex: 2,
                    text: 'Инспекторы',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'KindCheck',
                    flex: 2,
                    text: 'Вид решения',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        url: '/KindCheckGji/List',
                        editable: false,
                        storeAutoLoad: false,
                        emptyItem: { Name: '-' },
                        valueField: 'Name'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityId',
                    flex: 2,
                    text: 'Муниципальное образование',
                    renderer: function (value, metaData, record) {
                        return record.get('MunicipalityNames');
                    },
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    flex: 2,
                    text: 'Юридическое лицо',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObjectCount',
                    width: 65,
                    text: 'Количество домов',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    width: 65,
                    text: 'Номер документа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    width: 50,
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    width: 70,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq}
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    flex: 1,
                    text: 'Начало обследования',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq}
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    flex: 1,
                    text: 'Окончание обследования',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq}
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'IsActCheckExist',
                    width: 65,
                    text: 'Выполнено',
                    trueText: 'Да',
                    falseText: 'Нет', 
                    filter: { xtype: 'b4dgridfilteryesno' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeAgreementProsecutor',
                    text: 'Согласовано с прокуратурой',
                    hidden: true,
                    flex: 1,
                    renderer: function(val) {
                        return B4.enums.TypeAgreementProsecutor.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'combobox',
                        store: B4.enums.TypeAgreementProsecutor.getItemsWithEmpty([null, '-']),
                        operand: CondExpr.operands.eq,
                        editable: false
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
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
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
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});