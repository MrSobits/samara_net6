/*
* Переопределили аспект так как он используется еще и в техпаспорте с добавлением нового функционала через переопределение подписок.
*/

Ext.define('B4.aspects.realityobj.RealityObjEditPanelAspect', {
    extend: 'B4.aspects.GkhEditPanel',

    alias: 'widget.realityobjeditpanelaspect',
        
    name: 'realityobjEditPanelAspect',
    editPanelSelector: 'realityobjEditPanel',
    modelName: 'RealityObject',

    otherActions: function (actions) {
        var me = this;
        actions[me.editPanelSelector + ' #btnMap'] = { 'click': { fn: me.onClickbtnMap, scope: me } };
        actions[me.editPanelSelector + ' b4updatebutton'] = { 'click': { fn: me.onUpdateBtnClick, scope: me } };
    },
    
    onUpdateBtnClick: function () {
        var me = this,
            rec = me.getRecord();

        me.setData(rec.getId());
    },
    
    listeners: {
        savesuccess: function (asp, rec) {
            asp.setData(rec.getId());
        },
        aftersetpaneldata: function (asp, rec, panel) {
            var dfDateDemolutionRealityObject = panel.down('#dfDateDemolutionRealityObject');

            if (rec.get('ConditionHouse') == 40) {
                dfDateDemolutionRealityObject.setDisabled(false);
            }
            else {
                dfDateDemolutionRealityObject.setDisabled(true);
            }

            var stateButtonAspect = this.controller.getAspect('realityobjStateButtonAspect');

            if (stateButtonAspect) {
                stateButtonAspect.setStateData(rec.get('Id'), rec.get('State'));
            }
        },
        rendermap: function (asp) {
            asp.controller.loadMap(asp);
        }
    },

    onClickbtnMap: function () {
        var me = this;

        me.controller.application.redirectTo(Ext.String.format('map/{0}', me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId')));
    }

});