Ext.define('B4.view.objectcr.TypeWorkCrStage1RemovalWindow', {
    extend: 'B4.form.Window',
    
    alias: 'widget.typeworkcrst1removalwin',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    defaults: {
        labelWidth: 180,
        labelAlign: 'right'
    },
    minHeight: 350,
    width: 900,
    minWidth: 900,
    maxWidth: 800,
    bodyPadding: 5,
    title: 'Причина удаления',
    closeAction: 'destroy',
    trackResetOnLoad: true,
    modal: true,

    requires: [
        'B4.enums.TypeWorkCrReason',
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'textfield',
                    name: 'WorkName',
                    fieldLabel: 'Вид работы',
                    readOnly: true
                },
                {
                    xtype: 'textfield',
                    name: 'StructElement',
                    fieldLabel: 'Конструктивный элемент',
                    readOnly: true
                },
                {
                    xtype: 'container',
                    style: 'border: 0px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px 30px 0 5px; padding: 5px 10px; line-height: 16px;',
                    html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Укажите причину удаления работы из программы и сохраните. После сохранения работа будет удалена из краткосрочной программы.</span>'
                },
                {
                    xtype: 'component',
                    height:5
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    floating: false,
                    name: 'TypeReason',
                    fieldLabel: 'Причина',
                    displayField: 'Display',
                    store: B4.enums.TypeWorkCrReason.getStore(),
                    valueField: 'Value',
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox'
                    },
                    padding: '0 0 5 0',
                    defaults: {
                        labelWidth: 180,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'numberfield',
                            name: 'YearRepair',
                            fieldLabel: 'Год выполнения по ДПКР',
                            hideTrigger: true,
                            allowDecimals: false,
                            minValue: 1800,
                            maxValue: 2100,
                            negativeText: 'Значение не может быть отрицательным'
                        },
                        {
                            xtype: 'numberfield',
                            name: 'NewYearRepair',
                            fieldLabel: 'Новый год выполнения',
                            hideTrigger: true,
                            allowDecimals: false,
                            allowBlank: true,
                            readOnly: true,
                            disabled:false,
                            minValue: 1800,
                            maxValue: 2100,
                            negativeText: 'Значение не может быть отрицательным'
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    name: 'Description',
                    fieldLabel: 'Документ (основание)',
                    maxLength: 100,
                    allowBlank: false
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'hbox'
                    },
                    padding: '0 0 5 0',
                    defaults: {
                        labelWidth: 180,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'NumDoc',
                            fieldLabel: 'Номер документа',
                            maxLength: 100
                        },
                        {
                            xtype: 'datefield',
                            allowBlank: false,
                            name: 'DateDoc',
                            fieldLabel: 'от',
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'FileDoc',
                    fieldLabel: 'Файл',
                    editable: false
                }
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
    }
});