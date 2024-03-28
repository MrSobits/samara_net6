Ext.define('B4.view.manorglicense.TypeRequestSetWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.ux.button.Close',
        'B4.form.EnumCombo',
        'B4.form.ComboBox',
        'B4.enums.RequestSMEVType',
        'B4.ux.button.Save'     
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 300,
    bodyPadding: 10,
    itemId: 'manorglicenseTypeRequestSetWindow',
    title: 'Выбор типа запроса',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                                
                {
                    xtype: 'b4enumcombo',
                    editable: false,
                    name: 'RequestSMEVType',
                    itemId: 'cbRequestSMEVType',
                    fieldLabel: 'Тип запроса',
                    labelAlign: 'right',
                    enumName: 'B4.enums.RequestSMEVType',
                    allowBlank: false,
                    flex:1,
                    labelWidth: 80
                },
                {
                    xtype: 'button',
                    text: 'Применить',
                    tooltip: 'Создать запрос в СМЭВ',
                    iconCls: 'icon-accept',
                    width: 70,
                    itemId: 'btnSetTypeRequest'
                }
            ]
        });

        me.callParent(arguments);
    }
});