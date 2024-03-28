Ext.define('B4.view.personalAccount.InfoPanel', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.personalAccount_info_panel',
    closable: true,
    title: 'Лицевой счет',
    layout: {
        type: 'border'
    },

    requires: [
        'B4.store.NavigationMenu',
        'B4.view.MenuTreePanel',
        'B4.ux.breadcrumbs.Breadcrumbs',
        'B4.form.ComboBox'
    ],

    initComponent: function () {
        var me = this,
            currentDate = new Date(),
            store = Ext.create('B4.store.NavigationMenu', {
                menuName: 'PersonalAccountInfo'
            }),
            monthStore = Ext.create('Ext.data.Store', {
                fields: ['Month', 'Name'],
                data: []
            }),
            yearStore = Ext.create('Ext.data.Store', {
                fields: ['Year', 'Name'],
                data: []
            });

        for (var i = 0; i < 12; i++) {
            monthStore.add({
                Month: i + 1,
                Name: Ext.Date.monthNames[i]
            });
            yearStore.add({
                Year: currentDate.getFullYear() - i,
                Name: String(currentDate.getFullYear() - i)
            });
        }

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    region: 'north',
                    layout: {
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'breadcrumbs',
                            itemId: 'personalAccountInfoLabel',
                            flex: 1
                        },
                        //{
                        //    xtype: 'textfield',
                        //    fieldLabel: 'Секция',
                        //    labelAlign: 'right',
                        //    margin: '4 0 0 5',
                        //    value: 'По умолчанию'
                        //},
                        {
                            xtype: 'combobox',
                            fieldLabel: 'Период',
                            labelAlign: 'right',
                            labelWidth: 70,
                            width: 170,
                            margin: '4 0 0 5',
                            name: 'month',
                            displayField: 'Name',
                            valueField: 'Month',
                            store: monthStore,
                            queryMode: 'local',
                            editable: false,
                            value: currentDate.getMonth()
                        },
                        {
                            xtype: 'combobox',
                            width: 70,
                            margin: '4 0 0 5',
                            name: 'year',
                            displayField: 'Name',
                            valueField: 'Year',
                            store: yearStore,
                            queryMode: 'local',
                            editable: false,
                            value: currentDate.getFullYear()
                        }
                    ]
                },
                {
                    itemId: 'personalAccountMenuTree',
                    xtype: 'menutreepanel',
                    title: 'Разделы',
                    store: store,
                    margins: '0 2 0 0',
                    width: 300
                },
                {
                    xtype: 'container',
                    region: 'center',
                    layout: 'anchor',
                    items: [
                        {
                            xtype: 'container',
                            style: 'border: 1px solid #a6c7f1 !important; font: 12px tahoma,arial,helvetica,sans-serif; background: transparent; margin: 10px 30px 0 5px; padding: 5px 10px; line-height: 16px;',
                            html: '<span style="display: table-cell"><span class="im-info" style="vertical-align: top;"></span></span><span style="display: table-cell; padding-left: 5px;">Данная информация указана в соотвествии с выбранным периодом. Для смены периода необходимо выбрать значение месяца и года в правом верхнем углу окна.</span>'
                        },
                        {
                            xtype: 'tabpanel',
                            anchor: '100% -55',
                            margin: '5 0 0 0',
                            itemId: 'personalAccountMainTab',
                            enableTabScroll: true
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});