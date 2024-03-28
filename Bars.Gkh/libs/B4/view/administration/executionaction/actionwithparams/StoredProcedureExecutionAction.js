Ext.define('B4.view.administration.executionaction.actionwithparams.StoredProcedureExecutionAction', {
    extend: 'Ext.form.Panel',

    border: false,

    initComponent: function () {

        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textfield',
                    fieldLabel: 'Название процедуры',
                    name: 'ProcedureName',
                    allowBlank: false,
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});