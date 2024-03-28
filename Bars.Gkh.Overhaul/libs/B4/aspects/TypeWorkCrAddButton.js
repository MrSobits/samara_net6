/*
Данный аспект предназначен только для добавления вида работ в объекте КР
В Аспект вынес потомучто ДПКР несколько модулей и контроллер везде перекрывается прост очтобы не изменять каждый раз контроллер
а аспект заменить
*/

Ext.define('B4.aspects.TypeWorkCrAddButton', {
    extend: 'B4.aspects.GkhMultiSelectWindow',

    alias: 'widget.typeworkcraddbuttonaspect',

    requires: [
        'B4.store.dict.WorkSelect',
        'B4.store.dict.WorkSelected',
        'B4.store.version.Stage1GroupingForSelect',
        'B4.store.version.Stage1GroupingForSelected',
        'B4.view.objectcr.TypeWorkCrMultiSelectWindow',
        'B4.model.objectcr.TypeWorkCr'
    ],
    
    gridTypeWorkSelector: 'objectcr_type_work_cr_grid',
    
    buttonSelector: 'objectcr_type_work_cr_grid button[name=AddButtonDpkr] menuitem',
    filterSelector: '#typeWorkCrAddMultiSelectWindow b4selectfield[name=FinanceSource]',

    titleSelectWindow: 'Выбор вида работ',
    titleGridSelect: 'Виды работ для отбора',
    titleGridSelected: 'Выбранные виды работ',
    
    multiSelectWindowSelector: '#typeWorkCrAddMultiSelectWindow',
    multiSelectWindow: 'objectcr.TypeWorkCrMultiSelectWindow',
    
    init: function (controller) {
        var me = this,
            actions = {};

        me.callParent(arguments);

        actions[me.buttonSelector] = { 'click': { fn: me.btnAction, scope: me } };
        actions[me.filterSelector] = { 'change': { fn: me.updateGrid, scope: me } };

        this.addEvents('savetypework');
        
        controller.control(actions);
    },

    updateGrid: function () {
        this.getSelectGrid().getStore().load();
    },
    
    onBeforeLoad: function (store, operation) {
        var me = this,
            form = me.getForm();
        
        if (form.params && form.params.objectCrId > 0) {
            operation.params.objectCrId = form.params.objectCrId;
        }

        operation.params.isAdditionalWorks = true;
        operation.params.isActual = true;
        var finSource = this.getSelectGrid().down('b4selectfield[name=FinanceSource]').value;
        operation.params.financeSourceId = finSource ? finSource.Id : 0;     
    },
    
    btnAction: function (btn) {
        var me = this,
            action = btn.action,
            objectCrId = me.controller.getContextValue(me.controller.getMainComponent(), 'objectcrId'),
            typeWorkKind = me.controller.getContextValue(me.controller.getMainComponent(), 'typeWorkKind');
    
            // Если программа создана на основе ДПКР то показывае
        if (action === 'WorkDpkr') {
            me.storeSelect = 'version.Stage1GroupingForSelect';
            me.storeSelected = 'version.Stage1GroupingForSelected';
            me.columnsGridSelect = [
                { header: 'Вид работы', xtype: 'gridcolumn', dataIndex: 'WorkName', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'ООИ', xtype: 'gridcolumn', dataIndex: 'CeoName', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'КЭ', xtype: 'gridcolumn', dataIndex: 'StructElementName', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Год', xtype: 'gridcolumn', dataIndex: 'CorrectionYear', flex: 1, filter: { xtype: 'textfield' } }
            ];

            me.columnsGridSelected = [
                { header: 'Вид работы', xtype: 'gridcolumn', dataIndex: 'WorkName', flex: 1, sortable: false }
            ];      

        } else if (action === 'ServiceAdditionalWork') {
            me.storeSelect = 'dict.WorkSelect';
            me.storeSelected = 'dict.WorkSelected';
            me.columnsGridSelect = [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ];

            me.columnsGridSelected = [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ];
        } else {
            Ext.Msg.alert('Внимание!', 'Действие не определено');
            return;
        }
        
        me.selModelMode = 'SINGLE';

        var form = me.getForm();
            
        form.params = {};
        form.params.btnAction = action;
        form.params.objectCrId = objectCrId;
        form.params.typeWorkKind = typeWorkKind;
            
        form.show();

        var grid = me.getSelectedGrid();
            
        if (grid)
            grid.getStore().removeAll();

        me.updateSelectGrid();
            
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
            typeWorkKind = form.params.typeWorkKind,
            storeSelected,
            record;

        if (grid) {
            
            // в качестве данных возвращаются records из выбранных
            storeSelected = grid.getStore();
              //Убираем обязательность разреза финансирования тикет 384550
            //if (!finSrcValue) {
            //    Ext.Msg.alert('Ошибка!', 'Необходимо выбрать источник финансирования');
            //    return false;
            //}

            if (storeSelected.data.length > 0) {

                storeSelected.data.each(function (r) {
                    record = r;
                });
                
                if (form.params.btnAction === 'WorkDpkr') {

                    if (typeWorkKind == 10) {
                        me.showAddHistoryForm(record, finSrcValue);
                    } else {

                        me.controller.mask('Добавление', gridTypeWorks);
                        B4.Ajax.request({
                            method: 'POST',
                            url: B4.Url.action('AddWorks', 'Dpkr'),
                            timeout: 9999999,
                            params: {
                                workId: record.get('WorkId'),
                                objectCrId: form.params.objectCrId,
                                stage1Id: record.get('Stage1Id'),
                                finSrcId: finSrcValue,
                                year: record.get('CorrectionYear')
                            }
                        }).next(function(response) {

                            me.fireEvent('savetypework');

                            var resp = Ext.JSON.decode(response.responseText);

                            model.load(resp.Id, {
                                success: function(rec) {

                                    me.controller.unmask();
                                    gridTypeWorks.fireEvent('rowaction', gridTypeWorks, 'edit', rec);

                                },
                                failure: function() {
                                    me.controller.unmask();
                                },
                                scope: me
                            });

                        }).error(function(e) {
                            me.controller.unmask();
                            Ext.Msg.alert('Ошибка при добавлении', e.message || e);
                        });
                    }

                } else if (form.params.btnAction === 'ServiceAdditionalWork') {
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
                }

                form.close();
            } else {
                Ext.Msg.alert('Ошибка!', 'Необходимо выбрать вид работы');
                return false;
            }
        }
    },

    showAddHistoryForm: function (record, finSourceValue) { }
});