Ext.define('B4.view.basedisphead.MainInfoTabPanel', {
    extend: 'Ext.form.Panel',
    layout: { type: 'vbox', align: 'stretch' },
    requires: [
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.Inspector',
        'B4.store.DocumentGji',
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.store.Contragent',
        'B4.enums.TypeBaseDispHead',
        'B4.enums.TypeDocumentGji',
        'B4.enums.TypeFormInspection'
    ],
    alias: 'widget.basedispheadmaininfotabpanel',
    itemId: 'mainInfoTabPanel',
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
                    name: 'Head',
                    itemId: 'sflHead',
                    fieldLabel: 'Руководитель',
                    textProperty: 'Fio',
                   

                    store: 'B4.store.dict.Inspector',
                    allowBlank: false,
                    editable: false,
                    columns: [
                        { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Должность', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'dispHeadInspectors',
                    itemId: 'trfInspectors',
                    fieldLabel: 'Инспекторы'
                },
                {
                    xtype: 'combobox',
                    itemId: 'cbTypeBase',
                    name: 'TypeBaseDispHead',
                    fieldLabel: 'Основание',
                    displayField: 'Display',
                    store: B4.enums.TypeBaseDispHead.getStore(),
                    allowBlank: false,
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'b4selectfield',
                    itemId: 'dispHeadPrevDocumentSelectField',
                    name: 'PrevDocument',
                    fieldLabel: 'Предыдущий документ',
                    textProperty: 'DocumentNumber',
                   

                    store: 'B4.store.DocumentGji',
                    editable: false,
                    columns: [
                        { xtype: 'datecolumn', dataIndex: 'DocumentDate', text: 'Дата', format: 'd.m.Y', width: 100, filter: { xtype: 'datefield' } },
                        { xtype: 'gridcolumn', text: 'Номер', dataIndex: 'DocumentNumber', flex: 1, filter: { xtype: 'textfield' } },
                        {
                            xtype: 'gridcolumn',
                            text: 'Тип документа',
                            dataIndex: 'TypeDocumentGji',
                            flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                displayField: 'Display',
                                valueField: 'Value',
                                emptyItem: { Name: '-' },
                                store: B4.enums.TypeDocumentGji.getStore()
                            },
                            renderer: function (val) {
                                return B4.enums.TypeDocumentGji.displayRenderer(val);
                            }
                        }
                    ]
                },
                {
                    xtype: 'combobox',
                    name: 'TypeForm',
                    itemId: 'cbTypeForm',
                    fieldLabel: 'Форма проверки',
                    displayField: 'Display',
                    store: B4.enums.TypeFormInspection.getStore(),
                    valueField: 'Value',
                    allowBlank: false,
                    editable: false
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
                            itemId: 'tfDocumentName',
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
                                    itemId: 'tfDocumentNumber',
                                    fieldLabel: 'Номер',
                                    maxLength: 50
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    itemId: 'dfDocumentDate',
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