Ext.define('B4.view.appealcits.SoprInformationPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.appealcitssoprinformationpanel',

    requires: [
        'B4.view.appealcits.ContragentGrid',
        'B4.view.appealcits.SoprInformationGrid'
    ],

    title: 'Сведения для СОПР',
    closable: false,
    autoScroll: true,
    
    itemId: 'appealCitsSoprInformationPanel',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'appealcitscontragentgrid',
                    height: 200
                },
                {
                    xtype: 'appealcitssoprinformationgrid',
                    height: 200
                }
            ]
        });

        me.callParent(arguments);
    }
});