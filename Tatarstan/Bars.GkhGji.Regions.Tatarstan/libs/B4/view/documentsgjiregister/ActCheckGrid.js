﻿Ext.define('B4.view.documentsgjiregister.ActCheckGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypeBase',
        'B4.ux.grid.column.Enum',
        'B4.enums.YesNo'
    ],

    title: 'Акты проверок',
    store: 'view.ActCheck',
    itemId: 'docsGjiRegisterActCheckGrid',
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
                                options.params.typeId = 'gji_document_actcheck';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
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
                    width: 150,
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
                    text: 'Инспектор',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityNames',
                    flex: 2,
                    text: 'Муниципальное образование',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonInspectionAddress',
                    flex: 2,
                    text: 'Адрес объекта проверки',
                    filter: { xtype: 'textfield' }
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
                    width: 70,
                    text: 'Количество домов',
                    sortable: false,
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
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    width: 80
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNo',
                    text: 'Нарушения выявлены',
                    dataIndex: 'HasViolation',
                    width: 70,
                    filter: true,
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountExecDoc',
                    flex: 1,
                    text: 'Количество исполнительных документов',
                    sortable: false,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
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
                                    text: 'Выгрузка в Excel',
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