Ext.define('B4.view.dict.municipalitytree.EditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'fit',
    width: 650,
    //height: 415,
    minHeight: 415,
    maximizable: true,
    resizable: true,
    itemId: 'municipalityEditWindow',
    title: 'Муниципальное образование',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.view.dict.municipalitytree.SourceFinancingGrid',
        'B4.view.dict.municipalitytree.MunicipalityFiasOktmoGrid',
        'B4.form.SelectField',
        'B4.store.dict.municipality.MoArea',
        'B4.enums.TypeMunicipality'
    ],

    initComponent: function () {
        var me = this;

       Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    layout: 'vbox',
                    border: false,
                    margins: -1,
                    height: 500,
                    items: [
                        {
                            xtype: 'panel',
                            flex: 1,
                            margins: -1,
                            border: false,
                            frame: true,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            title: 'Общие сведения',
                            padding: '10 10 0 10',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'Name',
                                    fieldLabel: 'Наименование',
                                    labelAlign: 'right',
                                    labelWidth: 110,
                                    maxLength: 300,
                                    allowBlank: false
                                },
                                {
                                    xtype: 'fieldset',
                                    flex: 1,
                                    title: 'Реквизиты',
                                    layout: { type: 'vbox', align: 'stretch' },
                                    defaults: {
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Group',
                                            fieldLabel: 'Группа',
                                            maxLength: 30
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Cut',
                                            fieldLabel: 'Сокращение',
                                            maxLength: 10
                                        },
                                        //{
                                        //    xtype: 'b4selectfield',
                                        //    name: 'MoParentId',
                                        //    itemId: 'sfMunicipality',
                                        //    fieldLabel: 'Муниципальный район',
                                        //    store: 'B4.store.dict.municipality.MoArea',
                                        //    editable: false,
                                        //    allowBlank: true,
                                        //    columns: [
                                        //        { xtype: 'gridcolumn', header: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
                                        //    ]
                                        //},
                                        {
                                            xtype: 'b4combobox',
                                            name: 'Level',
                                            items: B4.enums.TypeMunicipality.getItems(),
                                            displayField: 'Display',
                                            valueField: 'Value',
                                            fieldLabel: 'Тип муниципального образования',
                                            editable: false,
                                            disabled : true
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Code',
                                            fieldLabel: 'Код',
                                            maxLength: 30
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'FederalNumber',
                                            fieldLabel: 'Федеральный номер',
                                            maxLength: 30
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Okato',
                                            fieldLabel: 'ОКАТО',
                                            maxLength: 30
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Oktmo',
                                            fieldLabel: 'OKTMO',
                                            maxLength: 30
                                        },
                                        {
                                            xtype: 'textareafield',
                                            padding: '10 0 0 0',
                                            height: 50,
                                            name: 'Description',
                                            fieldLabel: 'Описание/ комментарий',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'checkbox',
                                            name: 'CheckCertificateValidity',
                                            boxLabel: 'Проверять корректность сертификата ЭЦП',
                                            margin: '5 0 5 105'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'sourcefingrid',
                            margins: -1,
                            flex: 1
                        },
                        {
                            xtype: 'municipalityfiasoktmogrid',
                            margins: -1,
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