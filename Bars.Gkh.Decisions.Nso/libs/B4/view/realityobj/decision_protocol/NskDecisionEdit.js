Ext.define('B4.view.realityobj.decision_protocol.NskDecisionEdit', {
    extend: 'B4.form.Window',
    alias: 'widget.nskdecisionedit',

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.form.EnumCombo',
        'B4.ux.button.Close',
        'B4.form.FileField',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.store.ManagingOrganization',
        'B4.enums.MkdManagementDecisionType',
        'B4.enums.CrFundFormationDecisionType',
        'B4.store.ManagingOrganization',
        'Ext.grid.plugin.CellEditing',
        'B4.enums.AccountOwnerDecisionType',
        'B4.enums.AccountManagementType',

        // decisions forms
        'B4.view.realityobj.decision.MonthlyFee',
        'B4.view.realityobj.decision.JobYear',
        'B4.view.realityobj.decision.Protocol'
    ],

    modal: true,
    title: 'Решение общего собрания собственников жилых помещений',
    border: false,
    width: 768,
    height: 600,
    closeAction: 'destroy',
    autoScroll: true,
    protocolId: 0,

    initComponent: function () {
        var me = this,
            contragentStore = Ext.create('B4.store.Contragent');

        contragentStore.on('beforeload', function (store, operation) {
            operation.params.decisionType = me.down('form[entity=AccountOwnerDecision] b4enumcombo').getValue();
        });

        Ext.apply(me, {
            items: {
                xtype: 'form',
                defaults: {
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        margin: '5 5 5 5'
                    },
                    border: '0 0 1 0'
                },
                items: [
                    {
                        xtype: 'hiddenfield',
                        name: 'Id',
                        fname: 'Protocol'
                    },
                    {
                        xtype: 'protocoldecision'
                    },
                    {
                        xtype: 'form',
                        entity: 'CrFundFormationDecision',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        items: [
                            {
                                xtype: 'hidden',
                                name: 'Id'
                            },
                            {
                                xtype: 'checkbox',
                                name: 'IsChecked',
                                //checked: true,
                                boxLabel: 'Способ формирования фонда КР',
                                margin: '5 5 5 5',
                                flex: 1
                            },
                            {
                                xtype: 'container',
                                col: 'right',
                                width: 350,
                                layout: 'anchor',
                                items: {
                                    anchor: '100%',
                                    xtype: 'b4enumcombo',
                                    enumName: 'B4.enums.CrFundFormationDecisionType',
                                    includeEmpty: false,
                                    enumItems: [],
                                    value: 1,
                                    name: 'Decision',
                                    margin: '5 5 5 5'
                                }
                            }
                        ],
                        setValues: function(v) {
                            this.getForm().setValues(v);
                        }
                    },
                    {
                        xtype: 'form',
                        entity: 'AccountOwnerDecision',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        items: [
                            {
                                xtype: 'hidden',
                                name: 'Id'
                            },
                            {
                                xtype: 'checkbox',
                                specialacc: true,
                                disabled: true,
                                name: 'IsChecked',
                                boxLabel: 'Владелец специального счета',
                                flex: 1,
                                forceSelect: true
                            },
                            {
                                xtype: 'container',
                                width: 350,
                                col: 'right',
                                layout: 'anchor',
                                items: [
                                    {
                                        xtype: 'b4enumcombo',
                                        anchor: '100%',
                                        enumName: 'B4.enums.AccountOwnerDecisionType',
                                        includeEmpty: false,
                                        enumItems: [],
                                        margin: '5 5 5 5',
                                        name: 'DecisionType'
                                    }
                                ]
                            }
                        ],
                        setValues: function(v) {
                            this.getForm().setValues(v);
                        }
                    },
                    {
                        xtype: 'form',
                        entity: 'CreditOrgDecision',
                        layout: 'anchor',
                        items: [
                            {
                                xtype: 'hidden',
                                name: 'Id'
                            },
                            {
                                margin: '0 0 0 16',
                                xtype: 'checkbox',
                                disabled: true,
                                name: 'IsChecked',
                                boxLabel: 'Кредитная организация',
                                flex: 1
                            },
                            {
                                xtype: 'container',
                                flex: 1,
                                col: 'right',
                                layout: {
                                    type: 'vbox',
                                    align: 'stretch'
                                },
                                defaults: {
                                    flex: 1,
                                    labelWidth: 140,
                                    allowBlank: true
                                },
                                items: [
                                    {
                                        xtype: 'b4selectfield',
                                        fieldLabel: 'Наименование',
                                        name: 'Decision',
                                        store: 'B4.store.CreditOrg',
                                        isGetOnlyIdProperty: false,
                                        allowBlank: false
                                    }
                                    //,
                                    //{
                                    //    xtype: 'datefield',
                                    //    name: 'StartDate',
                                    //    fieldLabel: 'Дата открытия счета'
                                    //},
                                    //{
                                    //    xtype: 'textfield',
                                    //    name: 'BankAccountNumber',
                                    //    fieldLabel: 'Расчетный счет',
                                    //    minLength: 20,
                                    //    maxLength: 20
                                    //    //,
                                    //    //validator: function(value) {

                                    //    //    function validate(accNum, bik) {
                                    //    //        var coeffs = [7, 1, 3], bikTail = bik.substr(bik.length - 3), num, sum = 0;
                                    //    //        num = bikTail + accNum;

                                    //    //        Ext.Array.each(num.split(''), function(elem, ind) {
                                    //    //            sum += (+elem * coeffs[ind % 3]);
                                    //    //        });
                                    //    //        return sum % 10 === 0;
                                    //    //    }

                                    //    //    var credOrg;
                                    //    //    if (value.length !== 20) {
                                    //    //        return "Не верно введен номер счета (20 знаков)";
                                    //    //    }

                                    //    //    credOrg = this.up('form[entity=CreditOrgDecision]').down('b4selectfield[name=Decision]').getValue();
                                    //    //    if (credOrg) {
                                    //    //        return true; // validate(value, credOrg.Bik); пока точно не известно, как нужно проверять
                                    //    //    }
                                    //    //    return "Не выбрана кредитная организация";
                                    //    //}
                                    //},
                                    //{
                                    //    xtype: 'b4filefield',
                                    //    name: 'BankFile',
                                    //    fieldLabel: 'Справка из банка'
                                    //}
                                ]
                            }
                        ],
                        setValues: function(v) {
                            this.getForm().setValues(v);
                        }
                    },
                    {
                        xtype: 'monthlyfeedecision'
                    },
                    {
                        xtype: 'form',
                        entity: 'MinFundAmountDecision',
                        layout: {
                            type: 'hbox',
                            align: 'stertch'
                        },
                        items: [
                            {
                                xtype: 'hidden',
                                name: 'Id'
                            },
                            {
                                xtype: 'checkbox',
                                specialacc: true,
                                disabled: true,
                                name: 'IsChecked',
                                boxLabel: 'Минимальный размер фонда КР',
                                flex: 1
                            },
                            {
                                col: 'right',
                                labelWidth: 120,
                                xtype: 'numberfield',
                                hideTrigger: true,
                                disabled: true,
                                name: 'Decision',
                                minValue: 0,
                                maxValue: 100
                            }
                        ],
                        setValues: function(v) {
                            var decField = this.down('numberfield[name=Decision]');
                            decField.setMinValue(v.Default);
                            v.Decision = v.Decision || v.Default;
                            this.getForm().setValues(v);
                            decField.validate();
                        }
                    },
                    {
                        xtype: 'form',
                        entity: 'AccumulationTransferDecision',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        items: [
                            {
                                xtype: 'hidden',
                                name: 'Id',
                                flex: 0
                            },
                            {
                                xtype: 'checkbox',
                                specialacc: false,
                                //checked: true,
                                name: 'IsChecked',
                                boxLabel: 'Сумма ранее накопленных средств, перечисляемая на спецсчет',
                                flex: 2,
                                margin: '5 5 5 5'
                            },
                            {
                                labelWidth: 120,
                                col: 'right',
                                xtype: 'numberfield',
                                hideTrigger: true,
                                name: 'Decision',
                                minValue: 0,
                                allowDecimals: true
                            }
                        ],
                        setValues: function (v) {
                            this.getForm().setValues(v);
                        }
                    },
                    {
                        xtype: 'form',
                        entity: 'AccountManagementDecision',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        items: [
                            {
                                xtype: 'hidden',
                                name: 'Id',
                                flex: 0
                            },
                            {
                                xtype: 'checkbox',
                                specialacc: false,
                                name: 'IsChecked',
                                boxLabel: 'Ведение лицевых счетов',
                                flex: 1,
                                margin: '5 5 5 5'
                            },
                            {
                                xtype: 'container',
                                width: 350,
                                col: 'right',
                                layout: 'anchor',
                                items: [
                                    {
                                        xtype: 'b4enumcombo',
                                        anchor: '100%',
                                        enumName: 'B4.enums.AccountManagementType',
                                        includeEmpty: false,
                                        enumItems: [],
                                        margin: '5 5 5 5',
                                        name: 'Decision'
                                    }
                                ]
                            }
                        ],
                        setValues: function (v) {
                            this.getForm().setValues(v);
                        }
                    },
                    {
                        xtype: 'jobyeardecision',
                        height: 200
                    }
                ]
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Сформировать уведомление',
                                    itemId: 'formconfirmbtn'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Скачать договор',
                                    itemId: 'downloadContract'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'protocolStateBtn',
                                    text: 'Статус',
                                    menu: []
                                },
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
        me.__initDisable();
    },

    __initDisable: function () {
        var win = this,
            mainForm,
            subforms,
            subformsHash = {};

        mainForm = win.down('form');
        subforms = Ext.ComponentQuery.query('form', mainForm);
        Ext.each(subforms, function (sfr) {
            subformsHash[sfr.entity] = sfr;
        });


        Ext.Object.each(subformsHash, function (key, frm) {
            var fields,
                chb = frm.down('checkbox'),
                generateNotif = win.down('#formconfirmbtn'),
                downloadContr = win.down('#donwloadContract'),
                decisionEnum = win.down('b4enumcombo[name=Decision]'),
                decisionTypeEnum = win.down('b4enumcombo[name=DecisionType]'),
                val;

            if (decisionEnum.value != B4.enums.CrFundFormationDecisionType.SpecialAccount) {
                generateNotif.disable();
            }

            if (decisionEnum.value != B4.enums.CrFundFormationDecisionType.RegOpAccount) {
                if (decisionEnum.value != B4.enums.CrFundFormationDecisionType.SpecialAccount
                    && decisionTypeEnum.value != B4.enums.AccountOwnerDecisionType.RegOp) {
                    downloadContr.disable();
                }
            }
            
            if (key === 'Protocol' || key === 'MonthlyFeeAmountDecision') {

            } else if (key === 'JobYearDecision') { // 
                val = chb.getValue();
                frm.down('grid').setDisabled(!val);
            } else {
                val = chb.getValue();
                fields = Ext.ComponentQuery.query('[col=right] field', frm);
                Ext.each(fields, function (f) {
                    f.setDisabled(!val);
                });
            }
        });
    },

    loadValues: function (values) {
        var win = this,
            mainForm,
            subforms,
            subformsHash = {};

        mainForm = win.down('form');
        subforms = Ext.ComponentQuery.query('form', mainForm);
        Ext.each(subforms, function (sfr) {
            subformsHash[sfr.entity] = sfr;
        });

        win.__initDisable();
        if (values) {
            Ext.Object.each(values, function (key, value) {
                var frm = subformsHash[key];
                if (frm) {
                    frm.setValues(value);
                    frm.setValues(value);
                }
            });
        }
        if (values.Protocol && values.Protocol.ManOrgName) {
            mainForm.down('[name=ManOrgName]').setValue(values.Protocol.ManOrgName);
        }
    }
});