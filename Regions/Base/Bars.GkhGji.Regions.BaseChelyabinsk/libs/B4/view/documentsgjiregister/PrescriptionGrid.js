Ext.define('B4.view.documentsgjiregister.PrescriptionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.GridStateColumn',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.TypePrescriptionExecution',
        'B4.ux.grid.column.Enum',
        'B4.enums.ControlType',
        'B4.enums.TypeBase'
    ],

    title: 'Предписания',
    store: 'view.Prescription',
    itemId: 'docsGjiRegisterPrescriptionGrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        var renderer = function (val, meta, rec) {

            var dateRemoval = rec.get('DateRemoval');
            var existDisposal = rec.get('DisposalId');
            var HasNotRemoovedViolations = rec.get('HasNotRemoovedViolations');

            var deltaDate = ((new Date(dateRemoval)).getTime() - (new Date()).getTime()) / (1000 * 60 * 60 * 24);
            if (HasNotRemoovedViolations) {
                if (deltaDate >= 0 && deltaDate <= 4) {
                    meta.style = 'background: yellow;';
                    meta.tdAttr = 'data-qtip="Истекает срок устранения нарушений"';
                }

                if (deltaDate < 0 && !existDisposal) {
                    meta.style = 'background: red;';
                    meta.tdAttr = 'data-qtip="Необходима проверка исполнения предписания"';
                }
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
                                options.params.typeId = 'gji_document_prescr';
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
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.PrescriptionState',
                    dataIndex: 'PrescriptionState',
                    text: 'Статус предписания',
                    filter: true,
                    flex: 1
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
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.ControlType',
                    dataIndex: 'ControlType',
                    text: 'Вид контроля',
                    filter: true,
                    flex: 1
                },
                //
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AppealNumber',
                    flex: 0.5,
                    text: 'Номер обращения',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'AppealDate',
                    text: 'Дата обращения',
                    flex: 0.5,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq },
                    renderer: function (val, meta, rec) {
                        renderer(val, meta, rec);
                        if (!val) {
                            return null;
                        }
                        return Ext.Date.format(new Date(val), 'd.m.Y');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AppealDescription',
                    flex: 1,
                    text: 'Содержание',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                //
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectorNames',
                    flex: 1,
                    text: 'Инспекторы',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'MunicipalityNames',
                    width: 160,
                    text: 'Муниципальный район',
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
                    dataIndex: 'MoSettlement',
                    width: 160,
                    text: 'Муниципальное образование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlaceName',
                    width: 160,
                    text: 'Населенный пункт',
                    filter: { xtype: 'textfield' }
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
                    dataIndex: 'ViolationList',
                    flex: 1,
                    text: 'НПД нарушений',
                    filter: { xtype: 'textfield' },
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountRealityObject',
                    width: 70,
                    text: 'Количество домов',
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
                    dataIndex: 'DocumentNumber',
                    width: 75,
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
                        if (!val) {
                            return null;
                        }
                        return Ext.Date.format(new Date(val), 'd.m.Y');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CountViolation',
                    width: 75,
                    text: 'Количество нарушений',
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
                    dataIndex: 'DateRemoval',
                    name: 'DateRemoval',
                    text: 'Срок исполнения предписания',
                    width: 80,
                    filter: {
                        xtype: 'datefield',
                        operand: CondExpr.operands.eq
                    },
                    renderer: function (val, meta, rec) {
                        renderer(val, meta, rec);
                        if (!val) {
                            return null;
                        }
                        return Ext.Date.format(new Date(val), 'd.m.Y');
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.TypePrescriptionExecution',
                    dataIndex: 'TypePrescriptionExecution',
                    text: 'Статус исполнения',
                    filter: true,
                    flex: 1
                },
                
                    
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                },
                                {
                                    xtype: 'checkbox',
                                    labelWidth: 150,
                                    fieldLabel: 'Показать истекающие',
                                    labelAlign: 'right',
                                    itemId: 'showYellow'
                                },
                                {
                                    xtype: 'checkbox',
                                    labelWidth: 150,
                                    labelAlign: 'right',
                                    fieldLabel: 'Показать просроченные',
                                    itemId: 'showRed'
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