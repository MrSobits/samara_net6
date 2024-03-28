Ext.define('B4.view.dict.violationfeaturegji.EditFeatureWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.violationFeatureGjiEditWindow',

    mixins: ['B4.mixins.window.ModalMask'],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    width: 400,

    title: 'Добавление нарушения',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.store.dict.ViolationGji'
    ],

    bodyPadding: 6,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 115
            },
            items: [
                {
                    xtype: 'hidden',
                    allowBlank: false,
                    name: 'FeatureViolGji'
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.ViolationGji',
                    name: 'ViolationGji',
                    fieldLabel: 'Нарушение',
                    columns: [
                        {
                            dataIndex: 'CodePin',
                            widht: 70,
                            filter: { xtype: 'textfield' },
                            text: 'Пункт НПД'
                        },
                        {
                            dataIndex: 'Name',
                            filter: { xtype: 'textfield' },
                            flex: 1,
                            text: 'Текст нарушения'
                        }
                    ],
                    editable: false
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