Ext.define('B4.controller.CostLimitOOI', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow',
    ],
    stores: [
        'CostLimitOOI',
    ],
    models: [
        'CostLimitOOI',
    ],
    views: [
        'costlimitooi.Grid',
        'costlimitooi.Panel',
        'costlimitooi.EditWindow',
    ],
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    refs: [
        {
            ref: 'mainView',
            selector: 'costlimitpanelooi'
        }
    ],
    mainView: 'costlimitooi.Panel',
    mainViewSelector: 'costlimitpanelooi',
    //codeParam: null,
    init: function () {
        var me = this,
            actions = {
            };
        me.control(actions);
        me.callParent(arguments);
    },
    index: function () {
        var view = this.getMainView() || Ext.widget('costlimitpanelooi');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('CostLimitOOI').load();
    },
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'costlimitGridAspect',
            gridSelector: 'costlimitgridooi',
            editFormSelector: '#costlimitEditWindowOOI',
            storeName: 'CostLimitOOI',
            modelName: 'CostLimitOOI',
            editWindowView: 'costlimitooi.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно не закрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
        }]
});