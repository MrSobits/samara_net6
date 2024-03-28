Ext.define('B4.view.specialobjectcr.ContractPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,

    alias: 'widget.specialobjectcrcontractpanel',

    title: 'Договоры',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    requires: [
        'B4.view.specialobjectcr.BuildContractGrid',
        'B4.view.specialobjectcr.ContractCrGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    enableTabScroll: true,
                    flex: 1,
                    items: [
                        { xtype: 'specialobjectcrbuildcontractgrid' },
                        { xtype: 'specialobjectcrcontractgrid' }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});