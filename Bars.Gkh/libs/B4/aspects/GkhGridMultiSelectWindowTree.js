/*
   Данный аспект предназначен для описания взаимодействия грида и формы массового выбора элементов. Сохранение здесь пустой метод
   поскольку обработку сохранения необходимо писать в тех контроллерах где этот аспект применяется.
   Данный аспект преминяется там где необходимо чтобы при нажатии на кнопку добавить показалась форма массового выбора
   Но в контроллерах необходимо вешаться на событие getdata и получив массив записей произвести свои действия
*/

//TODO: сделать отдельным аспектом, без наследования от GkhMultiSelectWindow
Ext.define('B4.aspects.GkhGridMultiSelectWindowTree', {
    extend: 'B4.aspects.GkhGridMultiSelectWindow',

    alias: 'widget.gkhmultiselectwindowtreeaspect',

    init: function (controller) {
        var me = this,
            actions = {};
        actions[me.multiSelectWindowSelector + ' #btnMarkAll'] = { 'click': { fn: me.onClickMarkAll, scope: me } };
        actions[me.multiSelectWindowSelector + ' #btnUnmarkAll'] = { 'click': { fn: me.onClickUnmarkAll, scope: me } };
        actions[me.multiSelectWindowSelector + ' #tpSelect #btnUpdateTree'] = { 'click': { fn: me.onClickUpdate, scope: me } };
        actions[me.multiSelectWindowSelector + ' #tpSelect'] = { 'checkchange': { fn: me.onCheckRec, scope: me } };

        controller.control(actions);
        me.callParent(arguments);
    },

    onClickUpdate: function () {
        var me = this;
        me.clearFilter();
        me.filterBy(Ext.ComponentQuery.query(this.multiSelectWindowSelector + ' #tpSelect #tfSearchName')[0].getValue(), 'text');
    },

    onClickMarkAll: function () {
        var me = this;
        me.getTreePanel().getRootNode().cascadeBy(function() {
            if (!Ext.isEmpty(this.get('checked'))) {
                this.set('checked', true);
            }
            me.onCheckRec(this, true);
        });
    },

    onClickUnmarkAll: function () {
        var me = this;
        me.getTreePanel()
            .getRootNode().cascadeBy(function () {
                if (!Ext.isEmpty(this.get('checked'))) {
                    this.set('checked', false);
                }
                me.onCheckRec(this, false);
            });
    },
    onCheckRec: function (node, checked) {
        var grid = this.getSelectedGrid(),
            storeSelected = grid.getStore(),
            model = this.controller.getModel(this.modelName);
        //если элемент конечный то добавляем в стор выбранных
        if (grid && node.get('leaf')) {
            if (checked) {
                if (storeSelected.find('Id', node.get('id'), 0, false, false, true) == -1)
                    storeSelected.add(new model({ Id: node.get('id'), Name: node.get('text') }));
            } else {
                storeSelected.remove(storeSelected.getById(node.get('id')));
            }
        }
    },

    getTreePanel: function() {
        return Ext.ComponentQuery.query(this.multiSelectWindowSelector + ' #tpSelect')[0];
    },

    updateSelectGrid: function () {
        
    },

    updateGrid: function () {
        this.controller.getStore(this.storeName).load();
    },
    
    clearFilter: function () {
        var treePanel = this.getTreePanel(),
            view = treePanel.getView();

        treePanel.collapseAll();
        treePanel.getRootNode().cascadeBy(function (tree, view) {
            var uiNode = view.getNodeByRecord(this);
            if (uiNode) {
                Ext.get(uiNode).setDisplayed('table-row');
            }
        }, null, [this, view]);
    },
    
    filterBy: function (text, by) {
        var treePanel = this.getTreePanel(),
            view = treePanel.getView(),
            nodesAndParents = [];

        // Find the nodes which match the search term, expand them.
        // Then add them and their parents to nodesAndParents.
        treePanel.getRootNode().cascadeBy(function (tree) {
            var currNode = this;
            if (currNode && currNode.get(by) && currNode.get(by).toString().toLowerCase().indexOf(text.toLowerCase()) > -1) {
                treePanel.expandPath(currNode.getPath());

                if (currNode.hasChildNodes()) {
                    currNode.eachChild(function (child) {
                        nodesAndParents.push(child.id);
                    });
                }
                while (currNode.parentNode) {
                    nodesAndParents.push(currNode.id);
                    currNode = currNode.parentNode;
                }
            }
        }, null, [treePanel, view]);

        // Hide all of the nodes which aren't in nodesAndParents
        treePanel.getRootNode().cascadeBy(function (tree) {
            var uiNode = view.getNodeByRecord(this);
            if (uiNode && !Ext.Array.contains(nodesAndParents, this.id)) {
                Ext.get(uiNode).setDisplayed('none');
            }
        });
    }
});