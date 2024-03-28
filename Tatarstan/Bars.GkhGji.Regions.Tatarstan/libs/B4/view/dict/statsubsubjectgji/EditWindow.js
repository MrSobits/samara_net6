Ext.define('B4.view.dict.statsubsubjectgji.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    width: 700,
    height: 500,
    bodyPadding: 5,
    itemId: 'statSubsubjectGjiEditWindow',
    title: 'Подтематика',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.view.dict.statsubsubjectgji.SubjectGrid',
        'B4.view.dict.statsubsubjectgji.FeatureGrid',
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
                    xtype: 'textfield',
                    name: 'Name',
                    fieldLabel: 'Наименование',
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
                    xtype: 'checkboxfield',
                    name: 'NeedInSopr',
                    fieldLabel: 'СОПР'
                },
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    layout: { type: 'vbox', align: 'stretch' },
                    items: [
                        {
                            //грид тематик
                            xtype: 'statSubsubjectSubjectGrid',
                            flex: 1
                        },
                        {
                            //грид характеристик 
                            xtype: 'statSubsubjectFeatureGrid',
                            flex: 1
                        }
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