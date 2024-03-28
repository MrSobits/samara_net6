Ext.define('B4.view.masscalc.CalcPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.masscalclongprogpanel',
    layout: 'vbox',
    title: 'Массовый расчет ДПКР',
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: '10 10 10 10',
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'fieldset',
                    title: 'Долгосрочная программа',
                    layout: 'hbox',
                    defaults: {
                        iconCls: 'icon-add',
                        margin: '0 10 10 0',
                        width: 200
                    },
                    items: [
                        {
                            xtype: 'button',
                            action: 'makelongprograms',
                            text: 'Расчет ДПКР'
                        },
                        {
                            xtype: 'button',
                            action: 'setpriorityall',
                            text: 'Очередность'
                        },
                        {
                            xtype: 'button',
                            action: 'makeversions',
                            text: 'Сохранить версии программы'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Субсидирование',
                    layout: 'hbox',
                    defaults: {
                        iconCls: 'icon-add',
                        margin: '0 10 10 0',
                        width: 200
                    },
                    items: [
                        {
                            xtype: 'button',
                            action: 'calcownercollection',
                            text: 'Рассчитать собираемость'
                        },
                        {
                            xtype: 'button',
                            action: 'calcvalues',
                            text: 'Рассчитать показатели'
                        },
                        {
                            xtype: 'button',
                            action: 'publishedprograms',
                            text: 'Версии для публикации'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});