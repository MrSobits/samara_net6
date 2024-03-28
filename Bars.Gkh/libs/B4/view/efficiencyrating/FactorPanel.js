Ext.define('B4.view.efficiencyrating.FactorPanel', {
    extend: 'Ext.panel.Panel',
    requires: [
        'B4.ux.button.Add',

        'B4.view.efficiencyrating.AttributeTreePanel',
        'B4.view.efficiencyrating.FactorForm'
    ],

    title: 'Добавление атрибутов',
    alias: 'widget.efFactorPanel',
    clsable: false,

    layout: { type: 'vbox', align: 'stretch' },
    bodyStyle: Gkh.bodyStyle,

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'efFactorFormPanel'
                },
                {
                    xtype: 'efAttributeTreePanel',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});
