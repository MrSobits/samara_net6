Ext.define('B4.view.administration.executionaction.actionwithparams.TestMandatoryAction', {
    extend: 'Ext.form.Panel',

    border: false,

    initComponent: function () {

        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textfield',
                    fieldLabel: 'Количество итераций',
                    name: 'WorkIterationCount',
                    flex: 1
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Вероятность ошибки',
                    name: 'ErrorProbability',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});