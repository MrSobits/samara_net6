Ext.define('B4.view.claimwork.buildcontract.lawsuit.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'fit',
    width: 500,
    bodyPadding: 5,
    alias: 'widget.clwbclawsuiteditpanel',
    title: 'Исковая работа',
    autoScroll: true,
    frame: true,
    requires: [
        'B4.view.claimwork.buildcontract.lawsuit.MainInfoPanel',
        'B4.view.claimwork.buildcontract.lawsuit.CourtClaimInfoPanel',
        'B4.view.claimwork.lawsuit.CollectionPanel',
        'B4.view.claimwork.lawsuit.DocGrid',
        'B4.view.claimwork.lawsuit.CourtGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    layout: {
                        align: 'stretch'
                    },
                    border: true,
                    items: [
                        { xtype: 'clwlawsuitbccourtclaiminfopanel', flex: 1 },
                        { xtype: 'clwlawsuitbcmaininfopanel', flex: 1 },
                        { xtype: 'claimworklawsuitdocgrid', flex: 1 },
                        { xtype: 'clwlawsuitcollectionpanel', flex: 1 },
                        { xtype: 'claimworklawsuitcourtgrid', flex: 1 }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});