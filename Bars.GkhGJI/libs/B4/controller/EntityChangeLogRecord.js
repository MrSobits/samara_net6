Ext.define('B4.controller.EntityChangeLogRecord', {
    extend: 'B4.base.Controller',

	requires: [
		'B4.aspects.GkhInlineGrid'
      
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

	models: ['entitylog.EntityChangeLogRecord'],
	stores: ['entitylog.EntityChangeLogRecord'],

	views: ['entitylog.Grid'],

	mainView: 'entitylog.Grid',
	mainViewSelector: 'entitychangelogrecordgrid',

    refs: [
        {
            ref: 'mainView',
			selector: 'entitychangelogrecordgrid'
        }
    ],

	aspects: [
		{
			xtype: 'gkhinlinegridaspect',
			gridSelector: 'entitychangelogrecordgrid',
			name: 'entitychangelogrecordGridAspect',
			storeName: 'entitylog.EntityChangeLogRecord',
			modelName: 'entitylog.EntityChangeLogRecord',
            otherActions: function (actions) {              
                actions['entitychangelogrecordgrid #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };            
                actions['entitychangelogrecordgrid #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
            },
            onChangeDateStart: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateStart = newValue;
                }
            },
            onChangeDateEnd: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateEnd = newValue;
                }
            }
		}
      
    ],

    init: function () {
        var me = this,
            actions = {};
        me.params = {};
        this.getStore('entitylog.EntityChangeLogRecord').on('beforeload', this.onBeforeLoadDoc, this);
        this.callParent(arguments);
    },
    onBeforeLoadDoc: function (store, operation) {
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
        }
    },

	index: function () {
        var view = this.getMainView() || Ext.widget('entitychangelogrecordgrid');
        this.params.dateStart = view.down('#dfDateStart').getValue();
        this.params.dateEnd = view.down('#dfDateEnd').getValue();
        this.bindContext(view);
        this.application.deployView(view);
		this.getStore('entitylog.EntityChangeLogRecord').load();
    }
});