Ext.define('B4.view.suspenseaccount.DistributionSelectWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.distributionselectwin',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.form.EnumCombo',
        'B4.store.DistributionType'
    ],

    closeAction: 'destroy',
    title: 'Выбрать распределение',

    width: 600,

    distributionId: null,
    distributionSource: null,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.DistributionType');

        store.on('beforeload', function(s, operation) {
            operation.params.distributionId = me.distributionId;
            operation.params.distributionSource = me.distributionSource;
        });

        Ext.apply(me, {
            defaults: {
                margin: '5 5 5 5',
                labelAlign: 'right',
                labelWidth: 130
            },
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            bodyStyle: Gkh.bodyStyle,
            border: 0,
            items: [
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Тип распределения',
                    store: store,
                    displayField: 'Name',
                    valueField: 'Route',
                    name: 'DistributionType',
                    allowBlank: false
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'button',
                                    action: 'selectdistribution',
                                    text: 'Продолжить'
                                }
                            ]
                        }, '->', {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function(btn) {
                                            btn.up('window').close();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                }
            ]

        });
        me.callParent(arguments);
    }
});