Ext.define('B4.view.administration.executionaction.actionwithparams.FileStorageAnalyzeAction', {
    extend: 'Ext.form.Panel',
    requires: ['B4.form.MonthPicker'],

    border: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    flex: 1,
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'b4monthpicker',
                            name: 'Month',
                            fieldLabel: 'Месяц',
                            format: 'F, Y',
                            margin: '0 10 0 0',
                            labelWidth: 50,
                            width: 200,
                            allowBlank: false,
                            editable: false
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});