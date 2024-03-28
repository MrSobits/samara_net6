Ext.define('B4.controller.RequestStatePerson', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhInlineGrid',
        'B4.ux.button.Save',
        'B4.form.FileField'
    ],

	models: ['RequestStatePerson'],
	stores: ['RequestStatePerson'],
	views: ['requeststateperson.Grid'],

	mainView: 'requeststateperson.Grid',
	mainViewSelector: 'requestStatePersonGrid',

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
			selector: 'requestStatePersonGrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhinlinegridaspect',
			name: 'requestStatePersonGridGridAspect',
			storeName: 'RequestStatePerson',
			modelName: 'RequestStatePerson',
			gridSelector: 'requestStatePersonGrid'
        }
    ],

    index: function () {
		var view = this.getMainView() || Ext.widget('requestStatePersonGrid');
        this.bindContext(view);
        this.application.deployView(view);
		this.getStore('RequestStatePerson').load();
    },

    init: function () {
		var me = this;
        me.callParent(arguments);
    }

});