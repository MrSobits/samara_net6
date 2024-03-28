Ext.define('B4.view.subsidy.SubsidyMuTabPanel', {
    extend: 'Ext.tab.Panel',

    alias: 'widget.subsidymunicipalitytabpanel',

    requires: [
        'B4.view.subsidy.SubsidyMuPanel', 'B4.view.subsidy.SubsidyMuChart'
    ],
    
    title: 'Субсидирование',
    closable: true,
    
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
            {
                xtype: 'subsidymunicipalitypanel'
            },
            {
                xtype: 'panel',
                name: 'graphpanel',
                layout: 'fit',
                title: 'Просмотреть график',
                closable: true,
                items: [
                    {
                        xtype: 'subsidymunicipalitychart'
                    }
                ],
                disabled: true
            }]
        });

        me.callParent(arguments);
    }
});