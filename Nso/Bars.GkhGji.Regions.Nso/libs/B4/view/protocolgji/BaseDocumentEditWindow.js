Ext.define('B4.view.protocolgji.BaseDocumentEditWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.protocolgjiBaseDocumentEditWindow',
    
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    minHeight: 200,
    bodyPadding: 5,
    itemId: 'protocolgjiBaseDocumentEditWindow',
    title: 'Документ основание',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.form.SelectField',
        'B4.store.dict.KindBaseDocument',
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
                    store: 'B4.store.protocolgji.RealityObjViolation',
                    textProperty: 'RealityObject',
                    name: 'RealityObject',
                    anchor: '100%',
                    fieldLabel: 'Адрес МКД',
                    itemId: 'sfRealityObject',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'RealityObject', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.KindBaseDocument',
                    textProperty: 'Name',
                    name: 'KindBaseDocument',
                    anchor: '100%',
                    fieldLabel: 'Документ основание',
                    itemId: 'sfKindBaseDocument',
                    editable: false,
                    allowBlank: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Код', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'datefield',
                    name: 'DateDoc',
                    fieldLabel: 'Дата документа',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textfield',
                    name: 'NumDoc',
                    fieldLabel: 'Номер документа',
                    maxLength: 300
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
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
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