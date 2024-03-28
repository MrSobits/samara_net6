Ext.define('B4.view.documentsgjiregister.PreventiveActionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.ComboBox',
        'B4.form.GridStateColumn',

        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',

        'B4.enums.PreventiveActionType',
        'B4.enums.PreventiveActionVisitType',

        'B4.store.preventiveaction.DocumentRegistryPreventiveAction',
    ],

    itemId: 'docsGjiRegisterPreventiveActionGrid',
    title: 'Профилактические мероприятия',
    store: 'preventiveaction.DocumentRegistryPreventiveAction',

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
                    text: 'Статус',
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function(field, store, options) {
                                options.params.typeId = 'gji_document_preventive_action';
                            },
                            storeloaded: {
                                fn: function(me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    },
                    processEvent: function(type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    flex: 1.5,
                    scope: me
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
                    xtype: 'gridcolumn',
                    dataIndex: 'ControlType',
                    text: 'Вид контроля',
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/ControlType/ListWithoutPaging'
                    },
                    flex: 1.5
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'ActionType',
                    width: 160,
                    text: 'Вид мероприятия',
                    enumName: 'B4.enums.PreventiveActionType',
                    filter: true,
                    flex: 1
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'VisitType',
                    width: 160,
                    text: 'Тип визита',
                    enumName: 'B4.enums.PreventiveActionVisitType',
                    filter: true,
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ControlledOrganization',
                    text: 'Юридическое лицо',
                    filter: { xtype: 'textfield' },
                    flex: 2
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
                    text: 'Дата мероприятия',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield' ,
                        format: 'd.m.Y'
                    },
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspectors',
                    text: 'Инспекторы',
                    filter: { xtype: 'textfield' },
                    flex: 2
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
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});