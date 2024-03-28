Ext.define('B4.controller.administration.PrintCertHistory', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid'
    ],

	models: ['administration.PrintCertHistory'],
	stores: ['administration.PrintCertHistory'],
	views: ['administration.printcerthistory.Grid'],

	mainView: 'administration.printcerthistory.Grid',
	mainViewSelector: 'printCertHistoryGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
			selector: 'printCertHistoryGrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
			name: 'printCertHistoryGridGridAspect',
			storeName: 'administration.PrintCertHistory',
			modelName: 'administration.PrintCertHistory',
			gridSelector: 'printCertHistoryGrid'
        }
    ],

    index: function () {
		var view = this.getMainView() || Ext.widget('printCertHistoryGrid');
        this.bindContext(view);
        this.application.deployView(view);
		this.getStore('administration.PrintCertHistory').load();
    },

    init: function () {
		var me = this;
        me.callParent(arguments);
    }

});