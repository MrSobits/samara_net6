Ext.define('B4.view.dict.workkindcurrentrepair.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.store.dict.UnitMeasure',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.unitmeasure.Grid',
        'B4.enums.TypeWork'
    ],
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 500,
    bodyPadding: 5,
    itemId: 'workKindCurrentRepairEditWindow',
    title: 'Вид работы текущего ремонта',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Code',
                    fieldLabel: 'Код',
                    anchor: '100%',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
                    anchor: '100%',
                    allowBlank: false,
                    maxLength: 300
                },
                {
                    xtype: 'b4selectfield',
                    name: 'UnitMeasure',
                    fieldLabel: 'Ед. измерения',
                    anchor: '100%',
                    store: 'B4.store.dict.UnitMeasure',
                    editable: false,
                    allowBlank: false
                },
                {
                    xtype: 'combobox', editable: false,
                    fieldLabel: 'Тип работ',
                    store: B4.enums.TypeWork.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'TypeWork'
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