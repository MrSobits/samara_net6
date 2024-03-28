Ext.define('B4.view.constructionobject.participant.EditWindow', {
    extend: 'B4.form.Window',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    minWidth: 600,
    minHeight: 100,
    bodyPadding: 5,
    alias: 'widget.constructobjparticipanteditwindow',
    title: 'Участник строительства',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [        
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.enums.ConstructionObjectParticipantType',
        'B4.enums.ConstructionObjectCustomerType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 170,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'combobox',
                    name: 'ParticipantType',
                    fieldLabel: 'Участник строительства',
                    labelAlign: 'right',
                    store: B4.enums.ConstructionObjectParticipantType.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    allowBlank: false
                },
                {
                    xtype: 'combobox',
                    name: 'CustomerType',
                    fieldLabel: 'Тип заказчика',
                    labelAlign: 'right',
                    store: B4.enums.ConstructionObjectCustomerType.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    hidden: true
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                    name: 'Contragent',
                    fieldLabel: 'Наименование участника',
                    allowBlank: false,
                    labelAlign: 'right',
                    store: 'B4.store.Contragent',
                    columns: [
                        { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'ContragentInn',
                    fieldLabel: 'ИНН',
                    readOnly: true
                },
                {
                    xtype: 'textfield',
                    name: 'ContragentContactName',
                    fieldLabel: 'ФИО руководителя',
                    readOnly: true
                },
                {
                    xtype: 'textfield',
                    name: 'ContragentContactPhone',
                    fieldLabel: 'Контактный телефон',
                    readOnly: true
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Дополнительная информация',
                    maxLength: 300
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