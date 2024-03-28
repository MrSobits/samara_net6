Ext.define('B4.view.decision.CreditOrganization', {
    extend: 'Ext.form.Panel',

    requires: [
        'B4.store.CreditOrg',
        'B4.form.FileField'
    ],

    border: false,

    initComponent: function() {
        Ext.apply(this, {
            defaults: {
                hideTrigger: (this.saveable === false),
                labelWidth: 200
            },
            items: [
                {
                    xtype: 'hidden',
                    name: 'byTypeCode',
                    value: true
                },
                {
                    xtype: 'datefield',
                    name: 'StartDate',
                    fieldLabel: 'Дата ввода в действие решения'
                },
                 {
                     xtype: 'textfield',
                     fieldLabel: 'Наименование',
                     name: 'CreditOrgName'
                 }
                 //,
                //{
                //    xtype: 'b4selectfield',
                //    fieldLabel: 'Наименование',
                //    name: 'CreditOrg',
                //    store: 'B4.store.CreditOrg',
                //    editable: false,
                //    isGetOnlyIdProperty: false
                //}
            ]
        });

        this.callParent(arguments);
    },

    afterShow: function() {
        this.add([
            {
                xtype: 'fieldset',
                title: 'Реквизиты кредитной организации',
                layout: {
                    type: 'vbox',
                    align: 'stretch'
                },
                items: [
                    {
                        xtype: 'textfield',
                        fieldLabel: 'Почтовый адрес',
                        name: 'CreditOrgAddress'
                    },
                    {
                        xtype: 'container',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        items: [
                            {
                                xtype: 'fieldcontainer',
                                defaults: {
                                    labelAlign: 'right'
                                },
                                items: [
                                    {
                                        xtype: 'textfield',
                                        fieldLabel: 'ИНН',
                                        name: 'CreditOrgInn'
                                    },
                                    {
                                        xtype: 'textfield',
                                        fieldLabel: 'ОГРН',
                                        name: 'CreditOrgOgrn'
                                    },
                                    {
                                        xtype: 'textfield',
                                        fieldLabel: 'БИК',
                                        name: 'CreditOrgBik'
                                    },
                                    {
                                        xtype: 'textfield',
                                        fieldLabel: 'Номер р/с',
                                        name: 'CreditOrgAcc'
                                    }
                                ]
                            },
                            {
                                xtype: 'fieldcontainer',
                                defaults: {
                                    labelAlign: 'right'
                                },
                                items: [
                                    {
                                        xtype: 'textfield',
                                        fieldLabel: 'КПП',
                                        name: 'CreditOrgKpp'
                                    },
                                    {
                                        xtype: 'textfield',
                                        fieldLabel: 'ОКАТО',
                                        name: 'CreditOrgOkato'
                                    },
                                    {
                                        xtype: 'textfield',
                                        fieldLabel: 'ОКПО',
                                        name: 'CreditOrgOkpo'
                                    },
                                    {
                                        xtype: 'textfield',
                                        fieldLabel: 'Кор. счет',
                                        name: 'CreditOrgCorAcc'
                                    }
                                ]
                            }
                        ]
                    }
                ]
            },
            {
                xtype: 'fieldset',
                title: 'Информация о спец. счете',
                items: [
                    {
                        xtype: 'fieldcontainer',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        defaults: {
                            labelAlign: 'right'
                        },
                        items: [
                            {
                                xtype: 'textfield',
                                fieldLabel: 'Номер счета',
                                name: 'AccountNum'
                            },
                            {
                                xtype: 'datefield',
                                fieldLabel: 'Дата открытия',
                                name: 'AccountCreateDate',

                                hideTrigger: (this.saveable === false)
                            }
                        ]
                    },
                    {
                        xtype: 'b4filefield',
                        labelAlign: 'right',
                        fieldLabel: 'Справка банка',
                        name: 'File',

                        hideTrigger: (this.saveable === false)
                    }
                ]
            }
        ]);
    }
});