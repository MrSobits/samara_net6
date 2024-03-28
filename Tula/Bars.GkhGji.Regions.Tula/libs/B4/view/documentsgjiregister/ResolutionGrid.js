Ext.define('B4.view.documentsgjiregister.ResolutionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.GridStateColumn',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.YesNo',
        'B4.enums.TypeBase'
    ],

    title: 'Постановления',
    store: 'view.Resolution',
    itemId: 'docsGjiRegisterResolutionGrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        var renderer = function (val, meta, rec) {

            var deliveryDate = rec.get('DeliveryDate');
            var paided = rec.get('Paided');

            var deltaDate = ((new Date()).getTime() - (new Date(deliveryDate)).getTime()) / (1000 * 60 * 60 * 24);
            if (deliveryDate && deltaDate > 30 && paided == 20) {
                meta.style = 'background: red;';
                meta.tdAttr = 'data-qtip="Истек срок оплаты штрафа"';

            }
            else if (deliveryDate && deltaDate > 10 && paided == 20) {
                meta.style = 'background: yellow;';
                meta.tdAttr = 'data-qtip="Истекает срок оплаты штрафа"';
            }

            return val;
        };

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
                                options.params.typeId = 'gji_document_resol';
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
                    scope: this,
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeBase',
                    width: 150,
                    text: 'Основание проверки',
                    renderer: function (val, meta, rec) {
                        val = renderer(val, meta, rec);
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
                    dataIndex: 'OfficialName',
                    flex: 1,
                    text: 'ДЛ вынесшее постановление',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityId',
                    flex: 1,
                    text: 'Муниципальное образование',
                    renderer: function (val, meta, rec) {
                        renderer(val, meta, rec);
                        return rec.get('MunicipalityNames');
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
                    dataIndex: 'TypeExecutant',
                    flex: 1,
                    text: 'Тип исполнителя',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    flex: 1,
                    text: 'Организация',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    width: 80,
                    text: 'Номер документа',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
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
                    },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    width: 80,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    renderer: function (val, meta, rec) {
                        renderer(val, meta, rec);
                        return Ext.Date.format(new Date(val), 'd.m.Y');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sanction',
                    flex: 1,
                    text: 'Вид санкции',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'BecameLegal',
                    width: 120,
                    text: 'Вступило в законную силу',
                    trueText: 'Да',
                    falseText: 'Нет',
                    filter: {
                        xtype: 'b4combobox',
                        editable: false,
                        store: B4.enums.YesNo.getStore(),
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display',
                        listeners: {
                            storeloaded: {
                                fn: function (me1) {
                                    me1.getStore().removeAll();
                                    me1.getStore().insert(0, { Value: null, Display: '-' });
                                    me1.getStore().insert(1, { Value: false, Display: 'Нет' });
                                    me1.getStore().insert(2, { Value: true, Display: 'Да' });

                                }
                            }
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PenaltyAmount',
                    width: 70,
                    text: 'Сумма штрафа',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SumPays',
                    width: 70,
                    text: 'Оплачено штрафов',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
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