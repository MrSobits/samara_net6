Ext.define('B4.view.manorglicense.CourtDecisionGisEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 1000,
    minWidth: 1000,
    minHeight: 300,
    maxHeight: 400,
    bodyPadding: 5,
    title: 'Форма редактирования решения судебного участка',
    closeAction: 'hide',
    trackResetOnLoad: true,
    itemId: 'manorglicensecourtdecisiongiseditwindow',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.store.dict.JudicalOffice',
        'B4.view.dict.judicaloffice.Grid',
        'B4.model.dict.JudicalOffice',
        'B4.store.dict.Inspector',
        'B4.view.dict.inspector.Grid',
        'B4.model.dict.Inspector',
        'B4.enums.CourtDecisionType'
    ],

    initComponent: function () {
        debugger;
        var me = this;
        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 170
            },
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        labelAlign: 'right'
                    },
                    items: [
                          {
                              xtype: 'combobox',
                              labelWidth: 90,
                              editable: false,
                              fieldLabel: 'Признак правомочности',
                              store: B4.enums.CourtDecisionType.getStore(),
                              displayField: 'Display',
                              allowBlank: true,
                              flex: 1,
                              valueField: 'Value',
                              name: 'CourtDecisionType'
                          },
                        {
                            xtype: 'textfield',
                            name: 'DecisionNumber',
                            fieldLabel: 'Номер решения судебного участка',
                            allowBlank: false,
                            flex: 1
                        },
                       {
                           xtype: 'datefield',
                           name: 'DecisionDate',
                           labelWidth: 200,
                           width: 300,
                           allowBlank: false,
                           fieldLabel: 'Дата решения',
                           format: 'd.m.Y'
                       },
                        {
                            xtype: 'textfield',
                            name: 'ProtocolNumber',
                            fieldLabel: 'Номер протокола',
                            allowBlank: false,
                            flex: 1
                        },
                        {
                            xtype: 'component',
                            width: 120
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Violation',
                            fieldLabel: 'Нарушение',
                            allowBlank: false,
                            flex: 1
                        },
                       {
                           xtype: 'datefield',
                           name: 'DecisionEntryDate',
                           labelWidth: 200,
                           width: 300,
                           allowBlank: true,
                           fieldLabel: 'Дата вступления в законную силу',
                           format: 'd.m.Y'
                       },
                        {
                            xtype: 'component',
                            width: 120
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        padding: '0 0 5 0',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            labelAlign: 'right',
                            labelWidth: 200,
                            name: 'JudicalOffice',
                            fieldLabel: 'Судебный участок',
                            store: 'B4.store.dict.JudicalOffice',
                            readOnly: false,
                            flex: 1
                        },
                       {
                           xtype: 'b4selectfield',
                           store: 'B4.store.dict.Inspector',
                           name: 'Inspector',
                           editable: true,
                           fieldLabel: 'Инспектор',
                           textProperty: 'Fio',
                           isGetOnlyIdProperty: true,
                           columns: [
                               { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                               { text: 'Описание', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } }
                           ]
                       },
                        {
                            xtype: 'component',
                            width: 120
                        }
                    ]
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});