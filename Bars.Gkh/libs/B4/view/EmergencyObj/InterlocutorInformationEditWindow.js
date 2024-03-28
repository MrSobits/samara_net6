Ext.define('B4.view.emergencyobj.InterlocutorInformationEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 600,
    minWidth: 600,
    minHeight: 400,
    maxHeight: 870,
    bodyStyle: 'padding: 5px 20px 0px 5px; ',
    itemId: 'emergencyObjInterlocutorInformationEditWindow',
    title: 'Cобственник',
    closeAction: 'hide',
    closable: true,
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.ResettlementProgramSource',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhDecimalField',
        'B4.enums.RoomOwnershipType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                anchor: '100%',
                labelAlign: 'right'
                },
                items: [
                    {
                        xtype: 'fieldset',
                        defaults: {
                            labelWidth: 250,
                            labelAlign: 'right',
                            anchor: '100%',
                        },
                        title: 'Сведения о собственниках',
                        items: [
                            {
                                xtype: 'container',
                                layout: 'column',
                                items: [
                                    {
                                        xtype: 'container',
                                        columnWidth: 0.5,
                                        layout: 'anchor',
                                        defaults: {
                                            anchor: '100%'
                                        },
                                        items: [
                                            {
                                            	xtype: 'textfield',
                                            	name: 'ApartmentNumber',
                                            	fieldLabel: 'Номер квартиры',
                                            	allowBlank: false,
                                            	maxLength: 300
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'ApartmentArea',
                                                fieldLabel: 'Площадь квартиры',
                                                maxLength: 300
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'FIO',
                                                fieldLabel: 'ФИО собственника',
                                                allowBlank: false,
                                                maxLength: 300
                                            },
                                            {
                                                xtype: 'checkbox',
                                                id: 'AvailabilityMinorsAndIncapacitatedProprietors',
                                                name: 'AvailabilityMinorsAndIncapacitatedProprietors',
                                                fieldLabel: 'Наличие несовершеннолетних или недееспособных собственников',
                                                labelWidth: 390,
                                                flex: 0.1
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        columnWidth: 0.5,
                                        layout: 'anchor',
                                        defaults: {
                                            labelAlign: 'right',
                                            anchor: '100%'
                                        },
                                        items: [
                                            {
                                                xtype: 'combobox',
                                                editable: false,
                                                fieldLabel: 'Тип собственности',
                                                store: B4.enums.RoomOwnershipType.getStore(),
                                                displayField: 'Display',
                                                valueField: 'Value',
                                                labelWidth: 130,
                                                name: 'PropertyType'
                                            }
                                        ]
                                    }

                                ]
                            }
                        ]
                    },
                    {
                        xtype: 'fieldset',
                        defaults: {
                            labelAlign: 'right',
                            labelWidth: 380,
                            anchor: '100%'
                        },
                        title: 'Требование о о сносе/реконструкции аварийного МКД',
                        items: [
                            {
                                xtype: 'datefield',
                                name: 'DateDemolitionIssuing',
                                fieldLabel: 'Дата направления требования о сносе/реконструкции аварийного МКД - дата, необязательное.',
                                labelAlign: 'right',
                                format: 'd.m.Y'
                            },
                            {
                                xtype: 'datefield',
                                name: 'DateDemolitionReceipt',
                                fieldLabel: 'Дата получения требования о сносе/реконструкции аварийного МКД - дата, необязательное.',
                                labelAlign: 'right',
                                format: 'd.m.Y'
                            }
                        ]
                    },
                    {
                        xtype: 'fieldset',
                        defaults: {
                            labelWidth: 380,
                            labelAlign: 'right',
                            anchor: '100%'
                        },
                        title: 'Постановление об изъятии жилого помещения',
                        items: [
                            {
                                xtype: 'datefield',
                                name: 'DateNotification',
                                fieldLabel: 'Дата направления уведомления об изъятии жилого помещения',
                                labelAlign: 'right',
                                format: 'd.m.Y'
                            },
                            {
                                xtype: 'datefield',
                                name: 'DateNotificationReceipt',
                                fieldLabel: 'Дата получения уведомления об изъятии жилого помещения',
                                labelAlign: 'right',
                                format: 'd.m.Y'
                            }
                        ]
                    },
                    {
                        xtype: 'fieldset',
                        defaults: {
                            labelWidth: 380,
                            labelAlign: 'right',
                            anchor: '100%'
                        },
                        title: 'Соглашение об изъятии жилого помещения',
                        items: [
                            {
                                xtype: 'datefield',
                                name: 'DateAgreement',
                                fieldLabel: 'Дата заключения соглашения об изъятии аварийного жилого дома',
                                labelAlign: 'right',
                                format: 'd.m.Y'
                            },
                            {
                                xtype: 'datefield',
                                name: 'DateAgreementRefusal',
                                fieldLabel: 'Дата получения отказа от заключения соглашения',
                                labelAlign: 'right',
                                format: 'd.m.Y'
                            }
                        ]
                    },
                    {
                        xtype: 'fieldset',
                        defaults: {
                            labelWidth: 260,
                            labelAlign: 'right',
                            anchor: '100%'
                        },
                        title: 'Судебное разбирательство',
                        items: [
                            {
                                xtype: 'datefield',
                                name: 'DateOfReferralClaimCourt',
                                fieldLabel: 'Дата направления искового заявления в суд',
                                labelAlign: 'right',
                                format: 'd.m.Y'
                            },
                            {
                                xtype: 'datefield',
                                name: 'DateOfDecisionByTheCourt',
                                fieldLabel: 'Дата вынесения решения судом 1 инстанции',
                                labelAlign: 'right',
                                format: 'd.m.Y'
                            },
                            {
                                xtype: 'textarea',
                                labelAlign: 'right',
                                name: 'ConsiderationResultClaim',
                                fieldLabel: 'Результат рассмотрения искового заявления',
                                maxLength: 2000
                            },
                            {
                                xtype: 'datefield',
                                name: 'DateAppeal',
                                fieldLabel: 'Дата направления апелляции',
                                labelAlign: 'right',
                                format: 'd.m.Y'
                            },
                            {
                                xtype: 'datefield',
                                name: 'DateAppealDecision',
                                fieldLabel: 'Дата вынесения решения апелляции',
                                labelAlign: 'right',
                                format: 'd.m.Y'
                            },
                            {
                                xtype: 'textarea',
                                labelAlign: 'right',
                                name: 'AppealResult',
                                fieldLabel: 'Результат рассмотрения апелляции',
                                maxLength: 2000
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