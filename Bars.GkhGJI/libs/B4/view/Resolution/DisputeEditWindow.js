Ext.define('B4.view.resolution.DisputeEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 600,
    minHeight: 300,
    bodyPadding: 5,
    itemId: 'resolutionDisputeEditWindow',
    title: 'Форма редактирования оспаривания',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.dict.CourtVerdictGji',
        'B4.store.dict.InstanceGji',
        'B4.store.dict.TypeCourtGji',
        'B4.store.dict.Inspector',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.enums.ResolutionAppealed'
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
                    xtype: 'combobox', editable: false,
                    name: 'Appeal',
                    fieldLabel: 'Постановление обжаловано',
                    displayField: 'Display',
                    store: B4.enums.ResolutionAppealed.getStore(),
                    valueField: 'Value',
                    itemId: 'cbDisputeAppeal'
                },
                {
                    xtype: 'checkboxfield',
                    name: 'ProsecutionProtest',
                    fieldLabel: 'Протест прокуратуры'
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        xtype: 'b4combobox',
                        labelAlign: 'right',
                        allowBlank: false,
                        editable: false,
                        fields: ['Id', 'Name', 'Code'],
                        queryMode: 'local',
                        triggerAction: 'all',
                        flex: 1
                    },
                    items: [
                        {
                            name: 'Court',
                            fieldLabel: 'Вид суда',
                            url: '/TypeCourtGji/List',
                            itemId: 'cbDisputeCourt',
                            labelWidth: 170
                        },
                        {
                            name: 'Instance',
                            fieldLabel: 'Инстанция',
                            url: '/InstanceGji/List',
                            itemId: 'cbDisputeInstance',
                            labelWidth: 100
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    editable: false,
                   

                    store: 'B4.store.dict.Inspector',
                    textProperty: 'Fio',
                    name: 'Lawyer',
                    fieldLabel: 'Юрист',
                    columns: [
                        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                        { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'fieldset',
                    padding: '5 5 0 5',
                    flex: 1,
                    defaults: {
                        labelWidth: 100,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    title: 'Документ решения',
                    items: [
                        {
                            xtype: 'b4selectfield',
                            editable: false,
                           

                            store: 'B4.store.dict.CourtVerdictGji',
                            textProperty: 'Name',
                            name: 'CourtVerdict',
                            fieldLabel: 'Решение суда',
                            columns: [
                                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1 },
                                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1 }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                anchor: '100%',
                                labelAlign: 'right',
                                labelWidth: 100,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNum',
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
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Описание',
                            maxLength: 500,
                            anchor: '100% -80'
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
                        { xtype: 'tbfill' },
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