Ext.define('B4.view.preventiveaction.PreventiveActionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',

        'B4.form.GridStateColumn',
        'B4.form.SelectField',
        'B4.form.EnumCombo',

        'B4.enums.PreventiveActionType',
        'B4.enums.PreventiveActionVisitType',
        
        'B4.store.preventiveaction.PreventiveAction'
    ],

    alias: 'widget.preventiveactiongrid',
    title: 'Профилактические мероприятия',
    closable: true,
    
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.preventiveaction.PreventiveAction');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 200,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'gji_document_preventive_action';
                            },
                            storeloaded: {
                                fn: function (field) {
                                    field.getStore().insert(0, { Id: null, Name: '-' });
                                }
                            }
                        }
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    text: 'Муниципальное образование',
                    flex: 2,
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
                    dataIndex: 'DocumentNumber',
                    text: 'Номер',
                    flex: 1,
                    filter: { xtype: 'textfield' }
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
                    xtype: 'datecolumn',
                    text: 'Дата начала проведения мероприятия',
                    dataIndex: 'DocumentDate',
                    format: 'd.m.Y',
                    flex: 1,
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ControlledOrganization',
                    text: 'Контролируемое лицо',
                    flex: 2,
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
                            xtype: 'container',
                            layout: 'vbox',
                            defaults: {
                                xtype: 'container',
                                layout: {
                                    type: 'hbox',
                                    align: 'middle'
                                },
                                width: 600,
                                labelWidth: 110
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.PlanActionGji',
                                    name: 'Plan',
                                    fieldLabel: 'План',
                                    editable: false,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                    ],
                                    margin: '7 0 7 2'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Вид мероприятия',
                                    enumName: 'B4.enums.PreventiveActionType',
                                    name: 'ActionType',
                                    margin: '0 0 0 2',
                                    includeNull: true
                                },
                                {
                                    margin: '0 0 0 2',
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'PeriodStart',
                                            fieldLabel: 'Период проведения</br>мероприятия с',
                                            labelWidth: 110,
                                            editable: false,
                                            width: 335
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'PeriodEnd',
                                            fieldLabel: 'по',
                                            labelWidth: 20,
                                            editable: false,
                                            width: 258,
                                            margin: '0 0 0 7'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    editable: false,
                                    name: 'ControlledOrganization',
                                    fieldLabel: 'Контролируемое лицо',
                                    store: 'B4.store.Contragent',
                                    columns: [
                                        { 
                                            header: 'Наименование', 
                                            xtype: 'gridcolumn', 
                                            dataIndex: 'ShortName', 
                                            flex: 1, 
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            header: 'Муниципальное образование',
                                            xtype: 'gridcolumn',
                                            dataIndex: 'Municipality',
                                            flex: 1,
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            header: 'ИНН',
                                            xtype: 'gridcolumn',
                                            dataIndex: 'Inn',
                                            flex: 1,
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            header: 'КПП',
                                            xtype: 'gridcolumn',
                                            dataIndex: 'Kpp',
                                            flex: 1,
                                            filter: { xtype: 'textfield' }
                                        }
                                    ],
                                    margin: '0 0 3 2'
                                },
                                {
                                    width: 600,
                                    items: [
                                        {
                                            xtype: 'buttongroup',
                                            columns: 3,
                                            items: [
                                                {
                                                    xtype: 'b4addbutton'
                                                },
                                                {
                                                    xtype: 'b4updatebutton'
                                                },
                                                {
                                                    xtype: 'button',
                                                    itemId: 'btnExport',
                                                    action: 'downloadExcel',
                                                    text: 'Выгрузить в Excel'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'checkbox',
                                            boxLabel: '<span style="vertical-align: 2px">Показать закрытые мероприятия</span>',
                                            name: 'ShowClosed',
                                            margin: '0 0 0 5'
                                        }
                                    ]
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