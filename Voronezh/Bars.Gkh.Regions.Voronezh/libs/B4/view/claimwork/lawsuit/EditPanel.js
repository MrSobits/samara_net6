Ext.define('B4.view.claimwork.lawsuit.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: 'fit',
    width: 500,
    bodyPadding: 5,
    alias: 'widget.clwlawsuiteditpanel',
    title: 'Исковая работа',
    autoScroll: true,
    frame: true,
    requires: [
        'B4.view.claimwork.lawsuit.MainInfoPanel',
        'B4.view.claimwork.lawsuit.CourtClaimInfoPanel',
        'B4.view.claimwork.lawsuit.CollectionPanel',
        'B4.view.claimwork.lawsuit.DocGrid',
        'B4.view.claimwork.lawsuit.CourtGrid',
        'B4.view.claimwork.lawsuit.LawsuitReferenceCalculationGrid',
        'B4.view.claimwork.LawSuitDebtWorkSSPGrid'
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
                        { xtype: 'clwlawsuitcourtclaiminfopanel', flex: 1 },
                        { xtype: 'clwlawsuitmaininfopanel', flex: 1 },
                        { xtype: 'claimworklawsuitdocgrid', flex: 1 },
                        { xtype: 'clwlawsuitcollectionpanel', flex: 1 },
                        { xtype: 'lawsuitsspgrid', flex: 1 },
                        { xtype: 'claimworklawsuitcourtgrid', flex: 1 },
                        { xtype: 'lawsuitownerinfogrid', flex: 1 },
                        { xtype: 'lawsuitreferencecalculationgrid', flex: 1 }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});