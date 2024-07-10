Ext.define('B4.controller.MVDPassport', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.Ajax',
        'B4.Url'
    ],

    smevNDFL: null,
  
    models: [
        'smev.MVDPassport',
        'smev.MVDPassportFile'
    ],
    stores: [
        'smev.MVDPassport',
        'smev.MVDPassportFile',
        'smev.MVDPassportAnswer'
    ],
    views: [
        'mvdpassport.Grid',
        'mvdpassport.EditWindow',
        'mvdpassport.FileInfoGrid',
        'mvdpassport.AnswerGrid'
    ],

    mainView: 'mvdpassport.Grid',
    mainViewSelector: 'smevcertinfogrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'mvdpassportgrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'mvdpassportGridAspect',
            gridSelector: 'mvdpassportgrid',
            editFormSelector: '#mvdpassportEditWindow',
            storeName: 'smev.MVDPassport',
            modelName: 'smev.MVDPassport',
            editWindowView: 'mvdpassport.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },
            otherActions: function (actions) {
                actions['#mvdpassportEditWindow #cbMVDPassportRequestType'] = { 'change': { fn: this.onChangeRequestType, scope: this } };
            },            
            listeners: {
                aftersetformdata: function (asp, record, form) {
                 
                    var grid = form.down('mvdpassportfileinfogrid'),
                    store = grid.getStore();
                    store.filter('smevMVD', record.getId());

                    //var answergrid = form.down('mvdpassportanswergrid'),
                    //answerstore = answergrid.getStore();
                    //answerstore.filter('smevMVD', record.getId());
                }
            },
            onChangeRequestType: function (field, newValue) {
                var form = this.getForm(),
                    byFio = form.down('#byFio'),
                    byPassport = form.down('#byPassport');

                if (newValue == B4.enums.MVDPassportRequestType.PersonInfo) {
                    byFio.show();
                    byPassport.hide();
                }
                else {
                    byFio.hide();
                    byPassport.show();
                }
            },
        }   
    ],   

    init: function () {
        this.control({
            'mvdpassportgrid actioncolumn[action="openpassport"]': { click: { fn: this.sendRequest, scope: this } },
        });

        this.callParent(arguments);
    },

    sendRequest: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        
        smevCertInfo = rec.getId();
        B4.Ajax.request({
            url: B4.Url.action('Execute', 'MVDPassportExecute'),
            params: {
                taskId: smevCertInfo
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev.MVDPassport').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev.MVDPassport').load();
        });
    },

    /////////////////////
    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('mvdpassportGridAspect').editRecord(this.params);
            this.params = null;
        }
    },
    ////////////////////

    index: function () {
        var view = this.getMainView() || Ext.widget('mvdpassportgrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.MVDPassport').load();
    }
});