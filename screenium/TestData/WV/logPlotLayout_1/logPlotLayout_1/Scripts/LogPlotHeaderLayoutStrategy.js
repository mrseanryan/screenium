/// <reference path="LogPlotHeaderLayoutPartsCreator.js" />
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
        return this._isAtTop ? new LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtTop(this._partsCreator, this._config) : new LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom(this._partsCreator, this._config);
    }

    //name/unit With Value
    if(this._config.getShowSnapshotValue()) {
        return this._isAtTop ? new LogPlotHeaderLayoutStrategyNameUnitWithValueAtTop(this._partsCreator, this._config) : new LogPlotHeaderLayoutStrategyNameUnitWithValueAtBottom(this._partsCreator, this._config);
    }
    //metadata only
    return new LogPlotHeaderLayoutStrategyMetadataOnly(this._partsCreator, this._config);
};

/** @function
* @description name/unit/metadata NO value
* [name][Md12/34][unit]
* [value axis]
*/
var LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtTop = function (_partsCreator, _config) {
    if (!_partsCreator || !_config) {
        throw 'bad args!';
    }
    this._partsCreator = _partsCreator;
    this._config = _config
};
/** @function
*/
LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtTop.prototype.createLayoutHtml = function () {
    //TODO handle on/off:
    //this._config.getShowName
    //this._config.getShowUom
    //this._config.getShowMetadataIndicators

    //this._partsCreator
    return "TODO 4";
};

/** @function
* @description name/unit/metadata NO value
* [value axis]
* [name][Md12/34][unit]
*/
var LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom = function (_partsCreator, _config) {
    if (!_partsCreator || !_config) {
        throw 'bad args!';
    }
    this._partsCreator = _partsCreator;
    this._config = _config
};
/** @function
*/
LogPlotHeaderLayoutStrategyNameUnitMetadataNoValueAtBottom.prototype.createLayoutHtml = function () {
    //TODO handle on/off:
    //this._config.getShowName
    //this._config.getShowUom
    //this._config.getShowMetadataIndicators

    //this._partsCreator
    return "TODO 3";
};

/** @function
* @description name/unit With Value
[name][unit]
[value][Md12/34]
[value axis]
*/
var LogPlotHeaderLayoutStrategyNameUnitWithValueAtTop = function (_partsCreator, _config) {
    if (!_partsCreator || !_config) {
        throw 'bad args!';
    }
    this._partsCreator = _partsCreator;
    this._config = _config
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
    return "TODO 2";
};
/** @function
* @description name/unit With Value
[value axis]
[value][Md12/34]
[name][unit]
*/
var LogPlotHeaderLayoutStrategyNameUnitWithValueAtBottom = function (_partsCreator, _config) {
    if (!_partsCreator || !_config) {
        throw 'bad args!';
    }
    this._partsCreator = _partsCreator;
    this._config = _config
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
    return "TODO 1";
};

/** function
* @description metadata only:
* [value axis][Md12/34]
*/
var LogPlotHeaderLayoutStrategyMetadataOnly = function (_partsCreator, _config) {
    if (!_partsCreator || !_config) {
        throw 'bad args!';
    }
    this._partsCreator = _partsCreator;
    this._config = _config
};
/** @function
*/
LogPlotHeaderLayoutStrategyMetadataOnly.prototype.createLayoutHtml = function () {
    var html = '';
    var heightOfValueAxis = this._partsCreator.getHeightOfValueAxis();
    if (this._config.getShowMetadataIndicators()) {
        //[value axis][Md12/34]
        var metaDataWidth = this._partsCreator.getMetaDataWidth();
        html += "<div class='pure-g pure-g-valign-fix logPlotHeaderSerieCellTopBox' >" +
        "<div class='pure-u-24-24' style='width: calc(100% - " + metaDataWidth + "px); height: " + heightOfValueAxis + "px; min-height: " + heightOfValueAxis + "px;'>" + this._partsCreator.createDivHtmlForValueAxis() + "</div>" +
        "<div style='width:" + metaDataWidth + "px; height: 100%; float: right;'>" + this._partsCreator.createDivHtmlForMetadata() + "</div>";
        html += "</div>";
    } else {
        //[value axis]
        html += "<div class='pure-g pure-g-valign-fix logPlotHeaderSerieCellTopBox' >" +
            "<div class='pure-u-24-24' style='height: " + heightOfValueAxis + "px; min-height: " + heightOfValueAxis + "px;'>" + this._partsCreator.createDivHtmlForValueAxis() + "</div>";
        html += "</div>";
    }

    return html;
};