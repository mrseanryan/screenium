/** @constructor
* @description Create the divs for the parts, and also find them (PageObject pattern) so they can be cached & populated.
* Means we can manage the part Ids in one class.
*/
var LogPlotHeaderLayoutPartsCreator = function (config) {
    if (!config) {
        throw 'bad args!';
    }

    this._config = config;
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.createDivHtmlForMetadata = function () {
    //TODO could use grid for the 2x2 layout
    return "<div class='logPlotMetaDataIndicators'>"+"<div>b</div><div>l</div>"+"</div>"
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.createDivHtmlForValueAxis = function () {
    return "<div class='logPlotValueAxis'>{value axis here!}</div>"
};

/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.createDivHtmlForName = function () {
    return "<div class='logPlotSerieName'>{name}</div>";
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.createDivHtmlForUnits = function () {
    return "<div class='logPlotSerieUnits'>[u]</div>";
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.getMetaDataWidth = function () {
    //TODO ask config how many indicators we have (2x2 layout)
    return 26;
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.getHeightOfValueAxis = function () {
    return 25;
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype._findAndCheckDiv = function (container, selector) {
    var found = container.find(selector);
    if (found.length === 0) {
        throw 'could not find the div!';
    }
    if (found.length > 1) {
        throw 'found more than 1 matching div!';
    }
    return found;
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.findDivHtmlForMetadata = function (container) {
    this._findAndCheckDiv(container, '.logPlotMetaDataIndicators');
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.findDivHtmlForValueAxis = function () {
    this._findAndCheckDiv(container, '.logPlotValueAxis');
};

/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.findDivHtmlForName = function () {
    this._findAndCheckDiv(container, '.logPlotSerieName');
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.findDivHtmlForUnits = function () {
    this._findAndCheckDiv(container, '.logPlotSerieUnits');
};
