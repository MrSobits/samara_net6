Ext.define('B4.view.complaints.StepEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 500,
    minHeight: 250,
    bodyPadding: 5,
    itemId: 'complaintsStepEditWindow',
    title: 'Этап рассмотрения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.form.EnumCombo',
        'B4.enums.DOPetitionResult',
        'B4.enums.DOTypeStep',
        'B4.enums.YesNo',
        'B4.enums.TypeAnnex',
        'B4.ux.button.Save',
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4enumcombo',
                    anchor: '100%',
                    fieldLabel: 'Тип этапа',
                    enumName: 'B4.enums.DOTypeStep',
                    name: 'DOTypeStep'
                },
                {
                    xtype: 'b4enumcombo',
                    anchor: '100%',
                    fieldLabel: 'Результат',
                    enumName: 'B4.enums.DOPetitionResult',
                    name: 'DOPetitionResult'
                },               
                {
                    xtype: 'datefield',
                    name: 'NewDate',
                    fieldLabel: 'Новая дата',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'b4filefield',
                    name: 'FileInfo',
                    fieldLabel: 'Файл',
                    editable: false
                },
                {
                    xtype: 'textarea',
                    name: 'Reason',
                    fieldLabel: 'Основание',
                    maxLength: 1500,
                    flex: 1
                },
                {
                    xtype: 'textarea',
                    name: 'AddDocList',
                    fieldLabel: 'Список документов',
                    maxLength: 1500,
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
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