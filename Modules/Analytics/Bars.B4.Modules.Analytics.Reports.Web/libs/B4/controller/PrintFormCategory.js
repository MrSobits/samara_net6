Ext.define('B4.controller.PrintFormCategory', {
    extend: 'B4.base.Controller',
    views: [ 'PrintFormCategory.Grid' ],

    requires: [
        'B4.aspects.InlineGrid'
    ],

    models: ['PrintFormCategory'],
    stores: ['PrintFormCategory'],

    mainView: 'PrintFormCategory.Grid',
    mainViewSelector: '#printFormCategoryGrid',

    aspects: [
        {
            xtype: 'inlinegridaspect',
            name: 'PrintFormCategoryGridAspect',
            storeName: 'PrintFormCategory',
            modelName: 'PrintFormCategory',
            gridSelector: '#printFormCategoryGrid'
        }
    ],

    init: function () {
        this.callParent(arguments);
    }
});