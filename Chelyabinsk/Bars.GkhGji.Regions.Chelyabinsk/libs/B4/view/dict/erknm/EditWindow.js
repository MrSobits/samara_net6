Ext.define('B4.view.dict.erknm.EditWindow', {
    extend: 'B4.form.Window',
    itemId: 'directoryERKNMEditWindow',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.YesNoNotSet',
        'B4.view.dict.erknm.RecordGrid'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    bodyPadding: 10,
    title: 'Форма редактирования ЕРКНМ',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [
                {
                    xtype: 'textfield',
                    name: 'Name',
                    flex: 1,
                    fieldLabel: 'Наименование'
                },
                {
                    xtype: 'textfield',
                    name: 'Code',
                    flex: 1,
                    fieldLabel: 'Код'
                },
                {
                    xtype: 'textfield',
                    itemId: 'tfCodeERKNM',
                    name: 'CodeERKNM',
                    flex: 1,
                    fieldLabel: 'Код ЕРКНМ'
                },
                {
                    xtype: 'textfield',
                    name: 'EntityName',
                    flex: 1,
                    fieldLabel: 'Наименование объекта в системе'
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
                            xtype: 'recordDirectoryERKNMGrid',
                            flex: 1
                        },
                        {
                            xtype: 'erknmDictFileGrid',
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
                            columns: 1,
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