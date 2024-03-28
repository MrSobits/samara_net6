Ext.define('B4.view.realestatetype.Edit', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.realestatetypeedit',

    requires: [
        'Ext.tab.Panel',
        'B4.ux.button.Save',
        'Ext.form.field.Text',
        'Ext.form.field.Hidden',
        'Ext.form.Panel',

        'B4.view.realestatetype.CommonParamGrid',
        'B4.view.realestatetype.StructElemGrid'
    ],
    // Id редактируемого RealEstateType
    objId: 0,

    title: 'Редактирование типа дома',
    closable: true,


    initComponent: function () {
        var me = this;

        Ext.apply(me, {
            layout: {
                type: 'vbox',
                align: 'stretch',
                pack: 'start'
            },
            items: [
                {
                    xtype: 'form',
                    border: 0,
                    items: [
                        {
                            xtype: 'hiddenfield',
                            name: 'Id'
                        },
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            allowBlank: false,
                            width: 500,
                            margin: '10 10 10 10',
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            name: 'Code',
                            fieldLabel: 'Код',
                            width: 500,
                            margin: '10 10 10 10',
                            maxLength: 100
                        }
                    ],
                    tbar: {
                        items: [
                            {
                                xtype: 'b4savebutton'
                            }
                        ]
                    }
                },
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    items: [
                        {
                            title: 'Общие характеристики',
                            xtype: 'commonparameditgrid'
                        },
                        {
                            title: 'Конструктивные элементы',
                            xtype: 'structelemeditgrid'
                        },
                        {
                            title: 'Параметры очередности',
                            xtype: 'realestatetypepriorityparamgrid',
                            hidden: true
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});