Ext.define('B4.view.warninginspection.MainInfoTabPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.warninginspectionmaininfotabpanel',

    layout: { type: 'vbox', align: 'stretch' },
    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.Inspector',
        'B4.store.dict.BaseDict',
        'B4.store.DocumentGji',
        'B4.store.Contragent',
        'B4.store.dict.ControlType',
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.form.EnumCombo',
        'B4.enums.InspectionCreationBasis',
        'B4.enums.TypeBase'
    ],

    title: 'Основная информация',
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    defaults: {
        labelWidth: 150,
        labelAlign: 'right'
    },

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'WarningInspectionControlType',
                    fieldLabel: 'Вид контроля',
                    store: 'B4.store.dict.ControlType',
                    textProperty: 'Name',
                    editable: false,
                    labelAlign: 'right',
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            header: 'Наименование вида контроля (надзора)',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'Inspectors',
                    fieldLabel: 'Инспекторы'
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'InspectionBasis',
                    fieldLabel: 'Основание',
                    enumName: 'B4.enums.InspectionCreationBasis',
                    readOnly: true
                },
                {
                    xtype: 'textfield',
                    name: 'citizensAppeal',
                    fieldLabel: 'Обращение гражданина',
                    readOnly: true,
                    hidden: true
                },
                {
                    xtype: 'fieldset',
                    layout: { type: 'vbox', align: 'stretch' },
                    title: 'Документ',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentName',
                            fieldLabel: 'Наименование',
                            labelAlign: 'right',
                            maxLength: 300
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
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер',
                                    maxLength: 50
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'File',
                            fieldLabel: 'Файл'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});