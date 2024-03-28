Ext.define('B4.view.administration.executionaction.actionwithparams.CreateDebtorsAction', {
    extend: 'Ext.form.Panel',

    border: false,

    initComponent: function () {

        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'checkbox',
                    fieldLabel: 'Обновление реестра неплательщиков',
                    name: 'UpdateClaimWorks',
                    flex: 1,
                    labelWidth: 220,
                    labelAlign: 'right'
                }
            ]
        });

        me.callParent(arguments);
    }
});