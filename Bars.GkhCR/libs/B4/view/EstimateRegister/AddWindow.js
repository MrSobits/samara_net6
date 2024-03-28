Ext.define('B4.view.estimateregister.AddWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 600,
    maxHeight: 160,
    minHeight: 160,
    bodyPadding: 5,
    itemId: 'estimateRegisterAddWindow',
    title: 'Сметный расчет',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires:
    [
        'B4.store.dict.ProgramCr',
        'B4.store.ObjectCr',
        'B4.store.objectcr.TypeWorkCr',

        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.Control.GkhTriggerField',
        'B4.form.ComboBox'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 140,
                allowBlank: false,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.ProgramCr',
                    itemId: 'sfProgramCr',
                    fieldLabel: 'Программа КР',
                    editable: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.ObjectCr',
                    name: 'ObjectCr',
                    textProperty: 'RealityObjName',
                    itemId: 'sfObjectCr',
                    fieldLabel: 'Объект КР',
                    disabled: true,
                    editable: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'RealityObjName', flex: 1 }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.objectcr.TypeWorkCr',
                    name: 'TypeWorkCr',
                    textProperty: 'WorkName',
                    itemId: 'sfTypeWorkCr',
                    fieldLabel: 'Вид работы',
                    disabled: true,
                    editable: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'WorkName', flex: 1 }
                    ]
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