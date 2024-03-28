Ext.define('B4.controller.emergencyobj.Edit', {
    /* 
    * Контроллер формы редактирования аварийного дома
    */
    extend: 'B4.base.Controller',
    views: [ 'emergencyobj.EditPanel' ], 

    params: null,
    requires: [
        'B4.aspects.GkhEditPanel',
        'B4.aspects.StateButton',
        'B4.aspects.permission.emergencyobj.State'
    ],

    models: ['EmergencyObject'],

    mainView: 'emergencyobj.EditPanel',
    mainViewSelector: '#emergencyObjEditPanel',

    aspects: [
        {
            xtype: 'emergencyobjstateperm',
            editFormAspectName: 'emergencyObjEditPanelAspect'
        },
        {
            /*
            Вешаем аспект смены статуса в карточке редактирования
            */
            xtype: 'statebuttonaspect',
            name: 'emergencyObjStateButtonAspect',
            stateButtonSelector: '#emergencyObjEditPanel #btnState',
            listeners: {
                transfersuccess: function (asp, entityId) {
                    //После успешной смены статуса запрашиваем по Id актуальные данные записи
                    //и обновляем панель
                    asp.controller.getAspect('emergencyObjEditPanelAspect').setData(entityId);
                }
            }
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования раздела аварийного дома
            */
            xtype: 'gkheditpanel',
            name: 'emergencyObjEditPanelAspect',
            editPanelSelector: '#emergencyObjEditPanel',
            modelName: 'EmergencyObject',
            listeners: {
                aftersetpaneldata: function (asp, rec) {
                    this.controller.getAspect('emergencyObjStateButtonAspect').setStateData(rec.get('Id'), rec.get('State'));
                },
                savesuccess: function (asp, rec) {
                    var factDate = rec.get('FactDemolitionDate'),
                        conditionHouse = rec.get('ConditionHouse'),
                        isRazed = (conditionHouse && conditionHouse === 40);

                    //если заполнена фактическая дата и состояние дома<>Снесен
                    if (factDate && !isRazed) {
                        B4.QuickMsg.msg('Предупреждение', 'Указана фактическая дата сноса. Требуется изменить состояние дома на "Снесен"', 'warning', 4000);
                    }

                    //если состояние дома=Снесен и фактическая дата не заполнена 
                    if (isRazed && !factDate) {
                        B4.QuickMsg.msg('Предупреждение', 'Установлено состояние дома = Снесен. Требуется указать фактическую дату сноса', 'warning', 4000);
                    }
                   
                }
            }
        }
    ],

    onLaunch: function () {
        var me = this; 
        if (me.params) {
            me.getAspect('emergencyObjEditPanelAspect').setData(me.params.get('Id'));
        }
    }
});