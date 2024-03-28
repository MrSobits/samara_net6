Ext.define('B4.view.documentsgjiregister.TaskActionGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.actionisolated.DocumentRegistryTaskAction',
        'B4.enums.KindAction',
        'B4.enums.TypeBaseAction',
        'B4.ux.grid.filter.YesNo',
        'B4.ux.grid.column.YesNo',
        'B4.form.ComboBox'
    ],

    itemId: 'taskActionIsolatedGrid',
    title: 'Задания по КНМ',
    store: 'actionisolated.DocumentRegistryTaskAction',
    closable: true,

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
                                options.params.typeId = 'gji_document_task_actionisolated';
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
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeBaseAction',
                    width: 160,
                    text: 'Основание',
                    enumName: 'B4.enums.TypeBaseAction',
                    filter: true,
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspectors',
                    text: 'Инспекторы',
                    filter: { xtype: 'textfield' },
                    flex: 2
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'KindAction',
                    text: 'Вид мероприятия',
                    width: 160,
                    enumName: 'B4.enums.KindAction',
                    filter: true,
                    flex: 1.5
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
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    text: 'Адрес объекта',
                    filter: { xtype: 'textfield' },
                    flex: 2
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'CountOfHouses',
                    format: '0',
                    text: 'Количество домов',
                    filter: { 
                        xtype: 'numberfield',
                        hideTrigger: true,
                        operand: CondExpr.operands.eq
                    },
                    flex: 1
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
                    xtype: 'gridcolumn',
                    dataIndex: 'JurPerson',
                    text: 'Юридическое лицо',
                    filter: { xtype: 'textfield' },
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PhysicalPerson',
                    text: 'Физическое лицо',
                    filter: { xtype: 'textfield' },
                    flex: 1
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    text: 'Дата начала мероприятия',
                    format: 'd.m.Y',
                    filter: {
                        xtype: 'datefield' ,
                        format: 'd.m.Y'
                    },
                    flex: 1
                },
                {
                    xtype: 'yesnocolumn',
                    dataIndex: 'Done',
                    text: 'Выполнено',
                    filter: {xtype: 'b4dgridfilteryesno'},
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
                    store: me.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});