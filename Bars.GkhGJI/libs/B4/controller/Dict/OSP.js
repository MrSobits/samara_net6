Ext.define('B4.controller.dict.OSP', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GridEditWindow'
    ],

    models: ['dict.OSP'],
    stores: ['dict.OSP'],
    views: [
        'dict.OSP.EditWindow',
        'dict.OSP.Grid',
    ],

    aspects: [

        {
            xtype: 'grideditwindowaspect',
            name: 'oSPGridAspect',
            gridSelector: 'oSPGrid',
            editFormSelector: '#oSPEditWindow',
            storeName: 'dict.OSP',
            modelName: 'dict.OSP',
            editWindowView: 'dict.OSP.EditWindow'
        }
    ],

    mainView: 'dict.OSP.Grid',
    mainViewSelector: 'oSPGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'oSPGrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('oSPGrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('dict.OSP').load();
    }
});