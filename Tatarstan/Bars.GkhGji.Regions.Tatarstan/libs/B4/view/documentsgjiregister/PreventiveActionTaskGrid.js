Ext.define('B4.view.documentsgjiregister.PreventiveActionTaskGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.enums.PreventiveActionVisitType',
        'B4.enums.PreventiveActionType',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.preventiveaction.PreventiveActionTask'
    ],

    title: 'Задания по профилактическим мероприятиям',
    store: 'preventiveaction.PreventiveActionTask',
    itemId: 'docsGjiRegisterPreventiveActionTaskGrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            store: me.store,
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
                                options.params.typeId = 'gji_document_preventive_action_task';
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
                    flex: 1
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'ActionType',
                    text: 'Вид мероприятия',
                    enumName: 'B4.enums.PreventiveActionType',
                    flex: 1,
                    filter: true
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'VisitType',
                    text: 'Тип визита',
                    enumName: 'B4.enums.PreventiveActionVisitType',
                    flex: 1,
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    text: 'Номер документа',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield' ,
                        format: 'd.m.Y'
                    },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'ActionStartDate',
                    text: 'Дата начала мероприятия',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield' ,
                        format: 'd.m.Y'
                    },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Executor',
                    text: 'Ответственный за исполнение',
                    filter: { xtype: 'textfield' },
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