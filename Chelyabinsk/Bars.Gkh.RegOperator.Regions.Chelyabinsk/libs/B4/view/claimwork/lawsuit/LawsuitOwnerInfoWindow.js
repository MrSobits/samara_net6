﻿Ext.define('B4.view.claimwork.lawsuit.LawsuitOwnerInfoWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.lawsuitownerinfowindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 590,
    minWidth: 590,
    bodyPadding: 5,
    title: 'Добавление собственника',
    closeAction: 'destroy',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.regop.PersonalAccountOwnerType',
        'B4.store.regop.ChargePeriod',
        'B4.store.claimwork.AccountDetail',
        'B4.store.dict.JurInstitution',
        'B4.store.regop.personal_account.OwnerInformation',
        'B4.store.claimwork.lawsuit.LawsuitOwnerInfoAddInfo',
        'B4.view.claimwork.LawsuitOwnerRepresentativeGrid'
    ],
    initComponent: function () {
        var me = this,
            proxy = {
                type: 'b4proxy',
                controllerName: 'ChargePeriod',
                listAction: 'ListPeriodsByPersonalAccount'
            },
            startPeriodStore = Ext.create('B4.store.regop.ChargePeriod', {
                proxy: proxy
            }),
            endPeriodStore = Ext.create('B4.store.regop.ChargePeriod', {
                proxy: proxy
            });

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 180,
                allowBlank: false,
                padding: '5 22 0 0',
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'form',
                    name: 'Individual',
                    disabled: true,
                    hidden: true,
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 180,
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Surname',
                            fieldLabel: 'Фамилия'
                        },
                        {
                            xtype: 'textfield',
                            name: 'FirstName',
                            fieldLabel: 'Имя'
                        },
                        {
                            xtype: 'textfield',
                            name: 'SecondName',
                            fieldLabel: 'Отчество',
                            allowBlank: true
                        },
                        {//Несов
                            store: 'B4.store.claimwork.lawsuit.LawsuitOwnerInfoAddInfo',
                            xtype: 'checkbox',
                            name: 'Underage',
                            dataIndex: 'Underage',
                            fieldLabel: 'Несовершеннолетний'
                        },
                        {
                            xtype: 'textfield',
                            name: 'SNILS',
                            dataIndex: 'SNILS',
                            fieldLabel: 'СНИЛС',
                            allowBlank: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocIndName',
                            dataIndex: 'DocIndName',
                            fieldLabel: 'Документ',
                            allowBlank: true
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            defaults: {
                                labelAlign: 'right',
                                allowBlank: false
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocIndSerial',
                                    dataIndex: 'DocIndSerial',
                                    fieldLabel: 'Серия',
                                    labelWidth: 180,
                                    allowBlank: true,
                                    style: 'width:270px'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DocIndNumber',
                                    dataIndex: 'DocIndNumber',
                                    fieldLabel: 'Номер',
                                    labelWidth: 70,
                                    allowBlank: true
                                }
                            ]},
                        {
                            xtype: 'datefield',
                            name: 'DocIndDate',
                            //dataIndex: 'DocIndDate',
                            fieldLabel: 'Дата выдачи документа',
                            format: 'd.m.Y',
                            allowBlank: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocIndIssue',
                            dataIndex: 'DocIndIssue',
                            fieldLabel: 'Организация, выдавшая документ',
                            allowBlank: true
                        },
                        {
                            xtype: 'container',
                            layout: 'form',
                            defaults: {
                                labelAlign: 'right',
                                allowBlank: false,
                                padding: '5 22 0 0',
                                labelWidth: 180
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'LivePlace',
                                    dataIndex: 'LivePlace',
                                    text: 'Адрес проживания',
                                    fieldLabel: 'Адрес проживания',
                                    allowBlank: true
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.JurInstitution',
                                    name: 'JurInstitution',
                                    fieldLabel: 'Суд',
                                    //labelWidth: 170,
                                    labelAlign: 'right',
                                    allowBlank: true,
                                    textProperty: 'ShortName',
                                    columns: [
                                        { dataIndex: 'Municipality', text: 'Муниципальное образование', flex: 1, filter: { xtype: 'textfield' } },
                                        { dataIndex: 'ShortName', text: 'Краткое наименование', flex: 1, filter: { xtype: 'textfield' } }
                                    ],
                                    listeners: {
                                        'beforeload': function (store, operation) {
                                            operation.params['type'] = 10;
                                        }
                                    },
                                    editable: false
                                },
                            ]
                        },
                    ]
                },


                {
                    xtype: 'container',
                    layout: 'form',
                    name: 'Legal',
                    disabled: true,
                    hidden: true,
                    flex: 1,
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 180,
                        flex: 1,
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'ContragentName',
                            fieldLabel: 'Наименование контрагента'
                        },
                        {
                            xtype: 'textfield',
                            name: 'Inn',
                            fieldLabel: 'ИНН'
                        },
                        {
                            xtype: 'textfield',
                            name: 'Kpp',
                            fieldLabel: 'КПП'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'form',
                    name: 'CalcPeriod',
                    flex: 1,
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 180,
                        flex: 1,
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.claimwork.AccountDetail',
                            name: 'PersonalAccount',
                            fieldLabel: 'Номер ЛС',
                            textProperty: 'PersonalAccountNum',
                            columns: [
                                {
                                    text: 'Адрес',
                                    xtype: 'gridcolumn',
                                    dataIndex: 'RoomAddress',
                                    flex: 3,
                                    filter: {xtype: 'textfield'}
                                },
                                {
                                    text: 'Номер ЛС',
                                    xtype: 'gridcolumn',
                                    dataIndex: 'PersonalAccountNum',
                                    flex: 1,
                                    filter: {xtype: 'textfield'}
                                }
                            ],
                            onSelectValue: function () {
                                var me = this,
                                    rec = me.gridView.getSelectionModel().getSelected() || [],
                                    selectedRecord = {};

                                if (rec.length === 0) {
                                    Ext.Msg.alert('Ошибка', 'Необходимо выбрать запись!');
                                    return;
                                }

                                if (Ext.isEmpty(rec[0])) {
                                    Ext.Msg.alert('Ошибка', 'Необходимо выбрать запись!');
                                    return;
                                }
                                selectedRecord = rec[0].getData();
                                me.setValue({
                                    Id: selectedRecord.AccountId,
                                    PersonalAccountNum: selectedRecord.PersonalAccountNum
                                });

                                me.onSelectWindowClose();
                            }
                        },
                        {
                            fieldLabel: 'Номер заявления',
                            xtype: 'textfield',
                            name: 'ClaimNumber',
                            dataIndex: 'ClaimNumber',
                            flex: 1,
                            allowBlank: true
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'form',
                    name: 'CalcPeriod',
                    flex: 1,
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 180,
                        flex: 1,
                        allowBlank: false
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            store: startPeriodStore,
                            name: 'StartPeriod',
                            fieldLabel: 'Период с',
                            textProperty: 'Name',
                            disabled: true,
                            columns: [
                                {
                                    text: 'Период',
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Name',
                                    flex: 3,
                                    filter: { xtype: 'textfield' }
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            store: endPeriodStore,
                            name: 'EndPeriod',
                            fieldLabel: 'Период по',
                            textProperty: 'Name',
                            disabled: true,
                            columns: [
                                {
                                    text: 'Период',
                                    xtype: 'gridcolumn',
                                    dataIndex: 'Name',
                                    flex: 3,
                                    filter: { xtype: 'textfield' }
                                }
                            ]
                        }
                    ]
                },
                {//Совместное им
                    xtype: 'checkbox',
                    store: 'B4.store.claimwork.lawsuit.LawsuitOwnerInfoAddInfo',
                    name: 'SharedOwnership',
                    fieldLabel: 'Совместное имущество',
                    labelWidth: 170,
                    labelAlign: 'right',
                    listeners: {
                        change: function (cb, checked) {
                            Ext.getCmp('AreaShareNumerator').setDisabled(checked);
                            Ext.getCmp('AreaShareDenominator').setDisabled(checked);
                        }
                    },
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        labelAlign: 'right',
                        allowDecimals: false,
                        mouseWheelEnabled: false,
                        allowBlank: false,
                    },
                    items: [
                        {
                            id: 'AreaShareNumerator',
                            fieldLabel: 'Доля собственности',
                            labelWidth: 180,
                            name: 'AreaShareNumerator',
                            minValue: 0,
                            width: 220,
                            minText: 'Значение этого поля должно быть больше 0'
                        },
                        {
                            id: 'AreaShareDenominator',
                            name: 'AreaShareDenominator',
                            fieldLabel: ' / ',
                            labelWidth: 10,
                            width: 50,
                            labelSeparator: '',
                            minValue: 1,
                            minText: 'Значение этого поля должно быть больше 0'
                        }
                    ]
                }
                // ,
                // {
                //     xtype: 'fieldset',
                //     height: 150,
                //     title: 'Законные представители',
                //     layout: {
                //         type: 'hbox'
                //
                //     },
                //     defaults: {
                //         labelWidth: 170,
                //         labelAlign: 'right',
                //         flex: 1
                //     },
                //     items: [
                //         {
                //             xtype: 'lawsuitownerrepgrid',
                //             height: 150
                //         }
                //     ]
                //
                // }

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
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4addbutton',
                                    name: 'FillFromAccount',
                                    text: 'Выбрать из абонентов'
                                }
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
    },

    showNameFields: function (ownerType) {
        var me = this,
            indFields = me.down('[name=Individual]'),
            legalFields = me.down('[name=Legal]');

        if (ownerType === B4.enums.regop.PersonalAccountOwnerType.Individual) {
            indFields.setVisible(true);
            indFields.setDisabled(false);

            legalFields.setVisible(false);
            legalFields.setDisabled(true);
        } else if (ownerType === B4.enums.regop.PersonalAccountOwnerType.Legal) {
            indFields.setVisible(false);
            indFields.setDisabled(true);

            legalFields.setVisible(true);
            legalFields.setDisabled(false);
        } else {
            Ext.Error.raise('Некорректный тип собственника');
        }

        me.getForm().isValid();
    }
});