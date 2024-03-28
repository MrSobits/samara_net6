Ext.define('B4.view.taskcalendar.PrescriptionGrid', {
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
    store: 'taskcalendar.ListPrescription',
    alias: 'widget.taskcalendarprescriptiongrid',
    closable: false,
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
                                    xtype: 'b4updatebutton'
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