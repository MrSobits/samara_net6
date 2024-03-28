Ext.define('B4.view.longtermprobject.propertyownerdecision.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.longtermdecisioneditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 700,
    minWidth: 700,
    minHeight: 170,
    maxHeight: 170,
    bodyPadding: 5,
    title: 'Редактирование повестки ОСС',
    closable: false,
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],
    itemId: 'propertyownerdecisionEditWindow',
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    name: 'Decision',
                    fieldLabel: 'Повестка',
                    store: 'B4.store.dict.OwnerProtocolTypeDecisionForSelect',
                    allowBlank: false,
                    editable: false,
                    idProperty: 'Id',
                    itemId: 'sfDecision',
                    textProperty: 'Name',
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter:
                            {
                                xtype: 'textfield'
                            }
                        }],
                    valueField: 'Value',
                    flex: 1
                }
                //{
                //    xtype: 'b4filefield',
                //    name: 'DocumentFile',
                //    fieldLabel: 'Файл',
                //    itemId: 'ffDocumentFile'
                //},
                //{
                //    xtype: 'textfield',
                //    fieldLabel: 'Описание',
                //    name: 'Description'
                //}
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