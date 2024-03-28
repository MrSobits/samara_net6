Ext.define('B4.controller.LogEdo', {
    extend: 'B4.base.Controller',
    params: null,
    requires: [
        'B4.view.edolog.FilterPanel',
        'B4.view.edolog.Grid',
        'B4.view.edolog.Panel'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: ['edolog.AppealCits'],
    stores: ['edolog.AppealCits'],

    views: [
        'edolog.Grid',
        'edolog.Panel',
        'edolog.FilterPanel'
            
    ],

    mainView: 'edolog.Panel',
    mainViewSelector: 'appealCitsEdoLogPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'appealCitsEdoLogPanel'
        }
    ],

    aspects: [
    ],

    onUpdateGrid: function () {
        var store = this.getStore('edolog.AppealCits');
        store.currentPage = 1;
        store.load();
    },
    onChangeDateCreateStart: function (field, newValue) {
        this.params.dateCreateStart = newValue;
    },
    onChangeDateCreateEnd: function (field, newValue) {
        this.params.dateCreateEnd = newValue;
    },
    onChangeDateActualStart: function (field, newValue) {
        this.params.dateActualStart = newValue;
    },
    onChangeDateActualEnd: function (field, newValue) {
        this.params.dateActualEnd = newValue;
    },
    init: function() {
        var actions = {};
        actions[this.mainViewSelector + ' #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
        
        actions['#appealcitsEdoLogFilterPanel #dfDateCreateStart'] = { 'change': { fn: this.onChangeDateCreateStart, scope: this } };
        actions['#appealcitsEdoLogFilterPanel #dfDateCreateEnd'] = { 'change': { fn: this.onChangeDateCreateEnd, scope: this } };
        
        actions['#appealcitsEdoLogFilterPanel #dfDateActualStart'] = { 'change': { fn: this.onChangeDateActualStart, scope: this } };
        actions['#appealcitsEdoLogFilterPanel #dfDateActualEnd'] = { 'change': { fn: this.onChangeDateActualEnd, scope: this } };

        this.control(actions);
        this.getStore('edolog.AppealCits').on('beforeload', this.onBeforeLoad, this);
    },
    index: function () {
        var view = this.getMainView() || Ext.widget('appealCitsEdoLogPanel');
        this.bindContext(view);
        this.application.deployView(view);

        this.params = {};
        this.params.dateCreateStart = new Date(new Date().getFullYear(), 0, 1);
        this.params.dateCreateEnd = new Date;
        
        this.params.dateActualStart = new Date(new Date().getFullYear(), 0, 1);
        this.params.dateActualEnd = new Date;
        this.getStore('edolog.AppealCits').load();
    },
    onBeforeLoad: function (store, operation) {
        if (this.params) {
            operation.params.dateCreateStart = this.params.dateCreateStart;
            operation.params.dateCreateEnd = this.params.dateCreateEnd;
            operation.params.dateActualStart = this.params.dateActualStart;
            operation.params.dateActualEnd = this.params.dateActualEnd;
        }
    }
});