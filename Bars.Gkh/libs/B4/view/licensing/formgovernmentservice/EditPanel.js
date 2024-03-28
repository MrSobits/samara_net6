Ext.define('B4.view.licensing.formgovernmentservice.EditPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.formgovernmentserviceeditpanel',

    closable: true,
    bodyPadding: 5,
    title: 'Сведения о представлении государственных услуг (Форма 1-ГУ)',

    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Save',
        'B4.ux.button.Close',

        'B4.enums.FormGovernmentServiceType',
        'B4.enums.Quarter',

        'B4.view.licensing.formgovernmentservice.DetailForm'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyStyle: Gkh.bodyStyle,

    initComponent: function () {
        var me = this,
            years = [],
            i,
            currentYear = new Date().getFullYear();

        for (i = -1; i < 10; i++) {
            years.push([currentYear + i, currentYear + i]);
        }

        Ext.applyIf(me, {
            items: [
                {
                    // шапка
                    xtype: 'container',
                    layout: 'vbox',
                    width: 500,
                    defaults: {
                        labelWidth: 150,
                        anchor: '100%',
                        labelAlign: 'right',
                        readOnly: true
                    },
                    items: [
                        {
                            xtype: 'b4combobox',
                            name: 'GovernmentServiceType',
                            fieldLabel: 'Государственная услуга',
                            items: B4.enums.FormGovernmentServiceType.getItems(),
                            readOnly: true
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            margin: '0 0 10px 0',
                            defaults: {
                                readOnly: true
                            },
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    name: 'Year',
                                    labelAlign: 'right',
                                    labelWidth: 150,
                                    fieldLabel: 'Отчётный период',
                                    items: years,
                                    flex: 1
                                },
                                {
                                    xtype: 'b4combobox',
                                    name: 'Quarter',
                                    items: B4.enums.Quarter.getItems(),
                                    flex: 0.7,
                                    margin: '0 0 0 15px',
                                    editable: false
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    name: 'Details',
                    flex: 1,
                    items: [
                        {
                            xtype: 'formgovernmentservicedetailform',
                            title: 'Раздел 1',
                            type: B4.enums.ServiceDetailSectionType.PublicServices
                        },
                        {
                            xtype: 'formgovernmentservicedetailform',
                            title: 'Раздел 2',
                            type: B4.enums.ServiceDetailSectionType.ServiceDelivery
                        },
                        {
                            xtype: 'formgovernmentservicedetailform',
                            title: 'Раздел 3',
                            type: B4.enums.ServiceDetailSectionType.ServiceTime
                        },
                        {
                            xtype: 'formgovernmentservicedetailform',
                            title: 'Раздел 4',
                            type: B4.enums.ServiceDetailSectionType.AppealAndDecisions
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
                                    xtype: 'button',
                                    action: 'export',
                                    text: 'Экспорт',
                                    iconCls: 'icon-table-go'
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