Ext.define('B4.view.appealcits.EdoPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.edoPanel',
    bodyStyle: Gkh.bodyStyle,
    padding: '0 0 3 0',
    hidden: false,
    border: false,
    closable: false,
    itemId: 'edoPanel',
    height: 70,
    layout: {
        type: 'form'
    },
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
            {
                xtype: 'fieldset',
                title: 'Эдо',
                defaults: {
                    labelWidth: 140,
                    labelAlign: 'right'
                },
                layout: {
                    type: 'hbox',
                    align: 'stretch'
                },
                items: [
                        {
                            xtype: 'textfield',
                            name: 'AddressEdo',
                            fieldLabel: 'Адрес из ЭДО',
                            maxLength: 2000,
                            flex: 1,
                            readOnly: true
                        },
                        {
                            xtype: 'checkbox',
                            padding: '0 0 0 5',
                            labelWidth: 90,
                            width: 110,
                            name: 'IsEdo',
                            itemId: 'chkbxIsEdo',
                            fieldLabel: ' Из ЭДО:',
                            readOnly: true
                        },
                        {
                            xtype: 'datefield',
                            width: 240,
                            name: 'DateActual',
                            fieldLabel: 'Дата актуальности',
                            format: 'd.m.Y',
                            readOnly: true
                        }
                 ]
            }
            ]
        });

        me.callParent(arguments);
    }
});