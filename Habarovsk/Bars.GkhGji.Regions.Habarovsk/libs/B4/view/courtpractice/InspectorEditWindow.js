Ext.define('B4.view.courtpractice.InspectorEditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 450,
    minWidth: 400,
    minHeight: 200,
    height: 200,
    bodyPadding: 5,
    itemId: 'courtpracticeInspectorEditWindow',
    title: 'Форма редактирования',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.LawyerInspector',
        'B4.store.dict.Inspector'

    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 130,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'combobox',
                    name: 'LawyerInspector',
                    fieldLabel: 'Тип ДЛ',
                    displayField: 'Display',
                    itemId: 'cbLawyerInspector',
                    labelWidth: 90,
                    flex: 1,
                    store: B4.enums.LawyerInspector.getStore(),
                    valueField: 'Value',
                    allowBlank: false,
                    editable: false
                },           
                {
                    xtype: 'b4selectfield',
                    labelAlign: 'right',
                    anchor: '100%',
                    store: 'B4.store.dict.Inspector',
                    name: 'Inspector',
                    itemId: 'sfInspector',
                    textProperty: 'Fio',
                    flex: 1,
                    editable: false,
                    fieldLabel: 'Должностное лицо',
                    columns: [
                        { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Описание', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'textarea',
                    name: 'Discription',
                    fieldLabel: 'Комментарий',
                    maxLength: 1500
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