Ext.define('B4.view.import.chesimport.RealityObjectGrid', {
    extend: 'Ext.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.button.Update',

        'B4.form.ComboBox',
        'B4.form.GridStateColumn',

        'B4.enums.TypeHouse',
        'B4.enums.ConditionHouse',
        'B4.enums.CrFundFormationType'
    ],

    alias: 'widget.chesimportrealityobjectgrid',
    columnLines: true,
    title: 'Адреса в системе',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.view.ViewRealityObject');

        Ext.applyIf(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel', { mode: 'SINGLE' }),
            columns: [
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
                            storebeforeload: function(field, store, options) {
                                options.params.typeId = 'gkh_real_obj';
                            }
                        }
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 200,
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
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ConditionHouse',
                    width: 100,
                    text: 'Состояние дома',
                    renderer: function (val) {
                        return B4.enums.ConditionHouse.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.ConditionHouse.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccountFormationVariant',
                    width: 200,
                    text: 'Способ формирования фонда',
                    renderer: function (val) {
                        return B4.enums.CrFundFormationType.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.CrFundFormationType.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
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
                    store: store,
                    view: me,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});
