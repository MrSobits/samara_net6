Ext.define('B4.view.contragent.MunicipalityGrid', {    
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.contragentmunicipalitygrid',
    requires: [
        'B4.store.contragent.MunicipalityStore',
        'B4.ux.grid.column.Delete',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.mixins.MaskBody',
        
        'B4.ux.grid.toolbar.Paging'
    ],
    
    mixins:[
        'B4.mixins.MaskBody'
    ],
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.contragent.MunicipalityStore', {
                autoLoad: false
            });

        Ext.apply(me, {
            title: 'Муниципальные районы',
            closable: true,
            store: store,
            columns: [
                { header: 'Наименование', dataIndex: 'Name', filter: { xtype: 'textfield' }, flex: 1 },
                { xtype: 'b4deletecolumn' }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    docked: 'top',
                    items: [
                        { xtype: 'b4addbutton' },
                        { xtype: 'b4updatebutton' }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});