Ext.define('B4.grid.RealObjStructuralElementSelectiFieldEditor', {
    extend: 'B4.grid.SelectFieldEditor',
    alias: 'widget.realobjstructuralelementselectifieldeditor',
    requires: [
        'B4.form.GridStateColumn'
    ],

    constructor: function (config) {
        var me = this,
            store = Ext.create('B4.store.realityobj.StructuralElement');

        Ext.apply(config, {
            store: store,
            emptyText: 'Не задано',
            emptyCls: 'x-form-empty-field-cust', // добавляем несуществующий класс, чтобы очистить стиль
            textProperty: 'Name',
            columns: [
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    menuText: 'Статус',
                    width: 150,
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
                    }
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
                }
            ]
        });

        me.callParent(arguments);
    }
});