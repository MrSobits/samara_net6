Ext.define('B4.view.realityobj.decision_protocol.DecisionEdit', {

    extend: 'B4.form.Window',
    alias: 'widget.decisionedit',

    requires: [
        'B4.form.ComboBox',
        'B4.Url'
    ],
    modal: true,
    title: 'Решение',
    border: false,
    width: 600,

    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            items: {
                xtype: 'form',
                items: [
                    {
                        xtype: 'hiddenfield',
                        name: 'Id'
                    },
                    {
                        xtype: 'hiddenfield',
                        name: 'Protocol'
                    },
                    {
                        xtype: 'panel',
                        bodyPadding: 10,
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        bodyStyle: Gkh.bodyStyle,
                        border: false,
                        name: 'DecisionTypeCombo'
                    }
                ]
            }
        });

        me.callParent(arguments);
    }
});