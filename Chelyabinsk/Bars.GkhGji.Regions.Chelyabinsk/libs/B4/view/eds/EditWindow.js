Ext.define('B4.view.eds.EditWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },
    width: 1100,
    minWidth: 800,
    height: 500,
    resizable: true,
    bodyPadding: 3,
  //  layout: 'form',
    itemId: 'edsEditWindow',
    title: 'Проверка ГЖИ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.eds.NoticeGrid',
        'B4.view.eds.MotivRequstGrid',
        'B4.enums.TypeBase',
        'B4.view.eds.DocumentGrid',
        'B4.view.eds.UKDocumentGrid',
        //'B4.view.eds.PetitionGrid',
        'B4.form.EnumCombo',
        'B4.form.FileField',
        'B4.store.Contragent'

    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 130
            },
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'ContragentName',
                            editable: false,
                            fieldLabel: 'Организация',
                            flex: 2,
                            maxLength: 300,
                            labelWidth: 130
                        }
                    ]
                },               
                {
                    xtype: 'container',
                    layout: 'hbox',
                    padding: '0 0 5 0',
                    defaults: {
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'InspectionNumber',
                            editable: false,
                            fieldLabel: 'Номер проверки',
                            maxLength: 300,
                            labelWidth: 130
                        },
                        {
                            xtype: 'datefield',
                            anchor: '100%',
                            width: 250,
                            editable: false,
                            name: 'InspectionDate',
                            fieldLabel: 'Дата проверки',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'b4enumcombo',
                            anchor: '100%',
                            fieldLabel: 'Основание',
                            enumName: 'B4.enums.TypeBase',
                            name: 'TypeBase'
                        }
                    ]
                },    
                {
                    xtype: 'tabpanel',
                    itemId: 'edsInspectionTabPanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            xtype: 'esddocumentgrid',
                            flex: 1
                        },
                        {
                            xtype: 'esdnoticegrid',
                            flex: 1
                        },
                        {
                            xtype: 'esdmotivrequstgrid',
                            flex: 1
                        },
                        {
                            xtype: 'ukdocumentgrid',
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