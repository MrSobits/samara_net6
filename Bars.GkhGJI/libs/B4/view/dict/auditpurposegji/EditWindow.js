Ext.define('B4.view.dict.auditpurposegji.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    height: 500,
    bodyPadding: 5,
    itemId: 'auditPurposeGjiEditWindow',
    title: 'Цель проведения проверки',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.AuditPurposeSurveySubjectGji.Grid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'left'
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Цель',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'auditPurposeSurveySubjectGjiGrid',
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