Ext.define('B4.view.administration.executionaction.actionwithparams.EiasTariffImportServiceAction', {
    extend: 'Ext.form.Panel',

    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'datefield',
                    labelWidth: 120,
                    fieldLabel: 'Дата изменений с',
                    name: 'TimeStamp',
                    maxValue: new Date(),
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});