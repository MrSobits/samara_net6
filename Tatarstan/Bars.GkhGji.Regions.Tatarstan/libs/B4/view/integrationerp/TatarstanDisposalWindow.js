Ext.define('B4.view.integrationerp.TatarstanDisposalWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.enums.TaskState',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.filter.YesNo',
        'B4.view.baseintegration.RisTaskGrid'
    ],

    alias: 'widget.tatarstandisposalwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 1300,
    height: 720,
    bodyPadding: 5,

    title: 'Распоряжение',
    closeAction: 'destroy',
    trackResetOnLoad: true,
    maximizable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    align: 'stretch',
                    padding: 2,
                    items: [
                        {
                            xtype: 'textfield',
                            labelAlign: 'right',
                            fieldLabel: 'Номер распоряжения',
                            name: 'DocumentNumber',
                            labelWidth: 140,
                            flex: 1.5,
                            readOnly: true
                        },
                        {
                            xtype: 'datefield',
                            labelAlign: 'right',
                            fieldLabel: 'Дата распоряжения',
                            name: 'DocumentDate',
                            format: 'd.m.Y',
                            labelWidth: 160,
                            flex: 1,
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    align: 'stretch',
                    padding: 2,
                    items: [
                        {
                            xtype: 'textfield',
                            labelAlign: 'right',
                            fieldLabel: 'Идентификатор в ЕРП',
                            name: 'ErpGuid',
                            labelWidth: 140,
                            flex: 1.5,
                            readOnly: true
                        },
                        {
                            xtype: 'datefield',
                            labelAlign: 'right',
                            fieldLabel: 'Дата присвоения идентификатора в ЕРП',
                            name: 'ErpRegistrationDate',
                            format: 'd.m.Y',
                            labelWidth: 160,
                            flex: 1,
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'integrationristaskgrid',
                    padding: 2,
                    align: 'stretch',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'button',
                            text: 'Перейти к распоряжению',
                            name: 'goToDocumentButton'
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        click: function() {
                                            var store = me.down('integrationristaskgrid').getStore();
                                            if (store) {
                                                store.load();
                                            }
                                        }
                                    }
                                },
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