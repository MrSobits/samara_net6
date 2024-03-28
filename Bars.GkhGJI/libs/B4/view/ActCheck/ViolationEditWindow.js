Ext.define('B4.view.actcheck.ViolationEditWindow', {
    extend: 'B4.form.Window',

    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'actCheckViolationEditWindow',
    closeAction: 'hide',
    mixins: [ 'B4.mixins.window.ModalMask' ],

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
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
                    xtype: 'b4selectfield',
                    name: 'InspectionViolation',
                    textProperty: 'ViolationGji',
                    fieldLabel: 'Нарушение',
                    editable: false,
                    readOnly: true
                },
                {
                    xtype: 'datefield',
                    name: 'DatePlanRemoval',
                    fieldLabel: 'Срок устранения',
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
                                    xtype: 'b4closebutton',
                                    itemId: 'actCheckViolationCloseBtn'
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