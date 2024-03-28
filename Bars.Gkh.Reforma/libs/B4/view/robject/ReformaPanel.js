Ext.define('B4.view.robject.ReformaPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    alias: 'widget.robjectreformapanel',

    title: 'Реформа ЖКХ',

    layout: {
        type: 'vbox',
        align: 'left'
    },

    defaults: {
        labelWidth: 100,
        labelAlign: 'right',
        width: 800,
        margin: '5 0'
    },

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            bodyPadding: 5,
            items: [
                {
                    xtype: 'textfield',
                    name: 'ExternalIds',
                    fieldLabel: 'Код дома',
                    readOnly: true
                }
            ]
        });

        me.callParent(arguments);
    }
});