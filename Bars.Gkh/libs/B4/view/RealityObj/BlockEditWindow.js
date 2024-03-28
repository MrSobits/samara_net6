Ext.define('B4.view.realityobj.BlockEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.blockeditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    minWidth: 350,
    width: 400,
    minHeight: 180,
    bodyPadding: 5,
    title: 'Блок',
    closeAction: 'hide',
    trackResetOnLoad: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 140
            },
            items:[
                {
                    fieldLabel: 'Номер блока',
                    name: 'Number',
                    xtype: 'textfield',
                    maskRe: /[a-zA-Zа-яА-Я0-9]/
                },
                {
                    fieldLabel: 'Общая площадь блока',
                    name: 'AreaTotal',
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: true,
                    minValue: 0,
                    negativeText: 'Значение не может быть отрицательным',
                    decimalSeparator: ','
                },
                {
                    fieldLabel: 'Жилая площадь',
                    name: 'AreaLiving',
                    xtype: 'numberfield',
                    hideTrigger: true,
                    keyNavEnabled: false,
                    mouseWheelEnabled: false,
                    allowDecimals: true,
                    minValue: 0,
                    negativeText: 'Значение не может быть отрицательным',
                    decimalSeparator: ','
                },
                {
                    fieldLabel: 'Кадастровый номер',
                    name: 'CadastralNumber',
                    xtype: 'textfield',
                    maskRe: /[0-9:]/
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
                            items: [{ xtype: 'b4savebutton' }]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [{ xtype: 'b4closebutton' }]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});