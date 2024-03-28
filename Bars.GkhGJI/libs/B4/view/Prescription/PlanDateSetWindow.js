Ext.define('B4.view.prescription.PlanDateSetWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'     
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 500,
    bodyPadding: 10,
    itemId: 'prescriptionPlanDateSetWindow',
    title: 'Установка даты',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                                
                {
                    xtype: 'datefield',
                    format: 'd.m.Y H:i',
                    allowBlank: false,
                    name: 'SetPlanRemoval',
                    fieldLabel: 'Устранить до',
                    itemId: 'dfSetPlanRemoval',
                    flex: 1,
                }, 
                {
                    xtype: 'button',
                    text: 'Применить',
                    tooltip: 'Установить выбранную дату всем нарушениям',
                    iconCls: 'icon-accept',
                    width: 70,
                    itemId: 'btnSetPlanRemovat'
                }
            ]
        });

        me.callParent(arguments);
    }
});