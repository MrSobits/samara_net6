Ext.define('B4.view.preventiveaction.AddWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 600,
    bodyPadding: 5,
    alias: 'widget.preventiveactionaddwindow',
    itemId: 'preventiveactionaddwindow',
    title: 'Профилактическое мероприятие',
    trackResetOnLoad: true,
    closeAction: 'hide',

    requires: [
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.enums.PreventiveActionType',
        'B4.enums.PreventiveActionVisitType',
        'B4.store.dict.ZonalInspection',
        'B4.store.dict.Municipality',
        'B4.store.dict.PlanActionGji'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right',
                allowBlank: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Municipality',
                    name: 'Municipality',
                    fieldLabel: 'Муниципальное образование',
                    textProperty: 'Name',
                    editable: false,
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }],
                    listeners: {
                        beforeload: function (field, options) {
                            options.params.useAuthFilter = true;
                        }
                    }
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.ZonalInspection',
                    name: 'ZonalInspection',
                    fieldLabel: 'Орган ГЖИ',
                    textProperty: 'ZoneName',
                    editable: false,
                    columns: [{ text: 'Наименование', dataIndex: 'ZoneName', flex: 1 }],
                    listeners: {
                        beforeload: function (field, options) {
                            options.params.useAuthFilter = true;
                        }
                    }
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.PlanActionGji',
                    name: 'Plan',
                    fieldLabel: 'План',
                    allowBlank: true,
                    editable: false,
                    columns: [{ text: 'Наименование', dataIndex: 'Name', flex: 1 }]
                },
                {
                    xtype: 'b4enumcombo',
                    fieldLabel: 'Вид мероприятия',
                    enumName: 'B4.enums.PreventiveActionType',
                    name: 'ActionType',
                },
                {
                    xtype: 'b4enumcombo',
                    fieldLabel: 'Тип визита',
                    enumName: 'B4.enums.PreventiveActionVisitType',
                    name: 'VisitType',
                },
                {
                    xtype: 'datefield',
                    fieldLabel: 'Дата',
                    name: 'DocumentDate'
                },
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
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
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