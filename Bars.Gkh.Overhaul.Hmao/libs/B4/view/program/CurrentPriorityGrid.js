Ext.define('B4.view.program.CurrentPriorityGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.currprioritygrid',

    requires: [
        'B4.ux.grid.column.Delete',
        'B4.store.program.PriorityParam',
        'B4.ux.button.Add',
        'B4.ux.button.Close',
        'B4.store.CurrentPrioirityParams'
    ],

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.program.PriorityParam'),
            paramStore = Ext.create('B4.base.Store', {
                fields: ['Code', 'Name'],
                autoLoad: false,
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'RealityObjectStructuralElementInProgrammStage3',
                    listAction: 'GetAllParams'
                }
        });;

        paramStore.load({
            scope: this,
            callback: function (records, operation, success) {
                if (success) {
                    store.load();
                }
            }
        });

        Ext.applyIf(me, {
            store: 'CurrentPrioirityParams',
            columnLines: true,
            columns: [
                {
                    header: 'Критерий',
                    dataIndex: 'Code',
                    flex: 1,
                    editor: {
                        xtype: 'combo',
                        store: store,
                        valueField: 'Code',
                        displayField: 'Name',
                        allowBlank: false,
                        listeners: {
                            change: function (cmb, val) {
                                var added,
                                    grid = cmb.up('grid'),
                                    st = grid.getStore();
                                added = Ext.Array.map(st.data.items, function(r) {
                                    return r.get('Code');
                                });
                                if (Ext.Array.contains(added, val)) {
                                    cmb.setValue(null);
                                    B4.QuickMsg.msg('Предупреждение', 'Нельзя добавлять один и тот же параметр дважды.', 'warning');
                                }
                            },
                            focus: function (component, eventArgs, eventOpts) {
                                var param = paramStore.findRecord('Code', component.getValue());
                                if (param) {
                                    component.setValue(param);
                                }
                            }
                        }
                    },
                    renderer: function(value, meta, rec) {
                        var param = paramStore.findRecord('Code', value),
                            paramName = param ? param.get('Name') : '';

                        return rec.get('Name') || paramName;
                    }
                },
                {
                    header: 'Порядок сортировки',
                    dataIndex: 'Order',
                    flex: 1,
                    editor: {
                        xtype: 'numberfield',
                        allowBlank: false
                    }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            tbar: {
                items: [
                    { xtype: 'b4addbutton' },
                    { xtype: 'button', text: 'Применить', cmd: 'priority', iconCls: 'icon-table-go' },
                    '-',
                    '->',
                    { xtype: 'b4closebutton' }
                ]
            }
        });

        me.callParent(arguments);
    }
});