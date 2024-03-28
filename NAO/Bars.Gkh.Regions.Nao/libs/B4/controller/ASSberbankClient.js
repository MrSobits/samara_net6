Ext.define('B4.controller.ASSberbankClient', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.model.ASSberbankClient',
        'B4.store.ASSberbankClient',
    ],

    stores: [
        'ASSberbankClient',
    ],

    models: [
        'ASSberbankClient',
    ],

    views: [
        'assberbank.Grid',
        //'maxsumbyyear.Panel',
        'assberbank.EditWindow',
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        { ref: 'mainView', selector: 'assberbankgrid' },
        {
            ref: 'editWindow',
            selector: '#assberbankEditWindow'
        }
    ],

    codeParam: null,

    init: function () {
        var me = this,
            actions =
            {
            };
        me.control(actions);
        me.callParent(arguments);
    },

    index: function () {
        var view = Ext.widget('assberbankgrid');

        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('ASSberbankClient').load();
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'ASSberbankClientGridAspect',
            gridSelector: 'assberbankgrid',
            editFormSelector: '#assberbankEditWindow',
            storeName: 'ASSberbankClient',
            modelName: 'ASSberbankClient',
            editWindowView: 'assberbank.EditWindow',
            //onSaveSuccess: function () {
            //    // перекрываем чтобы окно не закрывалось после сохранения
            //    B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            //},
        }]
});