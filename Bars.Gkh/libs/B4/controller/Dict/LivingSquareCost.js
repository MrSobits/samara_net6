Ext.define('B4.controller.dict.LivingSquareCost', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: ['dict.LivingSquareCost'],
    stores: ['dict.LivingSquareCost'],
    views: [
        'dict.livingsquarecost.EditWindow',
        'dict.livingsquarecost.Grid',
    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'livingsquarecostAspect',
            gridSelector: 'livingsquarecostGrid',
            editFormSelector: '#livingsquarecostEditWindow',
            storeName: 'dict.LivingSquareCost',
            modelName: 'dict.LivingSquareCost',
            editWindowView: 'dict.livingsquarecost.EditWindow'
        }
    ],

    mainView: 'dict.livingsquarecost.Grid',
    mainViewSelector: 'livingsquarecostGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'livingsquarecostGrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('livingsquarecostGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.LivingSquareCost').load();
    }
});