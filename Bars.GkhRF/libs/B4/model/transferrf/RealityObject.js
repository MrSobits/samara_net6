Ext.define('B4.model.transferrf.RealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: [ 'B4.enums.YesNoNotSet',
                'B4.enums.HeatingSystem',
                'B4.enums.ConditionHouse',
                'B4.enums.TypeHouse',
                'B4.enums.TypeRoof'
              ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectForTransfer'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Municipality', defaultValue: null },
        { name: 'Address' },
        { name: 'CodeERC', defaultValue: null },
        { name: 'AreaLiving' },
        { name: 'AreaLivingOwned' },
        { name: 'AreaLivingNotLivingMKD' },
        { name: 'AreaMKD' },
        { name: 'AreaBasement' },
        { name: 'DateLastOverhaul' },
        { name: 'DateCommissioning' },
        { name: 'CapitalGroup', defaultValue: null },
        { name: 'DateDemolition' },
        { name: 'MaximumFloors' },
        { name: 'RoofingMaterial', defaultValue: null },
        { name: 'WallMaterial', defaultValue: null },
        { name: 'IsInsuredObject', defaultValue: false },
        { name: 'Notation' },
        { name: 'SeriesHome' },
        { name: 'PhysicalWear' },
        { name: 'TypeOwnership', defaultValue: null },
        { name: 'Floors' },
        { name: 'FederalNum' },
        { name: 'NumberApartments' },
        { name: 'NumberEntrances' },
        { name: 'NumberLifts' },
        { name: 'NumberLiving' },
        { name: 'HavingBasement', defaultValue: 30 },
        { name: 'HeatingSystem', defaultValue: 10 },
        { name: 'ConditionHouse', defaultValue: 10 },
        { name: 'TypeHouse', defaultValue: 10 },
        { name: 'TypeRoof', defaultValue: 10 },
        { name: 'GkhCode'}
    ]
});