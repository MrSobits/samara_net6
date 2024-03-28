Ext.define('B4.view.regop.bankstatement.DetailWindow', {
    extend: 'B4.form.Window',

    modal: true,

    width: 700,
    height: 500,
    bodyPadding: 5,
    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Close',
        'B4.view.regop.bankstatement.DetailGrid'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Детализация распределения',
    alias: 'widget.rbankstatementdetailwin',

    entityId: null,
    source: null,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.DistributionType');

        store.on('beforeload', function (s, operation) {
            operation.params = operation.params || {};
            operation.params.isForDetailWindow = true;
            operation.params.distributionId = me.entityId;
            operation.params.distributionSource = me.source;
        });

        Ext.apply(me, {
            items: [
                {
                    xtype: 'b4combobox',
                    name: 'DistributionType',
                    fieldLabel: 'Типы распределения',
                    labelAlign: 'right',
                    labelWidth: 125,
                    store: store,
                    readOnly: true,
                    displayField: 'Name',
                    valueField: 'Code'
                },
                {
                    xtype: 'rbankstatementdetailgrid',
                    entityId: me.entityId,
                    source: me.source,
                    flex: 1
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
                                    action: 'InternalCancel',
                                    text: 'Отменить распределение',
                                    iconCls: 'icon-cross'
                                },
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        'click': function() {
                                            me.down('grid').getStore().load();
                                        }
                                    }
                                }
                            ]
                        }, '->', {
                            xtype: 'buttongroup',
                            items: [{
                                xtype: 'b4closebutton',
                                listeners: {
                                    'click': function() {
                                        me.close();
                                    }
                                }
                            }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});