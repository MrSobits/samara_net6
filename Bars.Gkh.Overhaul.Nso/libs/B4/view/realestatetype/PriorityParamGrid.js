Ext.define('B4.view.realestatetype.PriorityParamGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.realestatetypepriorityparamgrid',

    requires: [
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Delete',
        'B4.store.program.PriorityParam',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.store.RealEstateTypePriorityParam'
    ],
    
    store: 'RealEstateTypePriorityParam',

    initComponent: function () {
        var me = this,
            storeParams = Ext.create('B4.store.program.PriorityParam');
        
        storeParams.load();

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
               {
                   header: 'Параметр',
                   dataIndex: 'Code',
                   flex: 1,
                   editor: {
                       xtype: 'combo',
                       store: storeParams,
                       valueField: 'Code',
                       displayField: 'Name',
                       allowBlank: false,
                       editable: false,
                       listeners: {
                           change: function (cmb, newValue, oldValue) {
                               var existing = [],
                                   store = cmb.up('grid').getStore();

                               store.each(function (r) {
                                   existing.push(r.get('Code'));
                               });
                               
                               if (Ext.Array.contains(existing, newValue) && newValue != oldValue) {
                                   cmb.suspendEvents();
                                   cmb.setValue(oldValue);
                                   cmb.resumeEvents();
                                   B4.QuickMsg.msg("Предупреждение", "Нельзя добавлять один и тот же параметр дважды.", "warning");
                               }
                           }
                       }
                   },
                   renderer: function (value) {
                       var record = storeParams.findRecord('Code', value);
                       if (record) {
                           return record.get('Name');
                       }
                       return '';
                   }
               },
               {
                   header: 'Вес',
                   dataIndex: 'Weight',
                   flex: 1,
                   editor: {
                       xtype: 'numberfield',
                       allowBlank: false,
                       minValue: 1
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
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' },
                                { xtype: 'b4savebutton'}
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