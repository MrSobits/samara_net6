﻿Ext.define('B4.view.baseprosclaim.MainPanel', {
    extend: 'B4.ux.grid.Panel',
    closable: true,
    title: 'Проверки по требованию прокуратуры',
    alias: 'widget.baseProsClaimPanel',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',

        'B4.form.ComboBox',
        'B4.form.GridStateColumn',

        'B4.enums.PersonInspection',
        'B4.enums.TypeJurPerson'
    ],

    store: 'BaseProsClaim',
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

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
                    text: 'Статус',
                    menuText: 'Статус',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_inspection';
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
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', view.getStore().getAt(recordIndex));
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'Муниципальный район',
                    filter: {
                        xtype: 'b4combobox',
                        //operand: CondExpr.operands.eq,
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
                    dataIndex: 'ContragentName',
                    flex: 2,
                    text: 'Юридическое лицо',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonInspection',
                    flex: 1,
                    text: 'Объект проверки',
                    filter: {
                        xtype: 'b4combobox',
                        storeAutoLoad: false,
                        hideLabel: true,
                        items: B4.enums.PersonInspection.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    },
                    renderer: function (val) {
                        return B4.enums.PersonInspection.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeJurPerson',
                    flex: 1.5,
                    text: 'Тип контрагента',
                    filter: {
                        xtype: 'b4combobox',
                        storeAutoLoad: false,
                        hideLabel: true,
                        items: B4.enums.TypeJurPerson.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    },
                    renderer: function (val) {
                        return B4.enums.TypeJurPerson.displayRenderer(val);
                    }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ProsClaimDateCheck',
                    text: 'Дата проверки',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObjectCount',
                    text: 'Количество домов',
                    flex: 1,
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectionNumber',
                    text: 'Номер',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectorNames',
                    text: 'Инспекторы',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                            columns: 3,
                            title: 'Действия',
                            defaults: {
                                margin: 3
                            },
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Экспорт',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                },
                                {
                                    itemId: 'updateGrid',
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            title: 'Фильтры',
                            defaults: {
                                margin: 3
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    labelWidth: 10,
                                    width: 110,
                                    fieldLabel: 'с',
                                    itemId: 'dfDateStart',
                                    value: new Date(new Date().getFullYear(), 0, 1)
                                },
                                {
                                    xtype: 'datefield',
                                    labelWidth: 12,
                                    width: 110,
                                    fieldLabel: 'по',
                                    itemId: 'dfDateEnd',
                                    value: new Date(new Date().setDate(new Date().getDate() + 7))
                                },
                                {
                                    xtype: 'checkbox',
                                    itemId: 'cbShowCloseInspections',
                                    boxLabel: 'Показать закрытые проверки',
                                    checked: false,
                                    fieldStyle: 'vertical-align: -4px !important;'
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
