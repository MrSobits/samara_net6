Ext.define('B4.view.longtermprobject.propertyownerdecision.work.ListServicesWorksGrid', {    
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.store.longtermprobject.ListServicesWorksStore',
        'Ext.grid.plugin.CellEditing'
    ],
    
    alias: 'widget.listservicesworksgrid',
    cls: 'x-large-head',
    store: 'listservicesworksstore',
    
    initComponent: function() {
        var me = this;

        Ext.apply(me, {
            store: Ext.create('B4.store.longtermprobject.ListServicesWorksStore'),
            columns: [
                {
                    dataIndex: 'Work',
                    header: 'Перечень работ и(или) услуг по капитальному ремонту',
                    flex: 2
                },
                {
                    dataIndex: 'PlanYear',
                    header: 'Плановый срок проведения КР',
                    align: 'center',
                    flex: 1
                },
                {
                    dataIndex: 'FactYear',
                    header: 'Фактический срок проведения КР',
                    flex: 1,
                    editor: {
                        xtype: 'numberfield',
                        minValue: 0,
                        maxValue: 3000
                    }
                }
            ],
            dockedItems:[
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1
                })
            ]
        });

        me.callParent(arguments);
    }
});