//tripsController.js

(function() {

	"use strict";

	//Getting the existin module
	angular.module("app-trips")
		.controller("tripsController", tripsController);

	function tripsController($http) {

		var vm = this;

		vm.trips = [];

		vm.newTrip = {};

		vm.errorMessage = "";
		vm.isBusy = true;

		vm.addTrip = function () {

	   	        vm.isBusy = true;
	            vm.errorMessage = "";

	        $http.post("/api/trips", vm.newTrip)
	            .then(function(response) {
	                    //success
	            	vm.trips.push(response.data);
	                    vm.newTrip = {};
	                },
	                function(error) {
	                    //error
	                    vm.errorMessage = "Failed to save new trip" + error;
	                })
	            .finally(function() {
	                    vm.isBusy = false;
	                }
	            );
	    };

		$http.get("/api/trips")
			.then(function(response) {
				//on success
					angular.copy(response.data, vm.trips);
				},
				function(error) {
					//on failure
				    vm.errorMessage = "Failed to load data" + error;
				})
			.finally(function() {
		        vm.isBusy = false;
		    });

	}

})();