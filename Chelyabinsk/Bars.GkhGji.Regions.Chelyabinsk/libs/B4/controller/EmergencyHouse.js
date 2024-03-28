Ext.define('B4.controller.EmergencyHouse', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
         'B4.aspects.GkhGridEditForm',
         'B4.aspects.ButtonDataExport',
         'B4.Ajax', 'B4.Url'
    ],

    appealCitsEmergencyHouse: null,

    mixins: {
        mask: 'B4.mixins.MaskBody'
        //ToDo Пока невозможно перевести реестр обращения на роуты
        /* Закоментировал в связи с невозможностью перевода на роутинг
        ,
        context: 'B4.mixins.Context'*/
    },

    stores: [
        'appealcits.EmergencyHouse'
    ],

    models: [
        'appealcits.EmergencyHouse'
    ],

    views: [
        'appealcits.EmergencyHouseGrid',
        'appealcits.EmergencyHouseEditWindow',
        'appealcits.EmergencyHouseMainPanel',
        'appealcits.EmergencyHouseFilterPanel'
    ],

    mainView: 'appealcits.EmergencyHouseMainPanel',
    mainViewSelector: 'appealcitsEmergencyHouseMainPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'appealcitsEmergencyHouseMainPanel'
        },
        {
            ref: 'emergencyHouseEditWindow',
            selector: 'emergencyhouseeditwindow'
        }
    ],

    aspects: [
         {
             xtype: 'grideditwindowaspect',
             name: 'emergencyHouseGridWindowAspect',
             gridSelector: 'emergencyhousegrid',
             editFormSelector: 'emergencyhouseeditwindow',
             modelName: 'appealcits.EmergencyHouse',
             storeName: 'appealcits.EmergencyHouse',
             editWindowView: 'appealcits.EmergencyHouseEditWindow',
             otherActions: function (actions) {
                 actions['#appealcitsEmergencyHouseFilterPanel #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                 actions['#appealcitsEmergencyHouseFilterPanel #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                 actions['#appealcitsEmergencyHouseFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
             },
             onUpdateGrid: function () {
                 var str = this.controller.getStore('appealcits.EmergencyHouse');
                 str.currentPage = 1;
                 str.load();
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
             },
             onSaveSuccess: function () {
                 // перекрываем чтобы окно незакрывалось после сохранения
                 B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
             },
             listeners: {
                 aftersetformdata: function (asp, rec, form) {
                     var me = this;
                     appealCitsEmergencyHouse = rec.getId();
                 }
             }
         }
    ],

    index: function (operation) {
        var me = this,
            view = me.getMainView() || Ext.widget('appealcitsEmergencyHouseMainPanel');
        me.params = {};
        me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
        me.params.dateEnd = new Date(new Date().getFullYear(), 11, 31);
        me.bindContext(view);
        this.application.deployView(view);
        
        this.getStore('appealcits.EmergencyHouse').load();
    },

    init: function () {
        var me = this,
            actions = {};
        me.params = {};
        me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
        me.params.dateEnd = new Date(new Date().getFullYear(), 11, 31);
        this.getStore('appealcits.EmergencyHouse').on('beforeload', this.onBeforeLoadEmergencyHouse, this);
        this.getStore('appealcits.EmergencyHouse').load();
        me.callParent(arguments);
    },

    onBeforeLoadEmergencyHouse: function (store, operation) {
        operation.params.isFiltered = true;
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
        }
    },
});