Ext.define('B4.view.finactivity.RepairCategoryGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.finrepaircateggrid',
    store: 'finactivity.RepairCategory',
    itemId: 'finActivityRepairCategoryGrid',
    title: 'Ремонт дома, благоустройство территории и средний срок обслуживания МКД',

    requires: [
        'B4.view.Control.GkhDecimalField',
        
        'B4.enums.TypeCategoryHouseDi'
    ],

    initComponent: function () {
        var me = this;

        var renderer = function (val, meta, rec, index) {
            if (rec.get('IsInvalid').split(';')[index] == true) {
                meta.style = 'background: red;';
                meta.tdAttr = 'data-qtip="Введенное значение не совпадает с суммой по столбцу"';
            }
            return val;
        };

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeCategoryHouseDi',
                    flex: 1,
                    text: 'Категория',
                    renderer: function (val) { return B4.enums.TypeCategoryHouseDi.displayRenderer(val); }
                },
                {
                    text: 'Ремонт и благоустройство территории',
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'WorkByRepair',
                            width: 250,
                            text: 'Объем работ по ремонту (тыс. руб.)',
                            editor: 'gkhdecimalfield',
                            renderer: function (val, meta, rec) {
                                val = renderer(val, meta, rec, 0);
                                
                                if (!Ext.isEmpty(val)) {
                                    val = '' + val;
                                    if (val.indexOf('.') != -1) {
                                        val = val.replace('.', ',');
                                    }
                                    return val;
                                }
                                return '';
                            }
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'WorkByBeautification',
                            width: 250,
                            text: 'Объем работ по благоустройству (тыс. руб.)',
                            editor: 'gkhdecimalfield',
                            renderer: function (val, meta, rec) {
                                val = renderer(val, meta, rec, 1);
                                
                                if (!Ext.isEmpty(val)) {
                                    val = '' + val;
                                    if (val.indexOf('.') != -1) {
                                        val = val.replace('.', ',');
                                    }
                                    return val;
                                }
                                return '';
                            }
                        }
                    ]
                },
                {
                    text: 'Средний срок обслуживания МКД',
                    flex: 1,
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'PeriodService',
                            width: 250,
                            text: 'Срок обслуживания',
                            editor: 'gkhdecimalfield',
                            renderer: function (val, meta, rec) {
                                val = renderer(val, meta, rec, 2);
                                
                                if (!Ext.isEmpty(val)) {
                                    val = '' + val;
                                    if (val.indexOf('.') != -1) {
                                        val = val.replace('.', ',');
                                    }
                                    return val;
                                }
                                return '';
                            }
                        }
                    ]
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});