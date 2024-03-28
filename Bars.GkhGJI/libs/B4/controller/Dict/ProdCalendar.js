Ext.define('B4.controller.dict.ProdCalendar', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhInlineGrid',
      
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

	models: ['dict.ProdCalendar'],
	stores: ['dict.ProdCalendar'],

	views: ['dict.prodcalendar.Grid'],

	mainView: 'dict.prodcalendar.Grid',
	mainViewSelector: 'prodCalendarGrid',

    refs: [
        {
            ref: 'mainView',
			selector: 'prodCalendarGrid'
        }
    ],

    aspects: [
      
        {
            xtype: 'gkhinlinegridaspect',
			name: 'prodCalendarGridAspect',
			storeName: 'dict.ProdCalendar',
			modelName: 'dict.ProdCalendar',
			gridSelector: 'prodCalendarGrid'
        }
    ],

    index: function () {
		var view = this.getMainView() || Ext.widget('prodCalendarGrid');
        this.bindContext(view);
        this.application.deployView(view);
		this.getStore('dict.ProdCalendar').load();
    }
});