Ext.define('B4.view.regressionanalysis.Chart', {
    extend: 'Ext.form.Panel',
    requires: [        
        'B4.view.regressionanalysis.ChartUnit'
    ],
    alias: 'widget.regressionanalysischart',
    
    layout: 'fit',    
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regressionanalysis.Chart');            

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'regressionanalysischartunit',
                    store: store                    
                }
            ]
        });
        me.callParent(arguments);
    }
});