Ext.define('B4.view.baselicenseapplicants.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.baselicenseapplicantsgrid',

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
        'B4.store.BaseLicenseApplicants'
    ],

    store: 'BaseLicenseApplicants',
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
                    dataIndex: 'RealObjAddresses',
                    text: 'Адреса домов',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PersonInspection',
                    width: 150,
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
                    width: 150,
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
                    xtype: 'gridcolumn',
                    dataIndex: 'ReqNumber',
                    text: 'Номер обращения',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectionNumber',
                    text: 'Номер',
                    width: 80,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    text: 'Юридическое лицо',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IsDisposal',
                    width: 100,
                    text: 'Распоряжение ',
                    renderer: function (val) {
                        return val ? "Да" : "Нет";
                    },
                    filter: { xtype: 'b4dgridfilteryesno' }
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