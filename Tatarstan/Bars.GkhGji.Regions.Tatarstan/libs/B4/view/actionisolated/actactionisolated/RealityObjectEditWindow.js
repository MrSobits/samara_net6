Ext.define('B4.view.actionisolated.actactionisolated.RealityObjectEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.actactionisolatedrealityobjecteditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    minHeight: 400,
    bodyPadding: 5,
    itemId: 'actActionIsolatedRealityObjectEditWindow',
    title: 'Форма редактирования результатов мероприятия',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNoNotSet',
        'B4.view.actcheck.ViolationGrid',
        'B4.form.EnumCombo'
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
                    xtype: 'textfield',
                    floating: false,
                    width: 250,
                    name: 'RealityObject',
                    fieldLabel: 'Адрес дома',
                    readOnly: true,
                    valueToRaw: function(value){
                        return value?.Address;
                    }
                },
                {
                    xtype: 'b4enumcombo',
                    itemId: 'cbHaveViolation',
                    floating: false,
                    width: 250,
                    name: 'HaveViolation',
                    fieldLabel: 'Нарушения выявлены',
                    enumName: 'B4.enums.YesNoNotSet',
                    editable: false
                },
                {
                    xtype: 'actCheckViolationGrid',
                    initAdditionalEditors: true,
                    flex: 1
                },
                {
                    xtype: 'textarea',
                    padding: '5 0 0 0',
                    itemId: 'taDescription',
                    name: 'Description',
                    fieldLabel: 'Описание',
                    labelWidth: 80,
                    maxLength: 2000
                }
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