/** @constructor
* @description Create the divs for the parts, and also find them (PageObject pattern) so they can be cached & populated.
* Means we can manage the part Ids in one class.
*/
var LogPlotHeaderLayoutPartsCreator = function (config, headerContainerDiv, serieId) {
    if (!config || !headerContainerDiv || typeof serieId === 'undefined') {
        throw 'bad args!';
    }

    this._config = config;

    //the container of the set of series for this column (at top or at bottom).
    //we later use the combination of headerContainerDiv + serieId to find the container div for this series.
    this._headerContainerDiv = headerContainerDiv;
    this._serieId = serieId;
};
/**
 * @function
 */
LogPlotHeaderLayoutPartsCreator.prototype.getIdOfSerieDiv = function() {
    return 'headerSerie' + this._serieId;
};

/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.createDivHtmlForMetadata = function () {
    //TODO could use grid for the 2x2 layout
    return "<div class='logPlotMetaDataIndicators'>" + "<div>bl</div>" + "</div>";
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.createDivHtmlForValueAxis = function () {
    return "<div class='logPlotValueAxis'>{value axis here!}</div>";
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.createDivHtmlForValue = function () {
    return "<div class='logPlotValue logPlotHeaderLargeText logPlotHeaderCenteredText logPlotHeaderAutoTruncatedText'>123.456789</div>";
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.createDivHtmlForName = function () {
    return "<div class='logPlotSerieName logPlotHeaderAutoTruncatedText'>{name}</div>";
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.createDivHtmlForUnits = function () {
    return "<div class='logPlotSerieUnits logPlotHeaderAutoTruncatedText logPlotHeaderTextAlignRight'>[u]</div>";
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
LogPlotHeaderLayoutPartsCreator.prototype._findAndCheckDiv = function (selector) {
    //TODO xxx perf - cache the jQuery object by selector (especially for tooltips)

    this._serieContainerDiv = this._serieContainerDiv || this._headerContainerDiv.find('#'+this.getIdOfSerieDiv());
    if(this._serieContainerDiv.length === 0) {
        throw 'could not find the serie div!';
    }
    if (this._serieContainerDiv.length > 1) {
        throw 'found more than 1 matching serie div!';
    }

    var found = this._serieContainerDiv.find(selector);
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
LogPlotHeaderLayoutPartsCreator.prototype.findDivHtmlForMetadata = function () {
    return this._findAndCheckDiv('.logPlotMetaDataIndicators');
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.findDivHtmlForValueAxis = function () {
    return this._findAndCheckDiv('.logPlotValueAxis');
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.findDivHtmlForValue = function () {
    return this._findAndCheckDiv('.logPlotValue');
};

/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.findDivHtmlForName = function () {
    return this._findAndCheckDiv('.logPlotSerieName');
};
/** @function
*/
LogPlotHeaderLayoutPartsCreator.prototype.findDivHtmlForUnits = function () {
    return this._findAndCheckDiv('.logPlotSerieUnits');
};
