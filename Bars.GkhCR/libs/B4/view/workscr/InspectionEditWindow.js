Ext.define('B4.view.workscr.InspectionEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyPadding: 5,

    title: 'Обследование объекта КР',
    alias: 'widget.workscrinspectionwin',
    itemId: 'inspectionTypeWorkCrEditWindow',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.InspectionState',
        'B4.form.FileField',
        'B4.store.dict.Official'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Номер',
                            name: 'Number'
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Плановая дата',
                            name: 'PlanDate',
                            format: 'd.m.Y',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'b4combobox',
                            fieldLabel: 'Факт обследования',
                            name: 'InspectionState',
                            editable: false,
                            items: B4.enums.InspectionState.getItems(),
                            value: 10
                        },
                        {
                            xtype: 'datefield',
                            fieldLabel: 'Фактическая дата',
                            name: 'FactDate',
                            format: 'd.m.Y'
                        }
                    ]
                },
                {
                    xtype: 'textfield',
                    fieldLabel: 'Причина',
                    name: 'Reason'
                },
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Ответственный',
                    name: 'Official',
                    textProperty: 'Fio',
                    editable: false,
                    store: 'B4.store.dict.Official'
                },
                {
                    xtype: 'b4filefield',
                    fieldLabel: 'Файл',
                    name: 'File',
                    editable: false
                },
                {
                    xtype: 'textarea',
                    fieldLabel: 'Описание',
                    name: 'Description',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});