Ext.define('B4.view.inspectionactionisolated.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.GridStateColumn',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.inspectionactionisolated.InspectionActionIsolated',
        'B4.enums.TatarstanInspectionFormType',
        'B4.enums.TypeObjectAction',
        'B4.enums.TypeJurPerson',
        'B4.ux.grid.filter.YesNo',
        'B4.form.ComboBox'
    ],

    alias: 'widget.inspectionactionisolatedgrid',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.inspectionactionisolated.InspectionActionIsolated');

        me.relayEvents(store, ['beforeload'], 'inspectionactionisolated.');

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
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function(field, store, options) {
                                options.params.typeId = 'gji_inspection';
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
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    text: 'Муниципальное образование',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.icontains,
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
                    dataIndex: 'Address',
                    text: 'Адрес дома',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'JurPerson',
                    text: 'Юридическое лицо',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeObject',
                    text: 'Объект проверки',
                    flex: 1,
                    enumName: 'B4.enums.TypeObjectAction',
                    filter: true
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeJurPerson',
                    text: 'Тип контрагента',
                    flex: 1,
                    enumName: 'B4.enums.TypeJurPerson',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectionNumber',
                    text: 'Номер',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'CheckDate',
                    text: 'Дата проверки',
                    flex: 1,
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inspectors',
                    text: 'Инспекторы',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeForm',
                    text: 'Форма проверки',
                    flex: 1,
                    enumName: 'B4.enums.TatarstanInspectionFormType',
                    filter: true
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
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-table-go',
                                    text: 'Выгрузка в Excel',
                                    textAlign: 'left',
                                    itemId: 'btnExport'
                                },
                                {
                                    margin: '0 0 0 10',
                                    xtype: 'checkbox',
                                    name: 'IsClosed',
                                    boxLabel: 'Показать закрытые проверки',
                                    width: 200
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