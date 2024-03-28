Ext.define('B4.view.actremoval.ViolationEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'actRemovalViolationEditWindow',
    title: 'Форма устранения нарушения',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    anchor: '100%',
                    name: 'InspectionViolation',
                    textProperty: 'ViolationGji',
                    fieldLabel: 'Нарушение',
                    labelAlign: 'right',
                    editable: false,
                    readOnly: true
                },
                {
                    xtype: 'datefield',
                    anchor: '100%',
                    name: 'DateFactRemoval',
                    fieldLabel: 'Дата факт. устранения',
                    labelAlign: 'right',
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