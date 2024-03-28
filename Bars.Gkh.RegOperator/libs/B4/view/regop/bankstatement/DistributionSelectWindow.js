Ext.define('B4.view.regop.bankstatement.DistributionSelectWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.rbankstatementdistrselectwin',

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
    modal: true,

    distributionIds: null,
    distributionSource: null,
    permissionNs: null,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.DistributionType');

        store.on('beforeload', function(s, operation) {
            operation.params = operation.params || {};
            operation.params.distributionIds = Ext.JSON.encode(me.distributionIds);
            operation.params.distributionSource = me.distributionSource;
            operation.params.permissionNs = me.permissionNs;
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
                                        click: function() {
                                            me.close();
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