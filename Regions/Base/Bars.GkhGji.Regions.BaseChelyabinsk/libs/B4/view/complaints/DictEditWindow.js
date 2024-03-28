Ext.define('B4.view.complaints.DictEditWindow', {
    extend: 'B4.form.Window',
    requires: [        
     
        'B4.ux.button.Close',
        'B4.enums.CompleteReject',
        'B4.form.EnumCombo',
        'B4.view.complaints.DecisionLSGrid',
        'B4.ux.button.Save'       
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 800,
    bodyPadding: 10,
    itemId: 'complaintsdecEditWindow',
    title: 'Решение по жалобе',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код',
                    allowBlank: false,
                    maxLength: 5
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    maxLength: 300
                },               
                {
                    xtype: 'textfield',
                    name: 'FullName',
                    fieldLabel: 'Наименоание в ТОРе',
                    maxLength: 300
                },
                {
                    xtype: 'b4enumcombo',
                    anchor: '100%',
                    fieldLabel: 'Тип',
                    enumName: 'B4.enums.CompleteReject',
                    name: 'CompleteReject'
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'complaintsdeclsTabPanel',
                    flex: 1,
                    border: false,
                    items: [                       
                        {
                            xtype: 'complaintsdeclsgrid',
                            disabled: true,
                            flex: 1
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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