Ext.define('B4.view.decision.VerificationSubjectPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.decisionverifsubjpanel',
    layout: { type: 'vbox', align: 'stretch' },
    requires: [
        'B4.view.decision.VerificationGrid',
        'B4.view.decision.NormDocItemGrid'
    ],

    title: 'Предметы проверки',

    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'decisionverificationgrid',
                    flex: 1
                },
                {
                    xtype: 'decisionnormdocitemgrid',
                    disabled: true,
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});