Ext.define('B4.view.ExtractEgrn.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.grid.Panel',
        'B4.ux.button.ChangeValue',
        'B4.form.SelectField',
        'B4.store.AccountsForComparsionNew',
        'B4.store.RealityObject',
        'B4.view.ExtractEgrn.IndGrid'
    ],
    alias: 'widget.extractegrneditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 800,
    minWidth: 700,
    minHeight: 500,
    height: 600,
    bodyPadding: 5,
    itemId: 'extractegrnEditWindow',
    title: 'ЕГРН - Помещения',
    trackResetOnLoad: true,
    closable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right',
                readOnly: false
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'RealityObjectId',
                    itemId: 'sfRealityObject',
                    allowBlank: true,
                    fieldLabel: 'Жилой дом',
                    store: 'B4.store.RealityObject',
                    textProperty: 'Address',
                    editable: false,
                    columns: [
                        {
                            text: 'Муниципальный район',
                            dataIndex: 'Municipality',
                            flex: 1,
                            filter: {
                                xtype: 'b4combobox',
                                operand: CondExpr.operands.eq,
                                storeAutoLoad: false,
                                hideLabel: true,
                                editable: false,
                                valueField: 'Name',
                                emptyItem: { Name: '-' },
                                url: '/Municipality/ListMoAreaWithoutPaging'
                            }
                        },
                        {
                            text: 'Адрес',
                            dataIndex: 'Address',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 100,
                        margin: '5 0 5 0',
                        labelAlign: 'right',
                        //    flex: 1
                    },
                    items: [
                       
                        {
                            xtype: 'b4selectfield',
                            name: 'Room_id',
                            fieldLabel: 'Лицевой счет',
                            store: 'B4.store.AccountsForComparsionNew',
                            editable: false,
                            flex: 1,
                            textProperty: 'Address',
                            itemId: 'sfPersAcc',
                            allowBlank: true,
                            columns: [
                                {
                                    text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                                    filter: {
                                        xtype: 'b4combobox',
                                        operand: CondExpr.operands.eq,
                                        storeAutoLoad: false,
                                        hideLabel: true,
                                        editable: false,
                                        valueField: 'Name',
                                        emptyItem: { Name: '-' },
                                        url: '/Municipality/ListWithoutPaging'
                                    }
                                },
                                { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Собственник', dataIndex: 'OwnerName', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Помещение', dataIndex: 'CnumRoom', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Площадь', dataIndex: 'AreaRoom', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Доля', dataIndex: 'AreaShare', flex: 1, filter: { xtype: 'textfield' } },
                            ]
                        }
                    ]
                },
                /*{
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 100,
                        margin: '5 0 5 0',
                        labelAlign: 'right',
                        //    flex: 1
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Room_id',
                            fieldLabel: 'Лицевой счет',
                            store: 'B4.store.AccountsForComparsion',
                            editable: false,
                            flex: 1,
                            textProperty: 'Address',
                            itemId: 'sfPersAcc',
                            allowBlank: true,
                            columns: [
                                 {
                                     text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
                                     filter: {
                                         xtype: 'b4combobox',
                                         operand: CondExpr.operands.eq,
                                         storeAutoLoad: false,
                                         hideLabel: true,
                                         editable: false,
                                         valueField: 'Name',
                                         emptyItem: { Name: '-' },
                                         url: '/Municipality/ListWithoutPaging'
                                     }
                                 },
                                { text: 'Адрес', dataIndex: 'Address', flex: 1, filter: { xtype: 'textfield' } },                               
                                { text: 'Собственник', dataIndex: 'OwnerName', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Помещение', dataIndex: 'CnumRoom', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Площадь', dataIndex: 'AreaRoom', flex: 1, filter: { xtype: 'textfield' } },
                                { text: 'Доля', dataIndex: 'AreaShare', flex: 1, filter: { xtype: 'textfield' } },
                            ]
                        }
                    ]
                },*/
                {
                    xtype: 'textfield',
                    name: 'Id',
                    fieldLabel: 'Id',
                    hidden: false,
                },
                {
                    xtype: 'textfield',
                    name: 'Address',
                    fieldLabel: 'Адрес',
                },
                {
                    xtype: 'textfield',
                    name: 'Area',
                    fieldLabel: 'Площадь',
                },
                {
                    xtype: 'textfield',
                    name: 'CadastralNumber',
                    fieldLabel: 'Кадастровый номер',
                },
                {
                    xtype: 'datefield',
                    name: 'ExtractDate',
                    fieldLabel: 'Дата выписки',
                    format: 'd.m.Y'
                },
                {
                    xtype: 'textfield',
                    name: 'Type',
                    fieldLabel: 'Тип'
                },
                {
                    xtype: 'textfield',
                    name: 'Purpose',
                    fieldLabel: 'Назначение'
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            xtype: 'extractegrnindgrid',
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
                            xtype: 'b4savebutton'
                        },
                        {
                            xtype: 'button',
                            iconCls: 'icon-book-go',
                            name: 'btnGetExtract',
                            itemId: 'btnGetExtract',
                            action: 'getExtract',
                            text: 'Скачать выписку'
                        }
                       
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});