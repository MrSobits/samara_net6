Ext.define('B4.view.dict.multipurpose.GlossaryNavigation', {    
    extend: 'Ext.panel.Panel',
    
    requires: ['B4.view.dict.multipurpose.GlossaryGrid'],

    closable: true,
    title: 'Универсальные справочники',
    alias: 'widget.multipurposeGlossaryNavigation',
    layout: {
         type: 'border'
    },
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {            
            items: [
                {
                    region: 'north',
                    xtype: 'breadcrumbs',
                    itemId: 'multiGlossaryInfoLabel'
                },
                {
                    id: 'multiGlossaryMenuGrid',
                    xtype: 'multiGlossaryGrid',
                    width: 300,
                    region: 'west'
                },
                {
                    xtype: 'multipurposeItemsGrid',
                    region: 'center'
                }
            ]
        });

        me.callParent(arguments);
    }
});