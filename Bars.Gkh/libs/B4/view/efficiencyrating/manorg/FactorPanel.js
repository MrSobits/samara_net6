Ext.define('B4.view.efficiencyrating.manorg.FactorPanel', {
    extend: 'Ext.form.Panel',
    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',

        'B4.view.efficiencyrating.manorg.FactorTreePanel',
        'B4.view.efficiencyrating.manorg.AttributeEditPanel'
    ],

    alias: 'widget.efManorgFactorPanel',
    closable: false,

    layout: { type: 'hbox', align: 'stretch' },
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    border: null,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me,
        {
            items: [
                {
                    xtype: 'efManorgFactorTreePanel',
                    flex: 1,
                    padding: '0 1px 0 0'
                },
                {
                    xtype: 'efAttributeEditPanel',
                    flex: 1,
                    padding: '0 0 0 1px'
                }
            ]
        });

        me.callParent(arguments);
    }
});