Ext.define('B4.view.actisolated.RealityObjectEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.actisolatedrealityobjecteditwindow',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 700,
    minHeight: 400,
    bodyPadding: 5,
    title: 'Форма редактирования результатов проверки',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNoNotSet',
        'B4.view.actisolated.ViolationGrid',
        'B4.view.actisolated.EventGrid',
        'B4.view.actisolated.MeasureGrid'
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
                    xtype: 'actisolatedeventgrid',
                    height: 200,
                    flex: 1
                },
                {
                    xtype: 'actisolatedviolationgrid',
                    height: 200,
                    flex: 1
                },
                {
                    xtype: 'actisolatedmeasuregrid',
                    height: 200,
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