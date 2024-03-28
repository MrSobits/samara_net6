Ext.define('B4.controller.chargessplitting.contrpersumm.ContractPeriod', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.view.chargessplitting.contrpersumm.contractperiod.EditWindow',
        'B4.view.chargessplitting.contrpersumm.contractperiod.Grid',

        'B4.aspects.GridEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'chargessplitting.contrpersumm.ContractPeriod'
    ],

    models: [
        'chargessplitting.contrpersumm.ContractPeriod'
    ],

    views: [
        'chargessplitting.contrpersumm.contractperiod.EditWindow',
        'chargessplitting.contrpersumm.contractperiod.Grid'
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'contractperiodgrideditaspect',
            gridSelector: 'contractperiodgrid',
            editFormSelector: 'contractperiodeditwindow',
            storeName: 'chargessplitting.contrpersumm.ContractPeriod',
            modelName: 'chargessplitting.contrpersumm.ContractPeriod',
            editWindowView: 'chargessplitting.contrpersumm.contractperiod.EditWindow',
            rowAction: function (grid, action, record) {
                if (!grid || grid.isDestroyed) return;
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                        case 'delete':
                            this.deleteRecord(record);
                            break;
                    }
                }
            },
            saveRequestHandler: function () {
                var me = this,
                    form = this.getForm(),
                    rec;

                if (this.fireEvent('beforesaverequest', this) !== false) {
                    form.getForm().updateRecord();
                    rec = this.getRecordBeforeSave(form.getRecord());

                    this.fireEvent('getdata', this, rec);

                    if (form.getForm().isValid()) {
                        if (this.fireEvent('validate', this)) {
                             
                            me.controller.mask('Сохранение', me.controller.getMainView());

                            B4.Ajax.request({
                                url: B4.Url.action('CreatePeriod', 'ContractPeriod'),
                                params: {
                                    startDate: rec.get('StartDate')
                                },
                                timeout: 5 * 60 * 1000 // 5 минут
                            }).next(function (response) {
                                me.controller.getStore(me.storeName).load();
                                me.controller.unmask();
                                form.close();
                                Ext.Msg.alert('Сохранено!', 'Список договоров ресурсоснабжения успешно сформирован');
                                return true;
                            }).error(function (response) {
                                me.controller.unmask();
                                form.close();
                                Ext.Msg.alert('Ошибка!', response.message);
                            });

                        }
                    } else {
                        var errorMessage = this.getFormErrorMessage(form);
                        Ext.Msg.alert('Ошибка сохранения!', errorMessage);
                    }
                }
            }
        }
    ],

    refs: [
        { ref: 'mainView', selector: 'contractperiodgrid' }
    ],

    init: function (){
        var me = this,
            actions = {};

        me.callParent(arguments);

        actions['contractperiodgrid button[action=Actualize]'] = { 'click': { fn: me.actualize, scope: me } };

        me.control(actions);
    },

    index: function () {
        var me = this,
            view = this.getMainView() || Ext.widget('contractperiodgrid');

        this.bindContext(view);
        this.application.deployView(view);

        me.getStore('chargessplitting.contrpersumm.ContractPeriod').load();
    },

    actualize: function(button) {
        var me = this,
            grid = button.up('contractperiodgrid');

        var selected = grid.getSelectionModel().getLastSelected();

        if (selected) {
            me.mask();

            B4.Ajax.request({
                url: B4.Url.action('Actualize', 'ContractPeriod'),
                params: {
                    id: selected.get('Id')
                },
                timeout: 5 * 60 * 1000 // 5 минут
            }).next(function () {
                me.unmask();
                me.getStore('chargessplitting.contrpersumm.ContractPeriod').load();
                Ext.Msg.alert('Успешно!', 'Список договоров ресурсоснабжения успешно обновлен');
            }).error(function (response) {
                me.unmask();
               
                Ext.Msg.alert('Ошибка!', response.message);
            });

        } else {
            Ext.Msg.alert('Внимание!', 'Необходимо выбрать период');
        }
    }
});