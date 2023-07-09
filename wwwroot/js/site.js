// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$.fn.jQuerySimpleCounter = function (options) {
	var settings = $.extend({
		start: 0,
		end: 100,
		easing: 'swing',
		duration: 400,
		complete: ''
	}, options);

	var thisElement = $(this);

	$({ count: settings.start }).animate({ count: settings.end }, {
		duration: settings.duration,
		easing: settings.easing,
		step: function () {
			var mathCount = Math.ceil(this.count);
			thisElement.text(mathCount);
		},
		complete: settings.complete
	});
};

