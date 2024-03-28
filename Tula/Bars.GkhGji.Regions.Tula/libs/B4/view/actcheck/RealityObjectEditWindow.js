Ext.define('B4.view.actcheck.RealityObjectEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    minHeight: 600,
    bodyPadding: 5,
    itemId: 'actCheckRealityObjectEditWindow',
    title: 'Форма редактирования результатов проверки',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNoNotSet',
        'B4.view.actcheck.ViolationGrid',
        'B4.view.actcheck.ViolationGroupGrid'
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
                    xtype: 'tabpanel',
                    itemId: 'actCheckViolationTabPanel',
                    border: false,
                    flex: 1,
                    items: [
                        {
                            xtype: 'actCheckViolationGrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        },
                        {
                            xtype: 'actCheckViolationGroupGrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    padding: '5 0 0 0',
                    itemId: 'taDescription',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    labelWidth: 70,
                    maxLength: 1000
                },
                {
                    xtype: 'textarea',
                    padding: '5 0 0 0',
                    itemId: 'taNotRevealedViolations',
                    name: 'NotRevealedViolations',
                    fieldLabel: 'Невыявленные нарушения',
                    labelWidth: 70,
                    maxLength: 1000
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
                                    xtype: 'b4savebutton',
                                    itemId: 'actRealObjEditWindowSaveButton'
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