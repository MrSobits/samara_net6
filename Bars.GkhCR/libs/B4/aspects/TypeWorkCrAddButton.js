/*
Данный аспект предназначен только для добавления вида работ в объекте КР
В Аспект вынес потомучто ДПКР несколько модулей и контроллер везде перекрывается прост очтобы не изменять каждый раз контроллер
а аспект заменить
В модуле Overhaul данный аспект заменяется
*/

Ext.define('B4.aspects.TypeWorkCrAddButton', {
    extend: 'B4.aspects.GkhMultiSelectWindow',

    alias: 'widget.typeworkcraddbuttonaspect',

    requires: [
        'B4.store.dict.WorkSelect',
        'B4.store.dict.WorkSelected',
        'B4.view.objectcr.TypeWorkCrMultiSelectWindow',
        'B4.model.objectcr.TypeWorkCr'
    ],
    
    gridTypeWorkSelector: 'objectcr_type_work_cr_grid',
    
    buttonSelector: 'objectcr_type_work_cr_grid button[name=AddButtonDpkr] menuitem',

    titleSelectWindow: 'Выбор вида работ',
    titleGridSelect: 'Виды работ для отбора',
    titleGridSelected: 'Выбранные виды работ',
    
    multiSelectWindowSelector: '#typeWorkCrAddMultiSelectWindow',
    multiSelectWindow: 'objectcr.TypeWorkCrMultiSelectWindow',
    
    init: function (controller) {

        var actions = {};
        this.callParent(arguments);

        actions[this.buttonSelector] = { 'click': { fn: this.btnAction, scope: this } };
        
        controller.control(actions);
    },
    
    onBeforeLoad: function (store, operation) {

        /*тут работа ведется тольк осо стором по выбору услуг и  доп работ*/
        operation.params.isAdditionalWorks = true;
    },
    
    btnAction: function (btn) {
        var me = this,
            action = btn.action,
            objectCrId = me.controller.params.get('Id');

        if (action === 'ServiceAdditionalWork') {
            me.storeSelect = 'dict.WorkSelect';
            me.storeSelected = 'dict.WorkSelected';
            me.columnsGridSelect = [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ];

            me.columnsGridSelected = [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ];

            var form = me.getForm();

            form.params = {};
            form.params.objectCrId = objectCrId;

            form.show();

            var grid = me.getSelectedGrid();

            if (grid)
                grid.getStore().removeAll();

            me.updateSelectGrid();
        } else if (action === 'WorkDpkr') {
            /*
                Поскольку данный аспект находится в базовом модуле Cr то речи о ДПКр тут быть не может
            */
                
            Ext.Msg.alert('Внимание!', 'Действие не доступно, поскольку отсутсвует модуль ДПКР');
            return false;
        } else {
            Ext.Msg.alert('Внимание!', 'Действие не определено');
            return false;
        }
 
    },
    
    onRowSelect: function (rowModel, record) {
        var me = this,
            grid = me.getSelectedGrid();
        
        if (grid) {
            var storeSelected = grid.getStore();

            storeSelected.removeAll();

            storeSelected.add(record);
        }
    },
    
    onSelectRequestHandler: function (btn) {
        var me = this,
            form = me.getForm(),
            finSrcValue = form.down('b4selectfield[name=FinanceSource]').getValue(),
            grid = me.getSelectedGrid(),
            gridTypeWorks = me.componentQuery(me.gridTypeWorkSelector),
            model = me.controller.getModel('objectcr.TypeWorkCr'),
            storeSelected,
            record;

        if (grid) {
            
            // в качестве данных возвращаются records из выбранных
            storeSelected = grid.getStore();
            //Убираем обязательность разреза финансирования тикет 384550
            //if (!finSrcValue) {
            //    Ext.Msg.alert('Ошибка!', 'Необходимо выбрать источник финансирования');
            //    return;
            //}

            if (storeSelected.data.length > 0) {

                storeSelected.data.each(function (r) {
                    record = r;
                });
                
                var rec = new model();
                rec.set('ObjectCr', form.params.objectCrId);
                rec.set('Work', record.get('Id'));
                rec.set('FinanceSource', finSrcValue);

                me.controller.mask('Добавление', gridTypeWorks);
                rec.save()
                    .next(function (result) {
                        me.controller.unmask();
                        gridTypeWorks.getStore().load();

                        gridTypeWorks.fireEvent('rowaction', gridTypeWorks, 'edit', result.record);
                    }, this)
                    .error(function (result) {
                        me.controller.unmask();
                            
                        Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                    }, this);
                
                form.close();
            } else {
                Ext.Msg.alert('Ошибка!', 'Необходимо выбрать вид работы');
                return false;
            }
        }
    }
});