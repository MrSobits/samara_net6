Ext.define('B4.controller.AppealCitsInfo', {
    extend: 'B4.base.Controller',

	requires: [
		'B4.aspects.GkhInlineGrid'
      
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

	models: ['appealcits.AppealCitsInfo'],
	stores: ['appealcits.AppealCitsInfo'],

	views: ['appealcits.AppealCitsInfoGrid'],

	mainView: 'appealcits.AppealCitsInfoGrid',
	mainViewSelector: 'appealcitsinfogrid',

    refs: [
        {
            ref: 'mainView',
			selector: 'appealcitsinfogrid'
        }
    ],

	aspects: [
		{
			xtype: 'gkhinlinegridaspect',
			gridSelector: 'appealcitsinfogrid',
			name: 'appealcitsinfoGridAspect',
			storeName: 'appealcits.AppealCitsInfo',
			modelName: 'appealcits.AppealCitsInfo'
		}
      
    ],

	index: function () {
		var view = this.getMainView() || Ext.widget('appealcitsinfogrid');
        this.bindContext(view);
        this.application.deployView(view);
		this.getStore('appealcits.AppealCitsInfo').load();
    }
});