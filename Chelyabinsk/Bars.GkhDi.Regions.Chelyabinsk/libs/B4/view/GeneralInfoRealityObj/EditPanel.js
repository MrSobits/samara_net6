Ext.define('B4.view.generalinforealityobj.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: 2,
    trackResetOnLoad: true,
    autoScroll: true,
    title: 'Общие сведения о доме (для рейтинга)',
    itemId: 'generalInfoRealityObjEditPanel',
    layout: 'anchor',
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right',
                        labelWidth: 420
                    },
                    title: 'Выплата по искам по договорам обслуживания',
                    items: [
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'ClaimCompensationDamage',
                            fieldLabel: 'Иски по компенсации нанесенного ущерба (тыс. руб.)'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'ClaimReductionPaymentNonService',
                            fieldLabel: 'Иски по снижению платы в связи с неоказанием услуг(тыс. руб.)'
                        },
                        {
                            xtype: 'gkhdecimalfield',
                            name: 'ClaimReductionPaymentNonDelivery',
                            fieldLabel: 'Иски по снижению платы в связи с недопоставкой ресурсов (тыс. руб.)'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    defaults: {
                        anchor: '100%',
                        labelAlign: 'right',
                        labelWidth: 150
                    },
                    title: 'Информация об услугах (работах) и обязательствах управляющей организации',
                    items: [
                        {
                            xtype: 'textarea',
                            name: 'ExecutionWork',
                            fieldLabel: 'Выполняемые работы (оказываемые услуги)',
                            maxLength: 3000
                        },
                        {
                            xtype: 'b4filefield',
                            fieldLabel: 'Файл',
                            name: 'FileExecutionWork',
                            padding: '0 0 15 0'
                        },
                        {
                            xtype: 'textarea',
                            name: 'ExecutionObligation',
                            fieldLabel: 'Выполнение обязательств',
                            maxLength: 3000
                        },
                        {
                            xtype: 'b4filefield',
                            fieldLabel: 'Файл',
                            name: 'FileExecutionObligation',
                            padding: '0 0 15 0'
                        },
                        {
                            xtype: 'textarea',
                            name: 'DescriptionServiceCatalogRepair',
                            fieldLabel: 'Стоимость услуг',
                            maxLength: 3000
                        },
                        {
                            xtype: 'b4filefield',
                            fieldLabel: 'Файл',
                            name: 'FileServiceCatalogRepair',
                            padding: '0 0 15 0'
                        },
                        {
                            xtype: 'textarea',
                            name: 'DescriptionTariffCatalogRepair',
                            fieldLabel: 'Тарифы',
                            maxLength: 3000
                        },
                        {
                            xtype: 'b4filefield',
                            fieldLabel: 'Файл',
                            name: 'FileTariffCatalogRepair',
                            padding: '0 0 15 0'
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
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
