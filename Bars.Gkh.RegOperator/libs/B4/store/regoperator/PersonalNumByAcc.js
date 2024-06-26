﻿Ext.define('B4.store.regoperator.PersonalNumByAcc', {
    extend: 'B4.base.Store',
    requires: ['B4.model.regoperator.AccountByRo'],
    autoLoad: false,
    model: 'B4.model.regoperator.AccountByRo',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BasePersonalAccount',
        listAction: 'GetPersonalNumByAccount'
    }
});