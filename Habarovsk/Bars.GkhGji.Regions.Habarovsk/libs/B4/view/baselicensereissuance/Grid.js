Ext.define('B4.view.baselicensereissuance.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.baselicensereissuancegrid',

    requires: [
        'B4.ux.button.Add',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.filter.YesNo',

        'B4.form.ComboBox',
        'B4.form.GridStateColumn',

        'B4.enums.PersonInspection',
        'B4.enums.TypeJurPerson',
        'B4.store.BaseLicenseReissuance'
    ],

    store: 'BaseLicenseReissuance',
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
                    width: 175,
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
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    text: 'Муниципальный район',
                    flex: 0.5,
                    filter: {
                        xtype: 'b4combobox',
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
                    dataIndex: 'ContragentInn',
                    text: 'ИНН лицензиата',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                  {
                      xtype: 'gridcolumn',
                      dataIndex: 'ContragentName',
                      text: 'Лицензиат',
                      flex: 1,
                      filter: { xtype: 'textfield' }
                  },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'ContragentInn',
                     text: 'ИНН лицензиата',
                     flex: 1,
                     filter: { xtype: 'textfield' }
                 },
           
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RegisterNum',
                    text: 'Номер обращения',
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
                            xtype: 'b4addbutton'
                        },
                        {
                            xtype: 'button',
                            iconCls: 'icon-table-go',
                            text: 'Экспорт',
                            textAlign: 'left',
                            action: 'bExport'
                        },
                        {
                            xtype: 'checkbox',
                            itemId: 'cbShowCloseInspections',
                            boxLabel: 'Показать закрытые проверки',
                            labelAlign: 'right',
                            checked: false,
                            margin: '10px 10px 0 0'
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