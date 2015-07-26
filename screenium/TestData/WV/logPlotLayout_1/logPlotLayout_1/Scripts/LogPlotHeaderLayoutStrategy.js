/** @function
* @description Factory to create the appropriate layout strategy for the given config.
*/
var LogPlotHeaderLayoutStrategyFactory = function (seriesHeaderConfig, isAtTop, partsCreator) {
    if (!seriesHeaderConfig || typeof isAtTop === 'undefined' || !partsCreator) {
        throw 'bad args!';
    }

    this._config = seriesHeaderConfig;
    this._isAtTop = isAtTop;
    this._partsCreator = partsCreator;
};

LogPlotHeaderLayoutStrategyFactory.prototype.create = function() {
    //name/unit/metadata NO value
    if((this._config.getShowName() || this._config.getShowUom()) && !this._config.getShowSnapshotValue()) {
        return this._isAtTop ? new LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtTop(this._partsCreator) : new LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom(this._partsCreator);
    }

    //name/unit With Value
    if((this._config.getShowName() || this._config.getShowUom()) && this._config.getShowSnapshotValue()) {
        return this._isAtTop ? new LogPlotHeaderLayoutStrategyNameUnitWithValueAtTop(this._partsCreator) : new LogPlotHeaderLayoutStrategyNameUnitWithValueAtBottom(this._partsCreator);
    }

    //metadata only
    return new LogPlotHeaderLayoutStrategyMetadataOnly(this._partsCreator);
};

/** @function
* @description name/unit/metadata NO value
* [name][Md12/34][unit]
* [value axis]
*/
var LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtTop = function (_partsCreator) {
    if (!_partsCreator) {
        throw 'bad args!';
    }
    this._partsCreator = _partsCreator;
};
/** @function
*/
LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtTop.prototype.createLayoutHtml = function () {
    //TODO handle on/off:
    //this._config.getShowName
    //this._config.getShowUom
    //this._config.getShowMetadataIndicators

    //this._partsCreator
};

/** @function
* @description name/unit/metadata NO value
* [value axis]
* [name][Md12/34][unit]
*/
var LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom = function (_partsCreator) {
    if (!_partsCreator) {
        throw 'bad args!';
    }
    this._partsCreator = _partsCreator;
};
/** @function
*/
LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom.prototype.createLayoutHtml = function () {
    //TODO handle on/off:
    //this._config.getShowName
    //this._config.getShowUom
    //this._config.getShowMetadataIndicators

    //this._partsCreator
};

/** @function
* @description name/unit With Value
[name][unit]
[value][Md12/34]
[value axis]
*/
var LogPlotHeaderLayoutStrategyNameUnitWithValueAtTop = function (_partsCreator) {
    if (!_partsCreator) {
        throw 'bad args!';
    }
    this._partsCreator = _partsCreator;
};
/** @function
*/
LogPlotHeaderLayoutStrategyNameUnitWithValueAtTop.prototype.createLayoutHtml = function () {
    //TODO handle on/off:
    //this._config.getShowName
    //this._config.getShowUom
    //this._config.getShowMetadataIndicators
    //this._config.getShowSnapshotValue

    //this._partsCreator
};
/** @function
* @description name/unit With Value
[value axis]
[value][Md12/34]
[name][unit]
*/
var LogPlotHeaderLayoutStrategyNameUnitWithValueAtBottom = function (_partsCreator) {
    if (!_partsCreator) {
        throw 'bad args!';
    }
    this._partsCreator = _partsCreator;
};
/** @function
*/
LogPlotHeaderLayoutStrategyNameUnitWithValueAtBottom.prototype.createLayoutHtml = function () {
    //TODO handle on/off:
    //this._config.getShowName
    //this._config.getShowUom
    //this._config.getShowMetadataIndicators
    //this._config.getShowSnapshotValue

    //this._partsCreator
};

/** function
* @description metadata only:
* [value axis][Md12/34]
*/
var LogPlotHeaderLayoutStrategyMetadataOnly = function (_partsCreator) {
    if (!_partsCreator) {
        throw 'bad args!';
    }
    this._partsCreator = _partsCreator;
};
/** @function
*/
LogPlotHeaderLayoutStrategyMetadataOnly.prototype.createLayoutHtml = function () {
    //TODO handle on/off:
    //this._config.getShowMetadataIndicators

    //this._partsCreator
};