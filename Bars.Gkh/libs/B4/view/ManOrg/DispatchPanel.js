Ext.define('B4.view.manorg.DispatchPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.manorgDispatchPanel',
    requires: [
        'B4.form.FiasSelectAddress'
    ],

    closable: false,
    title: '',
    trackResetOnLoad: true,
    bodyStyle: Gkh.bodyStyle,

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    defaults: {
        labelAlign: 'right',
        labelWidth: 220,
        anchor: '100%'
    },

    isReadOnly : false,

    initComponent: function () {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 220,
                        readOnly : this.isReadOnly,
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'IsDispatchCrrespondedFact',
                            fieldLabel: 'Адрес диспетчерской службы соответствует фактическому адресу',
                            checked: false,
                            labelWidth: 350
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Контактные номера телефонов',
                            name: 'DispatchPhone'
                        }
                    ]
                },
                {
                    xtype: 'b4fiasselectaddress',
                    width:600,
                    name: 'DispatchAddress',
                    fieldLabel: 'Адрес диспетчерской службы',
                    readOnly: this.isReadOnly,
                    fieldsRegex: {
                        tfHousing: {
                            regex: /^\d+$/,
                            regexText: 'В это поле можно вводить только цифры'
                        },
                        tfBuilding: {
                            regex: /^\d+$/,
                            regexText: 'В это поле можно вводить только цифры'
                        }
                    }
                }
            ]
        });

        me.callParent(arguments);
    }

});
