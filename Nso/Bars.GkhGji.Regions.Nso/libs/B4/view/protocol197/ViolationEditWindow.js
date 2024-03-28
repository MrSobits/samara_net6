Ext.define('B4.view.protocol197.ViolationEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    bodyPadding: 5,
    title: 'Форма редактирования нарушения',
    closeAction: 'hide',
    trackResetOnLoad: true,
    alias: 'widget.protocol197violationwin',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
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
                    xtype: 'textfield',
                    name: 'CodesPin',
                    fieldLabel: 'Пункты НПД',
                    readOnly: true
                },
                {
                    xtype: 'textfield',
                    name: 'ViolationGjiName',
                    fieldLabel: 'Текст нарушения',
                    readOnly: true
                },
                {
                    xtype: 'textfield',
                    name: 'Features',
                    fieldLabel: 'Характеристика нарушения',
                    readOnly: true
                },
                {
                    xtype: 'textarea',
                    name: 'Description',
                    fieldLabel: 'Описание'
                },
                {
                    xtype: 'datefield',
                    name: 'DatePlanRemoval',
                    fieldLabel: 'Срок устранения',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'datefield',
                    name: 'DateFactRemoval',
                    fieldLabel: 'Дата факт. устранения',
                    format: 'd.m.Y'
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