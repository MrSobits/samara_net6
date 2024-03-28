Ext.define('B4.view.regop.owner.ActivityStageEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    width: 500,
    bodyPadding: 5,

    alias: 'widget.activitystageeditwinowner',

    title: 'Добавление записи',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.EnumCombo',
        'B4.enums.ActivityStageType'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'datefield',
                    fieldLabel: 'Дата начала',
                    format: 'd.m.Y',
                    name: 'DateStart',
                    allowBlank: false,
                    maxValue: new Date()
                },
                {
                    xtype: 'datefield',
                    fieldLabel: 'Дата окончания',
                    format: 'd.m.Y',
                    disabled: true,
                    name: 'DateEnd'
                },
                {
                    xtype: 'b4enumcombo',
                    name: 'ActivityStageType',
                    fieldLabel: 'Стадия',
                    enumName: 'B4.enums.ActivityStageType',
                    allowBlank: false,
                    enumItems: [
                       B4.enums.ActivityStageType.Bankrupt,
                       B4.enums.ActivityStageType.Affordable
                    ]
                },
                {
                    xtype: 'b4filefield',
                    fieldLabel: 'Документ',
                    name: 'Document',
                    editable: false
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    allowBlank: true,
                    maxLength: 500
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