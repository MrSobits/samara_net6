Ext.define('B4.controller.LogEdoRequests', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.aspects.GridEditWindow',
        'B4.view.edologrequests.FilterPanel',
        'B4.view.edologrequests.Grid',
        'B4.view.edologrequests.Panel'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['edolog.AppealCits'],
    stores: ['edolog.AppealCits'],

    views: [
        'edologrequests.Grid',
        'edologrequests.Panel',
        'edologrequests.EditWindow',
        'edologrequests.AppealCitsGrid'
    ],

    mainView: 'edologrequests.Panel',
    mainViewSelector: 'appealCitsEdoLogRequestsPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'appealCitsEdoLogRequestsPanel'
        }
    ],

    aspects: [
         {
             xtype: 'grideditwindowaspect',
             name: 'requestsGridWindowAspect',
             gridSelector: '#edoLogRequestsGrid',
             editFormSelector: '#edoLogRequestsEditWindow',
             storeName: 'edolog.Requests',
             modelName: 'edolog.Requests',
             editWindowView: 'edologrequests.EditWindow',
             listeners: {
                 aftersetformdata: function (asp, record) {
                     asp.controller.setCurrentId(record.getId());
                 }
             }
         }
    ],
    setCurrentId: function (id) {
        this.logRequestsId = id;

        var sourceStore = this.getStore('edolog.RequestsAppealCits');
        sourceStore.load();
    },
    onRefreshGrid: function () {
        this.getStore('edolog.Requests').load();
    },
    onChangeDateStart: function (field, newValue) {
        this.params.dateStart = newValue;
    },
    onChangeDateEnd: function (field, newValue) {
        this.params.dateEnd = newValue;
    },
    init: function () {
        var actions = {};
        actions[this.mainViewSelector + ' #updateGrid'] = { 'click': { fn: this.onRefreshGrid, scope: this } };

        actions['#requestsFilterPanel #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
        actions['#requestsFilterPanel #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
        
        this.control(actions);
        this.getStore('edolog.Requests').on('beforeload', this.onBeforeLoad, this);
        this.getStore('edolog.RequestsAppealCits').on('beforeload', this.onBeforeLoadAppealCits, this);
        
        this.callParent(arguments);
    },
    index: function () {
        var view = this.getMainView() || Ext.widget('appealCitsEdoLogRequestsPanel');
        this.bindContext(view);
        this.application.deployView(view);

        this.params = {};
        this.params.dateCreateStart = new Date(new Date().getFullYear(), 0, 1);
        this.params.dateCreateEnd = new Date;

        this.params.dateActualStart = new Date(new Date().getFullYear(), 0, 1);
        this.params.dateActualEnd = new Date;
        this.getStore('edolog.Requests').load();
    },
    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
        }
    },
    onBeforeLoadAppealCits: function (store, operation) {
        if (this.params) {
            operation.params.logRequestsId = this.logRequestsId;
        }
    }
});