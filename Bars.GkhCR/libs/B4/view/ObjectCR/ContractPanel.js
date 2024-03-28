Ext.define('B4.view.objectcr.ContractPanel', {
    extend: 'Ext.panel.Panel',
    closable: true,

    alias: 'widget.objectcrcontractpanel',

    title: 'Договоры',
    layout: { type: 'vbox', align: 'stretch' },
    requires: [
        'B4.view.objectcr.BuildContractGrid',
        'B4.view.objectcr.ContractCrGrid'
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
                        { xtype: 'buildContractGrid' },
                        { xtype: 'objectcrcontractgrid' }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});