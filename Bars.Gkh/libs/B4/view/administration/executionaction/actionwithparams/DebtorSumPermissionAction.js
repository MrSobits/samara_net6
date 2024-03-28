Ext.define('B4.view.administration.executionaction.actionwithparams.DebtorSumPermissionAction', {
    extend: 'Ext.form.Panel',

    border: false,

    initComponent: function () {

        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 350
            },
            items: [
                {
                    xtype: 'checkbox',
                    fieldLabel: 'Детализация суммы по базовому тарифу и тарифу решения',
                    name: 'IsDetail',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});