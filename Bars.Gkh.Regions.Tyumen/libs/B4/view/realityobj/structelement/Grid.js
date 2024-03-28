Ext.define('B4.view.realityobj.structelement.Grid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.structelementgrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.realityobj.StructuralElement',
        'B4.ux.grid.filter.YesNo',
        'Ext.grid.feature.Grouping',
        'B4.form.GridStateColumn'
    ],

    title: 'Конструктивные элементы дома',
    store: 'realityobj.StructuralElement',
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        var groupingFeature = Ext.create('Ext.grid.feature.Grouping', {
            groupHeaderTpl: 'ООИ: {name}'
        });

        var requiredElementsNeededText = {
            xtype: 'label',
            name: 'info',
            text: 'Добавьте обязательные конструктивные элементы',
            style: 'color: #fa6e6e; font-weight: bold',
            hidden: true
        };

        Ext.applyIf(me, {
            features: [groupingFeature],
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
                    width: 200,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        editable: false,
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'ovrhl_ro_struct_el';
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
                    dataIndex: 'Group',
                    flex: 1,
                    text: 'Группа конструктивного элемента',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 3,
                    text: 'Конструктивный элемент',
                    filter: {
                        xtype: 'textfield'
                    },
                    renderer: function (val, p, rec) {
                        if (val == null) {
                            return rec.get('ElementName');
                        }
                        return rec.get('Multiple') ? val : rec.get('ElementName');
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'LastOverhaulYear',
                    flex: 1,
                    text: 'Год установки или последнего кап.ремонта',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        allowDecimal: false,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Wearout',
                    width: 100,
                    text: 'Износ(%)',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true,
                        minValue: 0,
                        maxValue: 100
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Volume',
                    width: 100,
                    text: 'Объем',
                    filter: {
                        xtype: 'numberfield',
                        operand: CondExpr.operands.eq,
                        hideTrigger: true
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'UnitMeasure',
                    width: 100,
                    text: 'Единица измерения',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanRepairYear',
                    width: 125,
                    text: 'Плановый год ремонта',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        allowDecimal: false,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    }
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'AdjustedYear',
                //    width: 125,
                //    hidden: true,
                //    text: 'Год в ДПКР',
                //    filter: {
                //        xtype: 'numberfield',
                //        hideTrigger: true,
                //        allowDecimal: false,
                //        minValue: 0,
                //        operand: CondExpr.operands.eq
                //    }
                //},
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record, index) {
                    var result = new String();
                    if (!record.data.RequiredFieldsFilled) {
                        result = 'back-coralred';
                    }
                    return result;
                }
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        requiredElementsNeededText
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