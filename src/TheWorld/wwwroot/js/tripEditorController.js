



(function () {

	"use strict";

	angular.module("app-trips")
		.controller("tripEditorController", tripEditorController);

	function tripEditorController($routeParams, $http) {
		var vm = this;

		vm.tripName = $routeParams.tripName;
		vm.stops = [];
		vm.newStop = {};
		vm.errorMessage = "";
		vm.isBusy = true;

	    var url = "/api/trips/" + vm.tripName + "/stops/";

		vm.addStop = function() {

			vm.isBusy = true;
			vm.errorMessage = "";

			$http.post( url, vm.newStop)
				.then(function(response) {
//success
					vm.stops.push(response.data);
			            _showMap(vm.stops);
						vm.newStop = {};

					},
					function(error) {
//failure
						vm.errorMessage = "Failed to save the stop" + error;
					})
				.finally(function() {
					vm.isBusy = false;
				});
		};

		$http.get(url)
			.then(function(response) {
					//success
				angular.copy(response.data, vm.stops);
					_showMap(vm.stops);
				},
				function(error) {

					//failurre
					vm.errorMessage = "Failed To load stops" + error;
				})
		.finally(function() {
				vm.isBusy = false;
			});


	}

	 function _showMap(stops) {
		 
		if (stops && stops.length > 0) {

			 var mapStops = _.map(stops, function(item) {
				 return {
					lat: item.latitude,
					long: item.longitude,
					info: item.name
				 };
			 });

			travelMap.createMap({
				stops: mapStops,
				selector: "#map",
				currentStop: 0,
				initialZoom: 3
			});
		}

	 }

})();