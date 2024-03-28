Ext.define('B4.view.actcheck.RealityObjectEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    minWidth: 400,
    minHeight: 400,
    autoScroll: true,
    bodyPadding: 5,
    itemId: 'actCheckRealityObjectEditWindow',
    title: 'Форма редактирования результатов проверки',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNoNotSet',
        'B4.view.actcheck.ViolationGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 190,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4combobox',
                    itemId: 'cbHaveViolation',
                    floating: false,
                    width: 250,
                    name: 'HaveViolation',
                    fieldLabel: 'Нарушения выявлены',
                    displayField: 'Display',
                    items: B4.enums.YesNoNotSet.getItems(),
                    valueField: 'Value',
                    editable: false
                },
                {
                    xtype: 'textarea',
                    itemId: 'taNotRevealedViolations',
                    name: 'NotRevealedViolations',
                    fieldLabel: 'Нарушения не выявлены',
                    maxLength: 500,
                    height: 70
                },
                {
                    xtype: 'fieldset',
                    title: 'Сведения о МКД',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'textarea',
                        labelWidth: 180,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            name: 'TechPassportChars',
                            fieldLabel: 'Характеристики из тех. паспорта',
                            readOnly: true
                        },
                        {
                            name: 'AdditionalChars',
                            fieldLabel: 'Дополнительные характеристики'
                        }
                    ]
                },
                {
                    xtype: 'actCheckViolationGrid',
                    flex: 1
                },
                {
                    xtype: 'textarea',
                    padding: '5 0 0 0',
                    itemId: 'taDescription',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    labelWidth: 80
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
                                {
                                    xtype: 'b4savebutton',
                                    itemId: 'actRealObjEditWindowSaveButton'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
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