Ext.define('B4.view.baseomsu.MainInfoTabPanel', {
    extend: 'Ext.form.Panel',
    layout: { type: 'vbox', align: 'stretch' },
    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.dict.PlanJurPersonGji',
        'B4.store.dict.Inspector',
        'B4.store.dict.ZonalInspection',
        'B4.view.Control.GkhTriggerField',
        
        'B4.enums.TypeBaseOMSU',
        'B4.enums.TypeFactInspection',
        'B4.enums.TypeFormInspection'
    ],
    alias: 'widget.baseomsumaininfotabpanel',
    itemId: 'mainInfoTabPanel',
    title: 'Основная информация',
    bodyStyle: Gkh.bodyStyle,
    defaults: {
        labelWidth: 250,
        margin: 3,
        labelAlign: 'right'
    },
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            items: [
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        pack: 'start',
                        type: 'hbox'
                    },
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            name: 'UriRegistrationNumber',
                            fieldLabel: 'Учетный номер проверки в едином реестре проверок',
                            labelWidth: 250
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 220,
                            name: 'UriRegistrationDate',
                            fieldLabel: 'Дата присвоения учетного номера'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        pack: 'start',
                        type: 'hbox'
                    },
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            itemId: 'dfDateStart',
                            xtype: 'datefield',
                            labelWidth: 250,
                            name: 'DateStart',
                            fieldLabel: 'Дата начала проверки',
                            allowBlank: false
                        },
                        {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            mouseWheelEnabled: false,
                            name: 'CountDays',
                            itemId: 'nfCountDays',
                            fieldLabel: 'Срок проверки (количество дней)',
                            labelWidth: 220
                        }
                    ]
                },
                {
                    xtype: 'combobox', editable: false,
                    name: 'TypeBaseOMSU',
                    itemId: 'cbTypeBaseOMSU',
                    fieldLabel: 'Основание проверки',
                    labelAlign: 'right',
                    displayField: 'Display',
                    store: B4.enums.TypeBaseOMSU.getStore(),
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        pack: 'start',
                        type: 'hbox'
                    },
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'combobox', editable: false,
                            labelWidth: 250,
                            name: 'TypeFact',
                            fieldLabel: 'Факт проверки',
                            displayField: 'Display',
                            store: B4.enums.TypeFactInspection.getStore(),
                            valueField: 'Value',
                            itemId: 'cbTypeFactInspection',
                            editable: false
                        },
                        {
                            xtype: 'textfield',
                            labelWidth: 220,
                            name: 'Reason',
                            fieldLabel: 'Причина',
                            itemId: 'tfReason',
                            maxLength: 500
                        }
                    ]
                },
                {
                    xtype: 'combobox', editable: false,
                    name: 'TypeForm',
                    itemId: 'cbTypeForm',
                    fieldLabel: 'Форма проверки',
                    displayField: 'Display',
                    store: B4.enums.TypeFormInspection.getStore(),
                    valueField: 'Value'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'JurPersonInspectors',
                    itemId: 'trigfInspectors',
                    fieldLabel: 'Инспекторы'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'JurPersonZonalInspections',
                    itemId: 'trigfZonalInspections',
                    fieldLabel: 'Отделы'
                }
            ]
        });

        me.callParent(arguments);
    }
});