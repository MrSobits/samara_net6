Ext.define('B4.view.regop.cproc.ComputingProcessGrid', {
    extend: 'B4.ux.grid.Panel',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',

        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.store.regop.ComputingProcess',

        'B4.enums.regop.ComputingProcessType',
        'B4.enums.regop.ComputingProcessStatus'
    ],

    title: 'Процессы',

    alias: 'widget.cprocgrid',

    store: 'regop.ComputingProcess',

    closable: true,

    initComponent: function() {
        var me = this;


        Ext.apply(me, {
            columns: [
                {
                    xtype: 'b4editcolumn'
                },
                {
                    text: 'Наименование',
                    dataIndex: 'Name',
                    flex: 1
                },
                {
                    text: 'Тип',
                    dataIndex: 'Type',
                    flex: 1,
                    renderer: function(value) {
                        if (value) {
                            return B4.enums.regop.ComputingProcessType.getStore().findRecord('Value', value).get('Display');
                        }
                        return value;
                    }
                },
                {
                    text: 'Дата формирования',
                    dataIndex: 'ObjectCreateDate',
                    flex: 1,
                    xtype: 'datecolumn',
                    format: 'd.m.Y H:i:s'
                },
                {
                    text: 'Статус',
                    dataIndex: 'Status',
                    flex: 1,
                    renderer: function (value) {
                        if (value) {
                            return B4.enums.regop.ComputingProcessStatus.getStore().findRecord('Value', value).get('Display');
                        }
                        return value;
                    }
                }
            ],

            dockedItems: [
                //{
                //    xtype: 'toolbar',
                //    dock: 'top',
                //    items: [
                //        {
                //            xtype: 'buttongroup',
                //            columns: 1,
                //            items: [
                //                {
                //                    xtype: 'b4updatebutton'
                //                }
                //            ]
                //        }
                //    ]
                //},
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: me.store,
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